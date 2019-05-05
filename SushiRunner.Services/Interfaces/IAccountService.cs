using System.Threading.Tasks;
using SushiRunner.Services.Dto;

namespace SushiRunner.Services.Interfaces
{
    public interface IAccountService
    {
        Task<SignInResult> SignInAsync(string username, string password);

        void SignOutAsync(string username);

        void SignUpCustomerAsync(string username, string password);
    }
}