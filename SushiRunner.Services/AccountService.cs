using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailService _emailService;

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
            if (user == null || user.IsAnonymous)
            {
                return new SignInResult
                {
                    IsSuccessful = false,
                    Errors = new List<AccountError>
                    {
                        new AccountError {Message = "Couldn't find such user"}
                    }
                };
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return new SignInResult
                {
                    IsSuccessful = false,
                    Errors = new List<AccountError>
                    {
                        new AccountError {Message = "User email not confirmed"}
                    }
                };
            }

            var signIn = await _signInManager.PasswordSignInAsync(user, password, true, false);
            if (signIn.Succeeded)
            {
                return new SignInResult
                {
                    IsSuccessful = true,
                    Roles = await _userManager.GetRolesAsync(user)
                };
            }

            var signInResult = new SignInResult {IsSuccessful = false};
            string errorMessage;
            if (signIn.IsLockedOut)
            {
                errorMessage = "User is locket out";
            }
            else if (signIn.IsNotAllowed)
            {
                errorMessage = "User not allowed to sign-in";
            }
            else if (signIn.RequiresTwoFactor)
            {
                errorMessage = "User requires two-factor authentication";
            }
            else
            {
                errorMessage = "Wrong password for this user";
            }

            signInResult.Errors = new List<AccountError> {new AccountError {Message = errorMessage}};
            return signInResult;
        }

        public async Task<SignUpResult> SignUpAsync(string anonymousId, string username, string email, string password,
            Func<User, string, string> generateEmailConfirmationLink)
        {
            var user = await _userManager.FindByIdAsync(anonymousId);
            if (user != null && user.IsAnonymous)
            {
                user.UserName = username;
                user.Email = email;
                user.IsAnonymous = false;
                user.SecurityStamp = Guid.NewGuid().ToString();
                await _userManager.UpdateAsync(user);
                await _userManager.ChangePasswordAsync(user, user.Id, password);
                SendConfirmationEmail(user, generateEmailConfirmationLink);

                return new SignUpResult
                {
                    IsSuccessful = true,
                    User = user
                };
            }

            user = new User {Email = email, UserName = username};
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                SendConfirmationEmail(user, generateEmailConfirmationLink);
                return new SignUpResult
                {
                    IsSuccessful = true,
                    User = user
                };
            }

            return new SignUpResult
            {
                IsSuccessful = false,
                Errors = result.Errors.Select(e => new AccountError {Message = e.Description}).ToList()
            };
        }

        private async void SendConfirmationEmail(User user,
            Func<User, string, string> generateEmailConfirmationLink)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = generateEmailConfirmationLink(user, code);

            await _emailService.SendEmailAsync(user.Email, "Confirm your account",
                $"Confirm the registration by clicking on the link: <a href='{confirmationLink}'>confirmation link</a>");
        }

        public async Task<EmailConfirmationResult> ConfirmEmailAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new EmailConfirmationResult
                {
                    IsSuccessful = false,
                    Errors = new List<AccountError> {new AccountError {Message = ""}}
                };
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            return new EmailConfirmationResult
            {
                IsSuccessful = result.Succeeded,
                Errors = result.Errors.Select(e => new AccountError {Message = e.Description}).ToList()
            };
        }

        public async Task<User> GetLoggedUserOrCreateAnonymous(ClaimsPrincipal principal, string newId)
        {
            var user = await GetLoggedUser(principal);
            if (user == null)
            {
                user = new User
                {
                    Id = newId,
                    UserName = newId,
                    IsAnonymous = true
                };

                var password = newId + 'A'; // password validation requires upper-case letter
                await _userManager.CreateAsync(user, password);
                await _signInManager.PasswordSignInAsync(newId, password, true, false);
            }

            return user;
        }

        public async Task<User> GetLoggedUser(ClaimsPrincipal principal)
        {
            return await _userManager.GetUserAsync(principal);
        }
    }
}