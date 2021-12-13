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

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var userId = HttpContext.User.GetUserId();
            var product = await _uow.ProductRepository.GetProduct(id, userId);
            if (product == null)
                return BadRequest("Product not found!");
            return Ok(product);
        }
    }
}
