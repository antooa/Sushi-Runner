using System;
using System.Security.Claims;
using System.Threading.Tasks;
using SushiRunner.Data.Entities;
using SushiRunner.Services.Dto;

namespace SushiRunner.Services.Interfaces
{
    public interface IAccountService
    {
        Task<SignInResult> SignInAsync(string username, string password);

        Task<SignUpResult> SignUpCustomerAsync(string anonymousId, string username, string email, string password,
            Func<User, string, string> generateEmailConfirmationLink);

        void SignOutAsync();

        Task<EmailConfirmationResult> ConfirmEmailAsync(string userId, string code);

        Task<User> GetLoggedUserOrCreateAnonymous(ClaimsPrincipal principal, string newId);

        Task<User> GetLoggedUser(ClaimsPrincipal principal);
    }
}
