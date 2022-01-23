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

        public async Task<Response<ProductDto, SearchContextDto>> Search(Dictionary<string, string> queryParams)
        {
            var context = new SearchContext(new Dictionary<string, string>(queryParams, StringComparer.OrdinalIgnoreCase));
            if (string.IsNullOrEmpty(context.SearchText)) throw new HttpException("Search value shouldn't be empty.");
            await GetCategory(context);
            return await GetProducts(context);
        }

        private async Task<Response<ProductDto, SearchContextDto>> GetProducts(SearchContext context)
        {
            var query = DataContext.ProductTags.AsQueryable();
            query = ApplyFilters(query, context);

            var inner = PredicateBuilder.False<ProductTag>();
            foreach (var keyword in context.Keywords)
            {
                inner = inner.Or(p => p.Name.Contains(keyword));
            }

            var group = query.Where(inner)
                .GroupBy(t => t.ProductId)
                .Select(g => new
                {
                    g.Key,
                    Score = g.Sum(p => p.Score),
                    Price = g.First().Product.Amount,
                    g.First().Product.Created,
                    g.First().Product.SoldQuantity
                });

            var order = context.OrderBy switch
            {
                OrderBy.HighToLow => group.OrderByDescending(g => g.Price),
                OrderBy.LowToHigh => group.OrderBy(g => g.Price),
                OrderBy.Latest => group.OrderByDescending(g => g.Created),
                _ => group.OrderByDescending(g => g.Score).ThenByDescending(g => g.SoldQuantity)
            };

            var productIds = await order.AddPagination(context.PageNumber, context.PageSize)
                .Select(g => g.Key)
                .ToListAsync();

            var products = await DataContext.Products
                .Where(p => productIds.Contains(p.Id))
                .ProjectTo<ProductDto>(Mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync();

            var result = products.OrderBy(p => productIds.IndexOf(p.Id)).ToList();

            return Response<ProductDto, SearchContextDto>.Create(result, Mapper.Map<SearchContextDto>(context));
        }

        private IQueryable<ProductTag> ApplyFilters(IQueryable<ProductTag> query, SearchContext context)
        {
            if (!string.IsNullOrEmpty(context.Category))
            {
                query = query.Where(t => t.Product.Category.Name == context.Category);
                foreach (var filter in context.Filters)
                {
                    var property = context.Properties.First(p => p.Name == filter.Key);
                    var values = filter.Value.Split(',');

                    switch (property.Type)
                    {
                        case Type.String:
                            query = query.Where(t =>
                                t.Product.Properties.Any(p => p.Property.Name == filter.Key && values.Contains(p.StringValue)));
                            break;
                        case Type.Integer:
                            {
                                filter.Value.GetRange(out int? from, out var to);
                                if (from != null)
                                    query = query.Where(t =>
                                        t.Product.Properties.Any(p => p.Property.Name == filter.Key && p.IntegerValue >= from));
                                if (to != null)
                                    query = query.Where(t =>
                                        t.Product.Properties.Any(p => p.Property.Name == filter.Key && p.IntegerValue <= to));
                                break;
                            }
                    }
                }
            }

            if (context.PriceFrom != null)
                query = query.Where(t => t.Product.Amount >= context.PriceFrom);
            if (context.PriceTo != null)
                query = query.Where(t => t.Product.Amount <= context.PriceTo);

            if (!string.IsNullOrEmpty(context.Brand))
            {
                var values = context.Brand.Split(',');
                query = query.Where(t => values.Contains(t.Product.Brand));
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
    }
}
