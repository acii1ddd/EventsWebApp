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
public class AccountController : BaseController
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;
    
    private const string RefreshTokenKey = "refreshToken";
    // TODO: вынести в конфигурацию 
    private const int ExpiresInMinutes = 30;
    
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
        // пользователь уже авторизован
        if (AuthorizedUserId != Guid.Empty)
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

    [AllowAnonymous]
    [HttpPost("refresh-tokens")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SignInResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<Error>))]
    public async Task<IActionResult> RefreshTokens()
    {
        var refreshToken = Request.Cookies[RefreshTokenKey];
        if (refreshToken is null)
        {
            return Unauthorized(); // need to relogin
        }

        var authTokenModelResult = await _authService.GetNewTokensPairAsync(refreshToken);
        if (authTokenModelResult.IsFailed)
        {
            return BadRequest(authTokenModelResult.Errors);
        }
        
        SetRefreshTokenToCookie(authTokenModelResult.Value.RefreshToken);
        var result = _mapper.Map<SignInResponse>(authTokenModelResult.Value.AuthAccessTokenModel);
        return Ok(result);
    }
    
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> Logout()
    {
        if (AuthorizedUserId == Guid.Empty)
        {            
            return BadRequest("Пользователь не авторизован");
        }
        
        var refreshToken = Request.Cookies[RefreshTokenKey];
        if (refreshToken is null)
        {
            return BadRequest("Refresh token не найден");
        }

        _ = await _authService.LogoutAsync(refreshToken);
        return Ok();
    }
    
    // TODO: регистрация
    
    private void SetRefreshTokenToCookie(string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddMinutes(ExpiresInMinutes)
        };
        
        Response.Cookies.Append(RefreshTokenKey, refreshToken, cookieOptions);
    }
}
