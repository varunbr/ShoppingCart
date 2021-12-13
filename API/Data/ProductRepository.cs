using API.DTOs;
using API.Extensions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public class ProductRepository : BaseRepository, IProductRepository
    {
        public ProductRepository(DataContext dataContext, IMapper mapper) : base(dataContext, mapper)
        {
        }

        public async Task<ProductDetailDto> GetProduct(int productId, int userId)
        {
            var userLocation = await GetUserLocation(userId);

            var product = await DataContext.Products
                .Where(p => p.Id == productId)
                .ProjectTo<ProductDetailDto>(Mapper.ConfigurationProvider)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (product == null) return null;

            product.DeliveryCharge = product.Amount >= 500 ? 0 : 60;

            if (userLocation != null)
            {
                var stateId = userLocation.Parent.ParentId;
                var storeItem = await DataContext.StoreItems
                    .Where(si => si.ProductId == productId && si.Available > 0)
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
                    .Where(si => si.ProductId == productId && si.Available > 0)
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
                .Where(pv => pv.ProductId == productId)
                .Include(pv => pv.Property)
                .AsNoTracking()
                .ToListAsync();

            product.Properties = pv.GetPropertyValue();

            return product;
        }
    }
}
