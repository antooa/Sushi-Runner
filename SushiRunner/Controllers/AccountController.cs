using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SushiRunner.Data.Entities;
using SushiRunner.Services.Interfaces;
using SushiRunner.ViewModels;

namespace SushiRunner.Controllers
{
    public class AccountController : Controller
    {
        private IEmailService _emailService;
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;

        private IAccountService _accountService;

        public AccountController(IEmailService emailService, UserManager<User> userManager,
            SignInManager<User> signInManager, IAccountService accountService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
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

                if (signInResult.UserExists)
                {
                    ModelState.AddModelError("Password", "Incorrect password, try again");
                }
                else
                {
                    ModelState.AddModelError("Username", "Couldn't find user with such email");
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
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return Redirect("/Home/Error");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Redirect("/Home/Error");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return Redirect("/Home/Error");
            }
        }
    }
}