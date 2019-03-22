using Microsoft.AspNetCore.Mvc;

namespace Sushi_Runner.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}