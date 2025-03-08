using AutoMapper;
using EventsApp.API.Contracts.Auth;
using EventsApp.BLL.Interfaces.Auth;
using EventsApp.Domain.Models;
using EventsApp.Domain.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsApp.API.Controllers;

[Route("api/auth")]
[ApiController]
public class AccountController : BaseController
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;
    
    // TODO: вынести в конфигурацию
    private const string RefreshTokenKey = "refreshToken";
    private const int ExpiresInMinutes = 30;
    
    public AccountController(IAuthService authService, IMapper mapper)
    {
        _authService = authService;
        _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpPost("sign-in")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SignInResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
    public async Task<IActionResult> SignIn([FromBody] SignInRequest request, CancellationToken cancellationToken)
    {
        // пользователь уже авторизован
        if (AuthorizedUserId != Guid.Empty)
        {
            return BadRequest(new ErrorResponse
            {
                ErrorMessage = $"Пользователь с email {request.Email} уже авторизован",
                Trace = string.Empty
            });
        }
        
        var signInModel = _mapper.Map<SignInModel>(request); 
        var authTokenModel = await _authService.SignIn(signInModel, cancellationToken);
        
        // устанавливаем refresh token в cookie
        SetRefreshTokenToCookie(authTokenModel.RefreshToken);
        
        var result = _mapper.Map<SignInResponse>(authTokenModel.AuthAccessTokenModel);
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("refresh-tokens")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SignInResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshTokens(CancellationToken cancellationToken)
    {
        var refreshToken = Request.Cookies[RefreshTokenKey];
        if (refreshToken is null)
        {
            return Unauthorized(); // need to relogin
        }

        var authTokenModel = await _authService.GetNewTokensPairAsync(refreshToken, cancellationToken);
        SetRefreshTokenToCookie(authTokenModel.RefreshToken);
        var result = _mapper.Map<SignInResponse>(authTokenModel.AuthAccessTokenModel);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        // refresh token пользователя истек
        var refreshToken = Request.Cookies[RefreshTokenKey];
        if (refreshToken is null)
        {
            return BadRequest("Refresh token пользователя истек");
        }
        
        await _authService.LogoutAsync(refreshToken, cancellationToken);
        Response.Cookies.Delete(RefreshTokenKey);
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
