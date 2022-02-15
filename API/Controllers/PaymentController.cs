using API.Data;
using API.DTOs;
using API.Extensions;
using API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    public class PaymentController : BaseController
    {
        private readonly IUnitOfWork _uow;
        public PaymentController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet("{pay-options}")]
        public async Task<ActionResult> GetPayOptions()
        {
            var userId = HttpContext.User.GetUserId();
            return Ok(await _uow.PayRepository.GetPaymentOptions(userId));
        }

        [HttpGet("transactions")]
        public async Task<ActionResult> GetTransactions([FromQuery] BaseParams @params)
        {
            var userId = HttpContext.User.GetUserId();
            return Ok(await _uow.PayRepository.GetTransactions(userId, @params));
        }

        [HttpPost("transfer")]
        public async Task<ActionResult> TransferAmount(TransferDto transfer)
        {
            var userId = HttpContext.User.GetUserId();
            if (HttpContext.User.GetUserName().Equals(transfer.UserName, StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("You cannot send to yourself.");
            }
            if (string.IsNullOrWhiteSpace(transfer.Description))
            {
                transfer.Description = $"{HttpContext.User.GetUserName()} to {transfer.UserName}";
            }
            await _uow.PayRepository.TransferAmount(userId, transfer);
            return await GetTransactions(new BaseParams());
        }
    }
}
