using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SushiRunner.Data.Entities;
using SushiRunner.Services.Interfaces;
using SushiRunner.ViewModels;

namespace SushiRunner.Controllers
{
    public class AccountController : SushiRunnerBaseController
    {
        public AccountController(IAccountService accountService) : base(accountService)
        {
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInModel model)
        {
            if (ModelState.IsValid)
            {
                var signInResult = await accountService.SignInAsync(model.Username, model.Password);
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

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            if (ModelState.IsValid)
            {
                var signUpResult = await accountService.SignUpCustomerAsync(
                    GetAnonymousId(),
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

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> SignOut()
        {
            accountService.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // TODO: add notification after confirm
        public async Task<IActionResult> ConfirmEmail([Required] string userId, [Required] string code)
        {
            var emailConfirmationResult = await accountService.ConfirmEmailAsync(userId, code);
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

        // TODO: remove and replace with /Home/PersonalInformation
        [Authorize]
        public async Task<IActionResult> Info()
        {
            var user = await GetLoggedUserOrCreateAnonymous();
            if (user.IsAnonymous)
            {
                return RedirectToAction("SignIn");
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword()
        {
            return RedirectToAction("Info");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdatePersonalInfo()
        {
            return RedirectToAction("Info");
        }
    }
}