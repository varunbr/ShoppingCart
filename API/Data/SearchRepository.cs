using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Services;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Type = API.Helpers.Type;

namespace API.Data
{
    public class SearchRepository : BaseRepository, ISearchRepository
    {
        public SearchRepository(DataContext dataContext, IMapper mapper, IPhotoService photoService) : base(dataContext, mapper, photoService)
        {
        }

        #region Search

        public async Task<Response<ProductDto, SearchContextDto>> Search(Dictionary<string, string> queryParams)
        {
            var context = new SearchContext(new Dictionary<string, string>(queryParams, StringComparer.OrdinalIgnoreCase));
            if (string.IsNullOrEmpty(context.SearchText)) throw new HttpException("Search value shouldn't be empty.");
            await GetCategory(context);
            return await GetProducts(context);
        }

        private async Task<Response<ProductDto, SearchContextDto>> GetProducts(SearchContext context)
        {
            var productsQuery = ApplyFilters(context);
            var query = from product in productsQuery
                        join productTag in DataContext.ProductTags on product.Id equals productTag.ProductId
                        select productTag;

            var inner = PredicateBuilder.False<ProductTag>();
            foreach (var keyword in context.Keywords)
            {
                inner = inner.Or(p => p.Name.Contains(keyword));
            }

            var groupQuery = query.Where(inner).GroupBy(p => p.ProductId);

            var products = context.OrderBy switch
            {
                OrderBy.HighToLow => from product in DataContext.Products
                                     join key in groupQuery.Select(g => g.Key) on product.Id equals key
                                     orderby product.Amount descending
                                     select product,
                OrderBy.LowToHigh => from product in DataContext.Products
                                     join key in groupQuery.Select(g => g.Key) on product.Id equals key
                                     orderby product.Amount
                                     select product,
                OrderBy.Latest => from product in DataContext.Products
                                  join key in groupQuery.Select(g => g.Key) on product.Id equals key
                                  orderby product.Created descending
                                  select product,
                _ => from product in DataContext.Products
                     join productGroup in groupQuery.Select(g => new { g.Key, Score = g.Sum(i => i.Score) }) on product.Id equals productGroup.Key
                     orderby productGroup.Score descending, product.SoldQuantity descending
                     select product
            };

            var resultQuery = products
                .ProjectTo<ProductDto>(Mapper.ConfigurationProvider)
                .AsNoTracking();

            resultQuery = resultQuery.AddPagination(context.PageNumber, context.PageSize);

            return Response<ProductDto, SearchContextDto>.Create(await resultQuery.ToListAsync(),
                Mapper.Map<SearchContextDto>(context));
        }

        private IQueryable<Product> ApplyFilters(SearchContext context)
        {
            var query = DataContext.Products.AsQueryable();
            if (!string.IsNullOrEmpty(context.Category))
            {
                query = query.Where(p => p.Category.Name == context.Category);

                var inner = PredicateBuilder.False<PropertyValue>();
                var count = 0;
                foreach (var filter in context.Filters)
                {
                    var property = context.Properties.First(p => p.Name == filter.Key);
                    var values = filter.Value.Split(',');

                    switch (property.Type)
                    {
                        case Type.String:
                            inner = inner.Or(pv => pv.Property.Name == filter.Key && values.Contains(pv.StringValue));
                            count++;
                            break;
                        case Type.Integer:
                            {
                                filter.Value.GetRange(out int? from, out var to);

                                if (@from != null || to != null)
                                    count++;

                                if (@from != null && to != null)
                                    inner = inner.Or(pv =>
                                        pv.Property.Name == filter.Key && pv.IntegerValue >= @from && pv.IntegerValue <= to);
                                else if (@from != null)
                                    inner = inner.Or(pv =>
                                        pv.Property.Name == filter.Key && pv.IntegerValue >= @from);
                                else if (to != null)
                                    inner = inner.Or(pv =>
                                        pv.Property.Name == filter.Key && pv.IntegerValue <= to);
                                break;
                            }
                    }
                }

                if (count > 0)
                {
                    var propertyCount = DataContext.PropertyValues
                        .Where(inner)
                        .GroupBy(p => p.ProductId)
                        .Select(g => new { g.Key, Count = g.Count() });

                    query = from product in query
                            join valueCount in propertyCount on product.Id equals valueCount.Key
                            where valueCount.Count == count
                            select product;
                }
            }


            if (context.PriceFrom != null)
                query = query.Where(p => p.Amount >= context.PriceFrom);
            if (context.PriceTo != null)
                query = query.Where(p => p.Amount <= context.PriceTo);

            if (!string.IsNullOrEmpty(context.Brand))
            {
                var values = context.Brand.Split(',');
                query = query.Where(p => values.Contains(p.Brand));
            }

            return query;
        }

        private async Task GetCategory(SearchContext context)
        {
            int? categoryId;
            if (context.Category == null)
            {
                var category = await DataContext.CategoryTags
                    .Where(t => context.Keywords.Contains(t.Name))
                    .GroupBy(t => t.Category.Name)
                    .OrderByDescending(g => g.Sum(t => t.Score))
                    .Select(g => new
                    {
                        g.Key,
                        g.FirstOrDefault().CategoryId
                    })
                    .FirstOrDefaultAsync();
                categoryId = category?.CategoryId;
                context.Category = category?.Key;
            }
            else
            {
                categoryId = await DataContext.Categories
                    .Where(c => c.Name == context.Category)
                    .Select(c => c.Id).FirstOrDefaultAsync();
            }


            if (categoryId != null)
            {
                var properties = await DataContext.Properties.Where(p => p.CategoryId == categoryId).ToListAsync();
                context.Properties = properties;
                AddFilters(context);
            }
        }

        private static void AddFilters(SearchContext context)
        {
            foreach (var param in context.QueryParams)
            {
                var property = context.Properties.FirstOrDefault(p => p.Name == param.Key && p.Filter);
                if (property == null) continue;

                switch (property.Type)
                {
                    case Type.Integer when param.Value.IsValidIntegerRange():
                    case Type.String:
                        context.Filters.Add(param.Key, param.Value);
                        break;
                }
            }
        }

        #endregion

        #region HomePage

        public async Task<HomePageDto> GetHomePage()
        {
            var context = GetHomeContext();
            var homePage = new HomePageDto();

            foreach (var category in context.Categories)
            {
                var productIds = await DataContext.Products
                    .Where(p => p.Category.Name == category && p.Available)
                    .GroupBy(p => p.Model)
                    .OrderByDescending(g => g.Average(p => p.SoldQuantity))
                    .Select(g => g.First().Id)
                    .Take(context.ItemsPerCategory)
                    .ToListAsync();

                var products = await DataContext.Products
                    .Where(p => productIds.Contains(p.Id))
                    .ProjectTo<ProductMiniDto>(Mapper.ConfigurationProvider)
                    .AsNoTracking()
                    .ToListAsync();

                products = products.OrderBy(p => productIds.IndexOf(p.Id)).ToList();

                homePage.CategoryProducts.Add(new CategoryProductDto
                {
                    Category = category,
                    Products = products
                });
            }

            var categories = await DataContext.Categories
                .Where(c => c.Products.Any())
                .OrderBy(c => c.Id)
                .ProjectTo<CategoryMiniDto>(Mapper.ConfigurationProvider)
                .ToListAsync();
            homePage.Categories = categories;

            return homePage;
        }

        private HomeContext GetHomeContext()
        {
            var context = new HomeContext
            {
                Categories = new List<string> { "Mobiles", "Television", "Laptops", "Refrigerators", "Washing Machine" }
            };
            return context;
        }

        #endregion
    }
}
