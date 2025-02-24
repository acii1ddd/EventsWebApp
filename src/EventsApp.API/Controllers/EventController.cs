using AutoMapper;
using EventsApp.API.Contracts.Events;
using EventsApp.API.Errors;
using EventsApp.Domain.Abstractions.Events;
using EventsApp.Domain.Models.Events;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace EventsApp.API.Controllers;

[Route("api/events")]
[ApiController]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;
    private readonly IMapper _mapper;
    
    private readonly Guid _authorizedUserId = Guid.Parse("9d86f170-9372-4f8e-adc7-18b42bc7d09b");
    
    public EventController(IEventService eventService, IMapper mapper)
    {
        _eventService = eventService;
        _mapper = mapper;
    }

    // /events?pageIndex=page-idex?pageSize=page-size
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetEventResponse>))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAllAsync([FromQuery] GetEventByPageRequest request)
    {
        // TODO: Валидация Fluent Validation на GetEventByPageRequest
        var events = await _eventService.GetAllAsync(request.PageIndex, request.PageSize);
        
        var result = _mapper.Map<GetPaginatedListResponse<GetEventResponse>>(events);
        return Ok(result);
    }
    
    // /events/guid
    [HttpGet("{eventId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetEventResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid eventId)
    {
        var eventModel = await _eventService.GetByIdAsync(eventId);

        if (eventModel is null) {
            return NotFound();
        }
        
        var result = _mapper.Map<GetEventResponse>(eventModel);
        return Ok(result);
    }

    // /events/name
    [HttpGet("{name}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetEventResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByNameAsync([FromRoute] string name)
    {
        var eventModel = await _eventService.GetByNameAsync(name);

        if (eventModel is null) {
            return NotFound();
        }
        
        var result = _mapper.Map<GetEventResponse>(eventModel);
        return Ok(result);
    }

    // данные должны быть получены из тела запроса в формате multipart/form-data
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetEventResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<Error>))]
    public async Task<IActionResult> AddAsync([FromForm] AddEventWithImageRequest request)
    {
        // TODO: Валидация FluentValidation
        var eventModel = _mapper.Map<EventModel>(request);
        
        var addedEventModel = await _eventService.AddAsync(eventModel, request.ImageFile);
        
        if (addedEventModel.IsFailed) {
            return BadRequest(addedEventModel.Errors);
        }
        
        var result = _mapper.Map<GetEventResponse>(addedEventModel.Value);
        return Ok(result);
    }

    // events/guid
    [HttpPut("{eventId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetEventResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid eventId, 
        [FromForm] UpdateEventWithImageRequest request)
    {
        // TODO: Валидация FluentValidation для UpdateEventWithImageRequest.EventData
        var eventModel = _mapper.Map<EventModel>(request);
        eventModel.Id = eventId;
        
        var updatedEventModel = await _eventService.UpdateAsync(eventModel, request.ImageFile);

        if (updatedEventModel is null) {
            return NotFound();
        }
        
        var result = _mapper.Map<GetEventResponse>(updatedEventModel);
        return Ok(result);
    }

    [HttpDelete("{eventId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetEventResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid eventId)
    {
        var deletedEventModel = await _eventService.DeleteAsync(eventId);
        
        if (deletedEventModel is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<GetEventResponse>(deletedEventModel);
        return Ok(result);
    }

    // events?Date=date&Location=location&Category=category
    [HttpGet("filter")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetPaginatedListResponse<GetEventResponse>))]
    public async Task<IActionResult> GetByFilter([FromQuery] GetEventByFilterRequest request)
    {
        // TODO: Валидация Fluent Validation для GetEventByFilterRequest
        var events = await _eventService.GetByFilter(request.Date, request.Location,
            request.Category, request.GetEventByPageRequest.PageIndex, request.GetEventByPageRequest.PageSize);
        
        var result = _mapper.Map<GetPaginatedListResponse<GetEventResponse>>(events);
        return Ok(result);
    }

    [HttpPost("{eventId:guid}/add-image")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddImageResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> AddImage([FromRoute] Guid eventId, [FromForm] AddImageRequest request)
    {
        var imageUrlResult = await _eventService.AddImageAsync(eventId, request.ImageFile);

        // success
        if (!imageUrlResult.IsFailed)
        {
            return Ok(new AddImageResponse
            {
                EventId = eventId, 
                ImageUrl = imageUrlResult.Value
            });
        }
        
        var error = imageUrlResult.Errors.First();
        return error is NotFoundError ? NotFound(error) : BadRequest(error);
    }
}
