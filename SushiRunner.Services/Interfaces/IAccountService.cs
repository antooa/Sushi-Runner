using System;
using System.Threading.Tasks;
using SushiRunner.Data.Entities;
using SushiRunner.Services.Dto;

namespace SushiRunner.Services.Interfaces
{
    public interface IAccountService
    {
        Task<SignInResult> SignInAsync(string username, string password);

        Task<SignUpResult> SignUpAsync(string username, string email, string password,
            Func<User, string, string> generateEmailConfirmationLink);

        Task<EmailConfirmationResult> ConfirmEmailAsync(string userId, string code);
    }
}
