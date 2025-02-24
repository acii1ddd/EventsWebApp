using EventsApp.Domain.Abstractions.Auth;
using Microsoft.AspNetCore.Mvc;

namespace EventsApp.API.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController
{
    private readonly IAuthService _authService;

    public AccountController(IAuthService authService)
    {
        _authService = authService;
    }

    //[HttpPost("login")]
    public Task<IActionResult> SignIn(SignInRequest request)
    {
        throw new NotImplementedException();
    }
    
    // refresh-tokens
}

public class SignInRequest
{
    public string Email { get; set; } = string.Empty;
    
    public string Password { get; set; } = string.Empty;
}