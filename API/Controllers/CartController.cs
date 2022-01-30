using API.Data;
using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    public class CartController : BaseController
    {
        private readonly IUnitOfWork _uow;
        public CartController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpPost]
        public async Task<ActionResult> AddToCart([FromBody] int storeItemId)
        {
            var userId = HttpContext.User.GetUserId();
            await _uow.OrdersRepository.AddToCart(userId, storeItemId);
            if (!await _uow.SaveChanges())
            {
                return BadRequest("Failed to add.");
            }
            return Ok(await _uow.OrdersRepository.GetCart(userId, storeItemId));
        }

        [HttpGet]
        public async Task<ActionResult> GetCart()
        {
            var result = await _uow.OrdersRepository.GetCart(HttpContext.User.GetUserId());
            return Ok(result);
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveFromCart([FromBody] int[] storeItemIds)
        {
            await _uow.OrdersRepository.RemoveFromCart(HttpContext.User.GetUserId(), storeItemIds);
            if (!await _uow.SaveChanges())
            {
                return BadRequest("Failed to remove.");
            }
            return Ok();
        }
    }
}
