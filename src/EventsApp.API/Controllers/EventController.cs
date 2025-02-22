using AutoMapper;
using EventsApp.API.Contracts.Events;
using EventsApp.Domain.Abstractions.Events;
using Microsoft.AspNetCore.Mvc;

namespace EventsApp.API.Controllers;

[Route("api/events")]
[ApiController]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;
    private readonly ILogger<EventController> _logger;
    private readonly IMapper _mapper;
    
    public EventController(IEventService eventService, ILogger<EventController> logger, IMapper mapper)
    {
        _eventService = eventService;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetEventResponse>))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAllAsync()
    {
        var events = await _eventService.GetAllAsync();

        if (events.Count == 0)
        {
            return NoContent();
        }
        
        var result = _mapper.Map<List<GetEventResponse>>(events);
        return Ok(result);
    }
}