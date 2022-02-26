using API.Data;
using API.DTOs;
using API.Extensions;
using API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize(Roles = "TrackAgent,TrackAdmin")]
    public class TrackController : BaseController
    {
        private readonly IUnitOfWork _uow;

        public TrackController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public async Task<ActionResult> GetOrders([FromQuery] TrackParams trackParams)
        {
            var userId = HttpContext.User.GetUserId();
            var result = await _uow.TrackRepository.GetOrders(userId, trackParams);
            return Ok(result);
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult> GetOrder(int orderId)
        {
            return Ok(await _uow.TrackRepository.GetOrder(orderId));
        }

        [HttpPost("receive")]
        public async Task<ActionResult> ReceiveOrder(TrackRequestDto requestDto)
        {
            var userId = HttpContext.User.GetUserId();
            return Ok(await _uow.TrackRepository.ReceiveOrder(userId, requestDto.OrderId, requestDto.LocationId));
        }

        [HttpPost("dispatch")]
        public async Task<ActionResult> DispatchOrder(TrackRequestDto requestDto)
        {
            var userId = HttpContext.User.GetUserId();
            return Ok(await _uow.TrackRepository.DispatchOrder(userId, requestDto.OrderId, requestDto.LocationId));
        }

        [HttpPost("deliver")]
        public async Task<ActionResult> DeliverOrder(TrackRequestDto requestDto)
        {
            var userId = HttpContext.User.GetUserId();
            return Ok(await _uow.TrackRepository.DispatchOrderForDelivery(userId, requestDto.OrderId, requestDto.LocationId));
        }
    }
}
