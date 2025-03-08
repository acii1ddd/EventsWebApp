using AutoMapper;
using EventsApp.DAL.Entities;
using EventsApp.Domain.Models;
using EventsApp.Domain.Models.Events;

namespace EventsApp.BLL.MappingProfiles;

public class PaginatedEventListProfile : Profile
{
    public PaginatedEventListProfile()
    {
        CreateMap<PaginatedList<EventEntity>, PaginatedList<EventModel>>();
    }
}