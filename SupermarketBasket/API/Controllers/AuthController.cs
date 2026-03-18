using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SupermarketBasket.Application.Services;
using Application.DTOs;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _service;
    private readonly JwtTokenGenerator _tokenGenerator;

    public AuthController(IUserService service, JwtTokenGenerator tokenGenerator)
    {
        _service = service;
        _tokenGenerator = tokenGenerator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var token = await _service.RegisterAsync(dto.Email, dto.Password);

        if (token == null)
            return BadRequest("User already exists");

        return Ok(new
        {
            message = "User registered. Confirm email using token.",
            token = token
        });
    }

    [HttpGet("confirm")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string token)
    {
        var success = await _service.ConfirmEmailAsync(token);

        if (!success)
            return BadRequest("Invalid or expired token");

        return Ok("Email confirmed successfully");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _service.LoginAsync(dto.Email, dto.Password);

        if (user == null)
            return Unauthorized("Invalid credentials or email not confirmed");

        var token = _tokenGenerator.GenerateToken(user);

        return Ok(new
        {
            token = token
        });
    }
}