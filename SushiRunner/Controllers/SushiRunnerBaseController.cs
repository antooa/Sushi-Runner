using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReturnTrue.AspNetCore.Identity.Anonymous;
using SushiRunner.Data.Entities;
using SushiRunner.Services.Interfaces;

namespace SushiRunner.Controllers
{
    public class SushiRunnerBaseController : Controller
    {
        protected readonly IAccountService accountService;

        public SushiRunnerBaseController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        protected string GetAnonymousId()
        {
            return HttpContext.Features.Get<IAnonymousIdFeature>()?.AnonymousId;
        }

        protected async Task<User> GetLoggedUserOrCreateAnonymous()
        {
            return await accountService.GetLoggedUserOrCreateAnonymous(HttpContext.User, GetAnonymousId());
        }
    }
}
