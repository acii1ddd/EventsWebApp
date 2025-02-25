using AutoMapper;
using EventsApp.API.Contracts.Auth;
using EventsApp.Domain.Abstractions.Auth;
using EventsApp.Domain.Errors;
using EventsApp.Domain.Models.Auth;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsApp.API.Controllers;

[Route("api/auth")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;

    public AccountController(IAuthService authService, IMapper mapper)
    {
        _authService = authService;
        _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpPost("sign-in")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SignInResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
    {
        // проверить авторизован ли уже пользователь
        if (false)
        {
            return BadRequest(new Error($"Пользователь с email {request.Email} уже авторизован"));
        }
        
        var signInModel = _mapper.Map<SignInModel>(request); 
        var authTokenModelResult = await _authService.SignIn(signInModel);
        
        if (authTokenModelResult.IsFailed)
        {
            var error = authTokenModelResult.Errors.FirstOrDefault();
            
            if (error is UserWithEmailNotFoundError)
            {
                return NotFound(error);
            }
            return BadRequest(error);
        }
        
        // устанавливаем refresh token в cookie
        SetRefreshTokenToCookie(authTokenModelResult.Value.RefreshToken);

        var result = _mapper.Map<SignInResponse>(authTokenModelResult.Value.AuthAccessTokenModel);
        return Ok(result);
    }

    private void SetRefreshTokenToCookie(string refreshToken)
    {
        const int expiresInMinutes = 30;
        
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddMinutes(expiresInMinutes)
        };
        
        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }
    
    // refresh-tokens
}
