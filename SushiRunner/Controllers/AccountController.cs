using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReturnTrue.AspNetCore.Identity.Anonymous;
using SushiRunner.Data.Entities;
using SushiRunner.Services.Interfaces;
using SushiRunner.ViewModels;

namespace SushiRunner.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInModel model)
        {
            if (ModelState.IsValid)
            {
                var signInResult = await _accountService.SignInAsync(model.Username, model.Password);
                if (signInResult.IsSuccessful)
                {
                    if (signInResult.Roles.Contains(UserRoles.Moderator))
                    {
                        return RedirectToAction("Index", "Moderator");
                    }

                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in signInResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Message);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            if (ModelState.IsValid)
            {
                var signUpResult = await _accountService.SignUpAsync(
                    HttpContext.Features.Get<IAnonymousIdFeature>()?.AnonymousId,
                    model.Email,
                    model.Email,
                    model.Password,
                    (user, code) => Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new {userId = user.Id, code},
                        HttpContext.Request.Scheme
                    )
                );

                if (signUpResult.IsSuccessful)
                {
                    return Content(
                        "To complete the registration, check the email and click on the link indicated in the mail.");
                }

                foreach (var error in signUpResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Message);
                }
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([Required] string userId, [Required] string code)
        {
            var emailConfirmationResult = await _accountService.ConfirmEmailAsync(userId, code);
            if (emailConfirmationResult.IsSuccessful)
            {
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in emailConfirmationResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Message);
            }

            return Redirect("/Home/Error");
        }
    }
}
