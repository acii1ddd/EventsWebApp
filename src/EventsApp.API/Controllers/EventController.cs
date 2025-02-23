using AutoMapper;
using EventsApp.API.Contracts.Events;
using EventsApp.Domain.Abstractions.Events;
using EventsApp.Domain.Models.Events;
using Microsoft.AspNetCore.Mvc;

namespace EventsApp.API.Controllers;

[Route("api/events")]
[ApiController]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;
    private readonly IMapper _mapper;
    
    private readonly Guid _authorizedUserId = Guid.Parse("9d86f170-9372-4f8e-adc7-18b42bc7d09b");
    
    public EventController(IEventService eventService, ILogger<EventController> logger, IMapper mapper)
    {
        _eventService = eventService;
        _mapper = mapper;
    }

    // /events
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetEventResponse>))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAllAsync()
    {
        var events = await _eventService.GetAllAsync();
        var result = _mapper.Map<List<GetEventResponse>>(events);
        return Ok(result);
    }
    
    // /events/guid
    [HttpGet("{eventId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetEventResponse>))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid eventId)
    {
        var eventModel = await _eventService.GetByIdAsync(eventId);

        if (eventModel is null)
        {
            return NoContent();
        }
        
        var result = _mapper.Map<GetEventResponse>(eventModel);
        return Ok(result);
    }

    // /events/name
    [HttpGet("{name}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetEventResponse))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetByNameAsync([FromRoute] string name)
    {
        var eventModel = await _eventService.GetByNameAsync(name);

        if (eventModel is null)
        {
            return NoContent();
        }
        
        var result = _mapper.Map<GetEventResponse>(eventModel);
        return Ok(result);
    }

    // данные должны быть получены из тела запроса в формате multipart/form-data
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetEventResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddAsync([FromForm] AddEventWithImageRequest request)
    {
        // валидация FluentValidation
        var eventModel = _mapper.Map<EventModel>(request);
        
        await using var stream = request.ImageFile.OpenReadStream();
        
        var addedEventModel = await _eventService.AddAsync(
            eventModel,
            stream ?? throw new NullReferenceException("Stream is null."),
            request.ImageFile.FileName ?? throw new NullReferenceException("ImageFile is null."),
            request.ImageFile.ContentType ?? throw new NullReferenceException("ImageFile is null.")
        );

        if (addedEventModel.IsFailed)
        {
            return BadRequest(addedEventModel.Errors);
        }
        
        var result = _mapper.Map<GetEventResponse>(addedEventModel.Value);
        return Ok(result);
    }
}