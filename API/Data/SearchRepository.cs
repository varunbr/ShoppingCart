using API.DTOs;
using API.Extensions;
using API.Helpers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            await GetBrand(context);
            return await GetProducts(context);
        }

        private async Task<SearchResult> GetProducts(SearchContext context)
        {
            var query = DataContext.ProductTags.AsQueryable();

            if (!string.IsNullOrEmpty(context.Category))
                query = query.Where(t => t.Product.Category.Name == context.Category);

            if (!string.IsNullOrEmpty(context.Brand))
                query = query.Where(t => t.Product.Brand == context.Brand);

            var group = query.Where(t => context.Keywords.Contains(t.Name) || t.Name == context.SearchText)
                .GroupBy(t => t.ProductId)
                .Select(g => new
                {
                    g.Key,
                    Score = g.Sum(p => p.Score),
                    Price = g.First().Product.Amount,
                    g.First().Product.Created
                });

            var order = context.OrderBy switch
            {
                "HighToLow" => group.OrderByDescending(g => g.Price),
                "LowToHigh" => group.OrderBy(g => g.Price),
                "Latest" => group.OrderByDescending(g => g.Created),
                _ => group.OrderByDescending(g => g.Score)
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
                new PagedList<ProductDto>(result, 0, context.PageSize, context.PageNumber), new SearchContextDto());
        }

        private async Task GetBrand(SearchContext context)
        {
            var brandQuery = DataContext.Products
                .Where(p => context.Keywords.Contains(p.Brand));

            if (!string.IsNullOrEmpty(context.Category))
                brandQuery = brandQuery.Where(p => p.Category.Name == context.Category);

            var brand = await brandQuery.GroupBy(p => p.Brand)
            .Select(g => new
            {
                Name = g.Key,
                Sum = g.Sum(p => p.SoldQuantity)
            })
            .OrderByDescending(r => r.Sum)
            .AsNoTracking()
            .FirstOrDefaultAsync();
            context.Brand = brand?.Name;
        }

        private async Task GetCategory(SearchContext context)
        {
            var category = await DataContext.CategoryTags
                .Where(t => context.Keywords.Contains(t.Name))
                .GroupBy(t => t.Category.Name)
                .Select(g => new
                {
                    Name = g.Key,
                    Sum = g.Sum(t => t.Score)
                })
                .OrderByDescending(r => r.Sum)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            context.Category = category?.Name;
        }
    }
}
