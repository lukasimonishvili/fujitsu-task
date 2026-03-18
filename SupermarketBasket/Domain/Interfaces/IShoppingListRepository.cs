using Domain.Entities;

namespace Domain.Interfaces;
public interface IShoppingListRepository
{
    Task<List<ShoppingListItem>> GetByUserIdAsync(int userId);
    Task<ShoppingListItem?> GetItemAsync(int userId, int productId);
    Task AddAsync(ShoppingListItem item);
    Task UpdateAsync(ShoppingListItem item);
    Task DeleteAsync(ShoppingListItem item);
}