using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SushiRunner.Controllers
{
    [Authorize]
    public class ModeratorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}