using AutoMapper;
using EventsApp.API.Contracts.Events;
using EventsApp.Domain.Abstractions.Events;
using EventsApp.Domain.Abstractions.Users;
using EventsApp.Domain.Errors;
using EventsApp.Domain.Models.Events;
using EventsApp.Domain.Models.Participants;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsApp.API.Controllers;

[Route("api/events")]
[ApiController]
public class EventController : BaseController
{
    private readonly IEventService _eventService;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    
    // private readonly Guid _authorizedUserId = Guid.Parse("9d86f170-9372-4f8e-adc7-18b42bc7d09b");
    
    public EventController(IEventService eventService, IMapper mapper, IUserService userService)
    {
        _eventService = eventService;
        _mapper = mapper;
        _userService = userService;
    }

    // /events?pageIndex=page-idex?pageSize=page-size
    [HttpGet]
    [Authorize("Default, Admin")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetEventResponse>))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAllAsync([FromQuery] GetEventByPageRequest request)
    {
        // TODO: Валидация Fluent Validation на GetEventByPageRequest
        var events = await _eventService.GetAllAsync(request.PageIndex, request.PageSize);
        
        var result = _mapper.Map<GetPaginatedListResponse<GetEventResponse>>(events);
        return Ok(result);
    }
    
    // /events/guid
    [HttpGet("{eventId:guid}")]
    [Authorize("Default, Admin")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetEventResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
    [Authorize("Default, Admin")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetEventResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
    [Authorize("Admin")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetEventResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<Error>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
    [Authorize("Admin")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetEventResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
    [Authorize("Admin")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetEventResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
    [Authorize("Default, Admin")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetPaginatedListResponse<GetEventResponse>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetByFilter([FromQuery] GetEventByFilterRequest request)
    {
        // TODO: Валидация Fluent Validation для GetEventByFilterRequest
        var events = await _eventService.GetByFilter(request.Date, request.Location,
            request.Category, request.GetEventByPageRequest.PageIndex, request.GetEventByPageRequest.PageSize);
        
        var result = _mapper.Map<GetPaginatedListResponse<GetEventResponse>>(events);
        return Ok(result);
    }

    [HttpPost("{eventId:guid}/add-image")]
    [Authorize("Default, Admin")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddImageResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
        return error is EventWithIdNotFoundError ? NotFound(error) : BadRequest(error);
    }
    
    [HttpPost("events/{eventId:guid}/register")]
    [Authorize("Default, Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
    public async Task<IActionResult> RegisterToEventAsync([FromRoute] Guid eventId)
    {
        var registerResult = await _userService.RegisterToEventAsync(eventId, AuthorizedUserId);
        if (registerResult.IsFailed)
        {
            var error = registerResult.Errors.First();
            if (error is UserWithIdNotFoundError or UserWithEmailNotFoundError)
                return NotFound(error);
        }

        if (registerResult.Value)
        {
            return Ok();
        }
        return BadRequest("Пользователь уже зарегистрирован на это событие");
    }

    [HttpGet("{eventId:guid}/participants")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetEventParticipantsResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetEventParticipantsAsync([FromRoute] Guid eventId)
    {
        var participants = await _eventService.GetParticipantsByIdAsync(eventId);

        if (participants is null) 
            return BadRequest();

        var result = _mapper.Map<GetEventParticipantsResponse>(participants);
        return Ok(result);
    }
    
    // TODO:  Получение определенного участника по его Id;
    
    // TODO: Отмена участия пользователя в событии; 
}
