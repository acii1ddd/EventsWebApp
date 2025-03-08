using AutoMapper;
using EventsApp.DAL.Entities;
using EventsApp.Domain.Models.Events;

namespace EventsApp.BLL.MappingProfiles;

public class EventProfile : Profile
{
    public EventProfile()
    {
        CreateMap<EventModel, EventEntity>().ReverseMap();
    }
}