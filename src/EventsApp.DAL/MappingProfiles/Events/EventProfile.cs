using AutoMapper;
using EventsApp.DAL.Entities;
using EventsApp.Domain.Models;

namespace EventsApp.DAL.MappingProfiles.Events;

public class EventProfile : Profile
{
    public EventProfile()
    {
        CreateMap<EventModel, EventEntity>().ReverseMap();
    }
}
