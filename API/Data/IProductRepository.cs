using System.Threading.Tasks;
using API.DTOs;

namespace API.Data;

public interface IProductRepository
{
    Task<ProductDetailDto> GetProduct(int productId,int userId);
}