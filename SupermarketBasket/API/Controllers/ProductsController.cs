using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using SupermarketBasket.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductDto dto)
    {
        var product = await _productService.CreateAsync(dto.Name, dto.Weight, dto.Quantity);
        return Ok(product);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var product = await _productService.GetByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ProductDto dto)
    {
        var product = await _productService.UpdateAsync(id, dto.Name, dto.Weight, dto.Quantity);

        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _productService.DeleteAsync(id);

        if (!success)
            return NotFound();

        return NoContent();
    }
}