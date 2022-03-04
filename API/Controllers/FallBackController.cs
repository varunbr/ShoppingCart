using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace API.Controllers
{
    public class FallBackController : Controller
    {
        public ActionResult Index()
        {
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html"), "text/HTML");
        }
    }
}
