using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories;

public class ShoppingListRepository : IShoppingListRepository
{
    private readonly AppDbContext _context;

    public ShoppingListRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ShoppingListItem>> GetByUserIdAsync(int userId)
    {
        return await _context.ShoppingListItems
            .Include(x => x.Product)
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }

    public async Task<ShoppingListItem?> GetItemAsync(int userId, int productId)
    {
        return await _context.ShoppingListItems
            .FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == productId);
    }

    public async Task AddAsync(ShoppingListItem item)
    {
        _context.ShoppingListItems.Add(item);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ShoppingListItem item)
    {
        _context.ShoppingListItems.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ShoppingListItem item)
    {
        _context.ShoppingListItems.Remove(item);
        await _context.SaveChangesAsync();
    }
}