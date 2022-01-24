using System.Collections.Generic;
using API.Data;
using API.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IUnitOfWork _uow;

        public ProductController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] Dictionary<string, string> queryParams)
        {
            var result = await _uow.SearchRepository.Search(queryParams);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var userId = HttpContext.User.GetUserId();
            var product = await _uow.ProductRepository.GetProduct(id, userId);
            if (product == null)
                return BadRequest("Product not found!");
            return Ok(product);
        }

        [HttpGet("home")]
        public async Task<ActionResult> GetHomePage()
        {
            var result = await _uow.SearchRepository.GetHomePage();
            return Ok(result);
        }
    }
}
