using API.Data;
using API.Extensions;
using API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    public class TrackController : BaseController
    {
        private readonly IUnitOfWork _uow;

        public TrackController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet("order")]
        public async Task<ActionResult> GetOrders([FromQuery] TrackParams trackParams)
        {
            var userId = HttpContext.User.GetUserId();
            var result = await _uow.TrackRepository.GetOrders(userId, trackParams);
            return Ok(result);
        }

        [HttpGet("order/{orderId}")]
        public async Task<ActionResult> GetOrder(int orderId)
        {
            return Ok(await _uow.TrackRepository.GetOrder(orderId));
        }

        [HttpPost("receive/{orderId}")]
        public async Task<ActionResult> ReceiveOrder(int orderId)
        {
            var userId = HttpContext.User.GetUserId();
            return Ok(await _uow.TrackRepository.ReceiveOrder(userId, orderId));
        }

        [HttpPost("dispatch/{orderId}")]
        public async Task<ActionResult> DispatchOrder(int orderId)
        {
            var userId = HttpContext.User.GetUserId();
            return Ok(await _uow.TrackRepository.DispatchOrder(userId, orderId));
        }

        [HttpPost("deliver/{orderId}")]
        public async Task<ActionResult> DeliverOrder(int orderId)
        {
            var userId = HttpContext.User.GetUserId();
            return Ok(await _uow.TrackRepository.DispatchOrderForDelivery(userId, orderId));
        }
    }
}
