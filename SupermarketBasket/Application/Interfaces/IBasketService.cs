using Application.DTOs;

namespace Application.Interfaces;
public interface IBasketService
{
    Task<List<BasketItemDto>> BuildBasketAsync(int userId);
}