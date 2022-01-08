using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
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
        public SearchRepository(DataContext dataContext, IMapper mapper) : base(dataContext, mapper)
        {
        }

        public async Task<SearchResult> Search(Dictionary<string, string> queryParams)
        {
            var context = new SearchContext(queryParams);
            await GetCategory(context);
            return await GetProducts(context);
        }

        private async Task<SearchResult> GetProducts(SearchContext context)
        {
            var query = DataContext.ProductTags.AsQueryable();
            query = ApplyFilters(query, context);

            var group = query.Where(t => context.Keywords.Contains(t.Name) || t.Name == context.SearchText)
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
                _ => group.OrderByDescending(g => g.Score).ThenByDescending(g=>g.SoldQuantity)
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

            return new SearchResult(
                new PagedList<ProductDto>(result, 0, context.PageSize, context.PageNumber), Mapper.Map<SearchContextDto>(context));
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
                query = query.Where(t => t.Product.Amount <= context.PriceFrom);

            if (!string.IsNullOrEmpty(context.Brand))
                query = query.Where(t => t.Product.Brand == context.Brand);
            return query;
        }

        private async Task GetCategory(SearchContext context)
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

            if (category != null)
            {
                context.Category = category.Key;
                var properties = await DataContext.Properties.Where(p => p.CategoryId == category.CategoryId).ToListAsync();
                context.Properties = properties;
                AddFilters(context);
            }
        }

        private static void AddFilters(SearchContext context)
        {
            foreach (var param in context.QueryParams)
            {
                if (param.Key.Equals(Constants.Brand, StringComparison.InvariantCultureIgnoreCase))
                {
                    context.Brand = param.Value;
                    continue;
                }

                var property = context.Properties.FirstOrDefault(p => p.Name == param.Key);
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
