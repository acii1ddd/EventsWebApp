using AutoMapper;
using EventsApp.API.Contracts.Events.Responses;
using EventsApp.Domain.Models;
using EventsApp.Domain.Models.Events;

namespace EventsApp.API.ContractProfiles.Events;

public class PaginationListEventProfile : Profile
{
    public PaginationListEventProfile()
    {
        CreateMap<PaginatedList<EventModel>, GetPaginatedListResponse<GetEventResponse>>();
    }
}