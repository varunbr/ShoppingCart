using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;

namespace API.Data;

public interface IProductRepository
{
    Task<ProductModelDto> GetProduct(int productId,int userId);
}