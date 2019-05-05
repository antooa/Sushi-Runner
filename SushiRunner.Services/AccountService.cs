using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SushiRunner.Data.Entities;
using SushiRunner.Services.Dto;
using SushiRunner.Services.Interfaces;
using SignInResult = SushiRunner.Services.Dto.SignInResult;

namespace SushiRunner.Services
{
    public class AccountService : IAccountService
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IEmailService _emailService;

        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public async Task<SignInResult> SignInAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                var signIn = await _signInManager.PasswordSignInAsync(user, password, false, false);
                if (signIn.Succeeded)
                {
                    return new SignInResult
                    {
                        IsSuccessful = true,
                        UserExists = true,
                        Roles = await _userManager.GetRolesAsync(user)
                    };
                }

                return new SignInResult
                {
                    IsSuccessful = false,
                    UserExists = true
                };
            }

            return new SignInResult {IsSuccessful = false, UserExists = false};
        }

        public void SignOutAsync(string username)
        {
            throw new NotImplementedException();
        }

        public async Task<SignUpResult> SignUpAsync(string username, string email, string password,
            Func<User, string, string> generateEmailConfirmationLink)
        {
            var user = new User
            {
                Email = email,
                UserName = username
            };

            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = generateEmailConfirmationLink(user, code);

                await _emailService.SendEmailAsync(email, "Confirm your account",
                    $"Confirm the registration by clicking on the link: <a href='{confirmationLink}'>confirmation link</a>");

                return new SignUpResult
                {
                    IsSuccessful = true,
                    User = user
                };
            }

            return new SignUpResult
            {
                IsSuccessful = false,
                Errors = result.Errors
            };
        }
    }
}