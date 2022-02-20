using API.Data;
using API.DTOs;
using API.Extensions;
using API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    public class OrderController : BaseController
    {
        private readonly IUnitOfWork _uow;

        public OrderController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpPost]
        public async Task<ActionResult> Order(OrderRequestDto orderRequest)
        {
            var userId = HttpContext.User.GetUserId();
            var id = await _uow.OrdersRepository.OrderItems(userId, orderRequest);
            return Ok(id);
        }

        [HttpPost("checkout")]
        public async Task<ActionResult> CheckOut(List<CheckoutItem> items)
        {
            var userId = HttpContext.User.GetUserId();

            if (items == null || !items.Any())
                return BadRequest("No item available!");

            return Ok(await _uow.OrdersRepository.CheckOut(userId, items));
        }

        [HttpGet]
        public async Task<ActionResult> GetUserOrders([FromQuery] BaseParams @params)
        {
            var userId = HttpContext.User.GetUserId();
            var orders = await _uow.OrdersRepository.GetUserOrders(userId, @params);
            return Ok(orders);
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult> GetUserOrder(int orderId)
        {
            var userId = HttpContext.User.GetUserId();
            var order = await _uow.OrdersRepository.GetUserOrder(userId, orderId);
            if (order == null)
                return NotFound("Order not found.");
            return Ok(order);
        }

        [HttpPost("accept/{orderId}")]
        public async Task<ActionResult> AcceptOrder(int orderId)
        {
            var userId = HttpContext.User.GetUserId();
            return Ok(await _uow.OrdersRepository.AcceptOrder(userId, orderId));
        }
    }
}
