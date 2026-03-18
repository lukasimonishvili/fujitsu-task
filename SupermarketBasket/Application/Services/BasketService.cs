using Application.DTOs;
using Application.Interfaces;
using Domain.Interfaces;

namespace Application.Services;

public class BasketService : IBasketService
{
    private readonly IShoppingListRepository _repository;
    private readonly double _maxWeight = 20.0;

    public BasketService(IShoppingListRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<BasketItemDto>> BuildBasketAsync(int userId)
    {
        var items = await _repository.GetByUserIdAsync(userId);

        var sortedItems = items
            .OrderByDescending(x => x.Product.Weight)
            .ToList();

        var result = new List<BasketItemDto>();
        double currentWeight = 0;

        foreach (var item in sortedItems)
        {
            var weight = item.Product.Weight;

            if (weight <= 0 || weight > _maxWeight)
                continue;

            int maxFit = (int)((_maxWeight - currentWeight) / weight);

            int quantityToTake = Math.Min(item.Quantity, maxFit);

            if (quantityToTake > 0)
            {
                result.Add(new BasketItemDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    Weight = weight,
                    Quantity = quantityToTake
                });

                currentWeight += quantityToTake * weight;
            }

            if (currentWeight >= _maxWeight)
                break;
        }

        return result;
    }
}