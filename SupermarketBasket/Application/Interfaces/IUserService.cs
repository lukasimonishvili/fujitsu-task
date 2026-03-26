using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<string?> RegisterAsync(RegisterDto dto);
        Task<bool> ConfirmEmailAsync(string token);
        Task<User?> LoginAsync(LoginDto dto);
    }
}
