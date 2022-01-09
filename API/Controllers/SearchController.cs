using API.Data;
using API.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class SearchController : BaseController
    {
        private readonly IUnitOfWork _uow;

        public SearchController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] Dictionary<string, string> queryParams)
        {
            var result = await _uow.SearchRepository.Search(queryParams);
            return Ok(result);
        }
    }
}
