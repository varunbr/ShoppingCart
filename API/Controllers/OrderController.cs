using API.Data;
using API.DTOs;
using API.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IUnitOfWork _uow;

        public OrderController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpPost]
        public async Task<ActionResult> Order(List<CheckoutItem> items)
        {
            var userId = HttpContext.User.GetUserId();
            await _uow.OrdersRepository.OrderItems(userId, items);
            return Ok();
        }

        [HttpPost("checkout")]
        public async Task<ActionResult> CheckOut(List<CheckoutItem> items)
        {
            var userId = HttpContext.User.GetUserId();

            if (items == null || !items.Any())
                return BadRequest("No item available!");

            return Ok(await _uow.OrdersRepository.CheckOut(userId, items));
        }
    }
}
