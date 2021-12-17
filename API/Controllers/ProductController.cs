using API.Data;
using API.DTOs;
using API.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
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

        [HttpPost("checkout")]
        public async Task<ActionResult> CheckOut(List<CheckoutItem> items)
        {
            var userId = HttpContext.User.GetUserId();

            if (items == null || !items.Any()) 
                return BadRequest("No item available!");

            return Ok(await _uow.ProductRepository.CheckOut(userId, items));
        }
    }
}
