using AutoMapper;
using EventsApp.API.Contracts.Events.Requests;
using EventsApp.API.Contracts.Events.Responses;
using EventsApp.API.Contracts.Users;
using EventsApp.BLL.Interfaces;
using EventsApp.Domain.Models;
using EventsApp.Domain.Models.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsApp.API.Controllers;

[ApiController]
[Route("api/events")]
public class EventController : BaseController
{
    private readonly IEventService _eventService;
    private readonly IMapper _mapper;
    
    public EventController(IEventService eventService, IMapper mapper)
    {
        _eventService = eventService;
        _mapper = mapper;
    }

    // /events?pageIndex=page-idex?pageSize=page-size
    [HttpGet]
    [Authorize("Default, Admin")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetPaginatedListResponse<GetEventResponse>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAllAsync([FromQuery] GetEventByPageRequest request, 
        CancellationToken cancellationToken)
    {
        var events = await _eventService.GetAllAsync(request.PageIndex, 
            request.PageSize, cancellationToken);
        
        var result = _mapper.Map<GetPaginatedListResponse<GetEventResponse>>(events);
        return Ok(result);
    }
    
    // /events/guid
    [HttpGet("{eventId:guid}")]
    [Authorize("Default, Admin")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetEventResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid eventId, CancellationToken cancellationToken)
    {
        var eventModel = await _eventService.GetByIdAsync(eventId, cancellationToken);
        var result = _mapper.Map<GetEventResponse>(eventModel);
        return Ok(result);
    }

    // /events/name
    [HttpGet("{name}")]
    [Authorize("Default, Admin")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetEventResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetByNameAsync([FromRoute] string name, CancellationToken cancellationToken)
    {
        var eventModel = await _eventService.GetByNameAsync(name, cancellationToken);
        var result = _mapper.Map<GetEventResponse>(eventModel);
        return Ok(result);
    }
    
    // events?Date=date&Location=location&Category=category
    [HttpGet("filter")]
    [Authorize("Default, Admin")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetPaginatedListResponse<GetEventResponse>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetByFilter([FromQuery] GetEventByFilterRequest request, CancellationToken cancellationToken)
    {
        var events = await _eventService.GetByFilter(
            request.Date, request.Location,
            request.Category, request.GetEventByPageRequest.PageIndex, 
            request.GetEventByPageRequest.PageSize, cancellationToken
        );
        
        var result = _mapper.Map<GetPaginatedListResponse<GetEventResponse>>(events);
        return Ok(result);
    }

    // данные должны быть получены из тела запроса в формате multipart/form-data
    [HttpPost]
    [Authorize("Admin")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetEventResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddAsync([FromForm] AddEventWithImageRequest request, CancellationToken cancellationToken)
    {
        var eventModel = _mapper.Map<EventModel>(request);
        var addedEventModel = await _eventService.AddAsync(eventModel, request.ImageFile, cancellationToken);
        var result = _mapper.Map<GetEventResponse>(addedEventModel);
        return Ok(result);
    }

    // events/guid
    [HttpPut("{eventId:guid}")]
    [Authorize("Admin")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetEventResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid eventId, [FromForm] UpdateEventWithImageRequest request, 
        CancellationToken cancellationToken)
    {
        var eventModel = _mapper.Map<EventModel>(request);
        eventModel.Id = eventId;
        
        var updatedEventModel = await _eventService.UpdateAsync(eventModel, request.ImageFile, cancellationToken);
        var result = _mapper.Map<GetEventResponse>(updatedEventModel);
        return Ok(result);
    }

    [HttpDelete("{eventId:guid}")]
    [Authorize("Admin")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetEventResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid eventId, CancellationToken cancellationToken)
    {
        var deletedEventModel = await _eventService.DeleteAsync(eventId, cancellationToken);
        var result = _mapper.Map<GetEventResponse>(deletedEventModel);
        return Ok(result);
    }

    [HttpPost("{eventId:guid}/add-image")]
    [Authorize("Admin")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddImageResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddImage([FromRoute] Guid eventId, [FromForm] AddImageRequest request, 
        CancellationToken cancellationToken)
    {
        var imageUrl = await _eventService.AddImageAsync(eventId, request.ImageFile, cancellationToken);
        return Ok(new AddImageResponse
        {
            EventId = eventId, 
            ImageUrl = imageUrl
        });
    }
    
    [HttpPost("events/{eventId:guid}/register")]
    [Authorize("Default, Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RegisterToEventAsync([FromRoute] Guid eventId, CancellationToken cancellationToken)
    {
        await _eventService.RegisterToEventAsync(eventId, AuthorizedUserId, cancellationToken);
        return Ok();
    }

    [HttpGet("{eventId:guid}/participants")]
    [Authorize("Default, Admin")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetEventParticipantsResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetEventParticipantsAsync([FromRoute] Guid eventId, CancellationToken cancellationToken)
    {
        var participants = await _eventService.GetParticipantsByIdAsync(eventId, cancellationToken);
        var result = _mapper.Map<GetEventParticipantsResponse>(participants);
        return Ok(result);
    }
    
    [HttpGet("{eventId:guid}/participants/{userId:guid}")]
    [Authorize("Default, Admin")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetUserResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetEventParticipantByIdAsync([FromRoute] Guid eventId, [FromRoute] Guid userId, 
        CancellationToken cancellationToken)
    {
        var participant = await _eventService.GetParticipantByIdAsync(eventId, userId, cancellationToken);
        var result = _mapper.Map<GetUserResponse>(participant);
        return Ok(result);
    }
    
    [HttpDelete("{eventId:guid}/participation")]
    [Authorize("Default, Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CancelEventParticipationAsync([FromRoute] Guid eventId, 
        CancellationToken cancellationToken)
    {
        await _eventService.CancelEventParticipationAsync(eventId, AuthorizedUserId, cancellationToken);
        return Ok();
    }
}
