using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<string?> RegisterAsync(string email, string password);
        Task<bool> ConfirmEmailAsync(string token);
        Task<User?> LoginAsync(string email, string password);
    }
}
