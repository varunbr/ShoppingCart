using API.Data;
using API.Extensions;
using API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize(Roles = "StoreAdmin,StoreAgent")]
    public class StoreController : BaseController
    {
        private readonly IUnitOfWork _uow;
        public StoreController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet("order")]
        public async Task<ActionResult> GetOrders([FromQuery] OrderParams orderParams)
        {
            var userId = HttpContext.User.GetUserId();
            var orders = await _uow.StoreRepository.GetOrders(orderParams, userId);
            return Ok(orders);
        }

        [HttpGet("order/{orderId}")]
        public async Task<ActionResult> GetOrder(int orderId)
        {
            var userId = HttpContext.User.GetUserId();
            var order = await _uow.StoreRepository.GetOrder(userId, orderId);
            return Ok(order);
        }

        [HttpPost("dispatch/{orderId}")]
        public async Task<ActionResult> DispatchOrder(int orderId)
        {
            var userId = HttpContext.User.GetUserId();
            var order = await _uow.StoreRepository.StartDispatchingOrder(userId, orderId);
            return Ok(order);
        }
    }
}
