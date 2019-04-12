using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SushiRunner.Controllers
{
    public class ModeratorController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}