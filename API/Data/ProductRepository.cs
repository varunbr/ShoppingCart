using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Services;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public class ProductRepository : BaseRepository, IProductRepository
    {
        public ProductRepository(DataContext dataContext, IMapper mapper, IPhotoService photoService) : base(dataContext, mapper, photoService)
        {
        }

        public async Task<ProductModelDto> GetProduct(int productId, int userId)
        {
            var userLocation = await GetUserLocation(userId);

            var model = await DataContext.Products
                .Where(p => p.Id == productId)
                .Select(p => p.Model)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(model)) return null;

            var products = await GetProducts(model, userLocation);

            return products.GetProductModel();
        }

        private async Task<List<ProductDetailDto>> GetProducts(string model, Location userLocation)
        {
            var products = await DataContext.Products
                .Where(p => p.Model == model)
                .ProjectTo<ProductDetailDto>(Mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync();

            foreach (var product in products)
            {
                product.DeliveryCharge = product.Amount >= 500 ? 0 : 60;

                if (userLocation != null && product.Available)
                {
                    var stateId = userLocation.Parent.ParentId;
                    var storeItem = await DataContext.StoreItems
                        .Where(si => si.ProductId == product.Id && si.Available > 0)
                        .Where(si => si.Store.Address.Location.Parent.ParentId == stateId)
                        .Select(si => new { si.Store.Name, si.Id })
                        .AsNoTracking()
                        .FirstOrDefaultAsync();

                    if (storeItem != null)
                    {
                        product.DeliveryCharge = product.Amount >= 500 ? 0 : 40;
                        product.StoreItemId = storeItem.Id;
                        product.StoreName = storeItem.Name;
                    }
                }

                if (product.Available && string.IsNullOrEmpty(product.StoreName))
                {
                    var storeItem = await DataContext.StoreItems
                        .Where(si => si.ProductId == product.Id && si.Available > 0)
                        .Select(si => new { si.Store.Name, si.Id })
                        .AsNoTracking()
                        .FirstOrDefaultAsync();

                    if (storeItem != null)
                    {
                        product.StoreItemId = storeItem.Id;
                        product.StoreName = storeItem.Name;
                    }
                }

                var pv = await DataContext.PropertyValues
                    .Where(pv => pv.ProductId == product.Id)
                    .Include(pv => pv.Property)
                    .AsNoTracking()
                    .ToListAsync();

                product.Properties = pv.GetPropertyValue();
            }
            return products;
        }
    }
}
