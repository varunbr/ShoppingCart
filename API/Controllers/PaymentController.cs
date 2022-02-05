using API.Data;
using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    }
}
