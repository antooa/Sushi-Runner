using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SushiRunner.Controllers
{
    [Authorize(Roles = "MODERATOR")]
    public class ModeratorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}