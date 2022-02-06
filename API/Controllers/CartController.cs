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

        [HttpPost("{storeItemId}/{productId}")]
        public async Task<ActionResult> AddToCart(int storeItemId, int productId)
        {
            if (storeItemId == 0 && productId == 0)
                return BadRequest("Invalid Inputs");

            var userId = HttpContext.User.GetUserId();
            var item = await _uow.OrdersRepository.AddToCart(userId, storeItemId, productId);
            if (!await _uow.SaveChanges())
            {
                return BadRequest("Failed to add.");
            }
            return Ok(await _uow.OrdersRepository.GetCart(userId, item.StoreItemId));
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
            var remove = await _uow.OrdersRepository.RemoveFromCart(HttpContext.User.GetUserId(), storeItemIds);
            if (remove && !await _uow.SaveChanges())
            {
                return BadRequest("Failed to remove.");
            }
            return Ok();
        }
    }
}
