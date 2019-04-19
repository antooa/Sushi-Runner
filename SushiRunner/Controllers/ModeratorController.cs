using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SushiRunner.Data.Entities;

namespace SushiRunner.Controllers
{
    [Authorize(Roles = UserRoles.Moderator)]
    public class ModeratorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}