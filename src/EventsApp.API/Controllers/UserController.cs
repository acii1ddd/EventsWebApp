using AutoMapper;
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

    // public async Task<IActionResult> RegisterToEventAsync([FromBody] RegisterUserRequest request)
    // {
    //     _userService.RegisterToEventAsync(request.EventId, request.UserId);
    //     
    //     
    //         
    //     return Ok();
    // }
    //
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

public interface IUserService
{
    public Task<IActionResult> GetEventParticipantsAsync(Guid eventId);
}

public class RegisterUserRequest
{
    public Guid UserId { get; set; }
    
    public Guid EventId { get; set; }
}
