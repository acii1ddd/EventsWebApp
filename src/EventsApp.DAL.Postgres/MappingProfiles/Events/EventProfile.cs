using AutoMapper;
using EventsApp.DAL.Entities;
using EventsApp.Domain.Models;
using EventsApp.Domain.Models.Events;

namespace EventsApp.DAL.MappingProfiles.Events;

public class EventProfile : Profile
{
    public EventProfile()
    {
        CreateMap<EventModel, EventEntity>().ReverseMap();
    }
}
