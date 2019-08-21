using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SushiRunner.Data.Entities;
using SushiRunner.Services;
using SushiRunner.Services.Interfaces;
using SushiRunner.ViewModels.Account;

namespace SushiRunner.Controllers
{
    public class AccountController : SushiRunnerBaseController
    {
        private readonly IOrderService _orderService;

        public AccountController(IAccountService accountService, IOrderService orderService)
            : base(accountService)
        {
            _orderService = orderService;
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
            await accountService.SignOutAsync();
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

        [Authorize]
        public async Task<IActionResult> PersonalInfo()
        {
            var user = await GetLoggedUserOrCreateAnonymous();
            if (user.IsAnonymous)
            {
                return RedirectToAction("SignIn");
            }

            if (user.Role == UserRoles.Moderator)
            {
                ViewBag.IsM = true;
            }

            var accountInfoModel = new AccountInfoModel
            {
                AccountInfoChange = new AccountInfoChangeModel
                {
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email
                },
                PasswordChange = new PasswordChangeModel()
            };

            return View(accountInfoModel);
        }

        // TODO: add validation !!!
        // TODO: send notification when changed info
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdatePersonalInfo(AccountInfoModel accountInfoModel)
        {
            var model = accountInfoModel.AccountInfoChange;
            var user = await GetLoggedUserOrCreateAnonymous();
            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;
            user.Email = model.Email;
            await accountService.UpdateInfo(user, model.FullName, model.PhoneNumber, model.Email);
            return RedirectToAction("PersonalInfo");
        }

        // TODO: add validation !!!
        // TODO: send notification when changed info
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(AccountInfoModel accountInfoModel)
        {
            if (ModelState.IsValid)
            {
                var model = accountInfoModel.PasswordChange;
                var user = await GetLoggedUserOrCreateAnonymous();
            
                if (user.Role == UserRoles.Moderator)
                {
                    ViewBag.IsM = true;
                }

                var changePasswordResult =
                    await accountService.ChangePassword(user, model.OldPassword, model.NewPassword,
                        model.NewPasswordRepeat);
            
                if (changePasswordResult.IsSuccessful)
                {
                    return RedirectToAction("PersonalInfo");
                }

                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Message);
                }
            }

            return View("PersonalInfo", accountInfoModel);
            //return Redirect("/Home/Error");
        }

        [Authorize]
        public async Task<IActionResult> MyOrders()
        {
        
            var user = await GetLoggedUserOrCreateAnonymous();
            if (user.IsAnonymous)
            {
                return RedirectToAction("SignIn");
            }

            if (user.Role == UserRoles.Moderator)
            {
                ViewBag.IsM = true;
            }
            var orders = _orderService.GetList();
            var model = new MyOrdersModel();
            model.Orders = orders.Select(order => new OrderItemModel
                {
                    Id = order.Id,
                    Status = order.OrderStatus,
                    PlacedAt = order.PlacedAt,
                    Address = order.Address
                })
                .ToList();
            return View(model);
        }
    }
}
