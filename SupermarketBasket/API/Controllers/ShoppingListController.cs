using Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ShoppingListController : ControllerBase
{
    private readonly IShoppingListService _service;

    public ShoppingListController(IShoppingListService service)
    {
        _service = service;
    }

    private int GetUserId()
    {
        return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var userId = GetUserId();
        var list = await _service.GetUserListAsync(userId);
        return Ok(list);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddShoppingListItemDto dto)
    {
        var userId = GetUserId();

        await _service.AddAsync(userId, dto.ProductId, dto.Quantity);

        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateShoppingListItemDto dto)
    {
        var userId = GetUserId();

        var success = await _service.UpdateQuantityAsync(userId, dto.ProductId, dto.Quantity);

        if (!success)
            return NotFound();

        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Remove(int productId)
    {
        var userId = GetUserId();

        var success = await _service.RemoveAsync(userId, productId);

        if (!success)
            return NotFound();

        return Ok();
    }
}