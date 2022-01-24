using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;

namespace API.Data
{
    public interface ISearchRepository
    {
        Task<Response<ProductDto, SearchContextDto>> Search(Dictionary<string, string> queryParams);
        Task<HomePageDto> GetHomePage();
    }
}
