using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;

namespace API.Data
{
    public interface ISearchRepository
    {
        Task<SearchResult> Search(Dictionary<string, string> queryParams);
    }
}
