using AutoMapper;
using EventsApp.API.Contracts.Events.Responses;
using EventsApp.Domain.Models.Events;

namespace EventsApp.API.ContractProfiles.Events;

public class GetEventResponseProfile : Profile
{
    public GetEventResponseProfile()
    {
        CreateMap<EventModel, GetEventResponse>();
    }
}