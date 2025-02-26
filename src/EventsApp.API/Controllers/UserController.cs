using AutoMapper;
using EventsApp.Domain.Abstractions.Users;
using EventsApp.Domain.Errors;
using Microsoft.AspNetCore.Mvc;

namespace EventsApp.API.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : BaseController
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UserController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    
    // /events/{eventId}/register
   
    // [HttpGet]
    // public async Task<IActionResult> RegisterToEventAsync([FromBody] RegisterUserRequest request)
    // {
    //     _userService.RegisterToEventAsync(request.EventId, request.UserId);
    //     
    //     
    //         
    //     return Ok();
    // }
}
