using AutoMapper;
using EventsApp.DAL.Entities;
using EventsApp.Domain.Models.EventUsers;

namespace EventsApp.DAL.MappingProfiles;

public class EventUserProfile : Profile
{
    public EventUserProfile()
    {
        CreateMap<EventUserModel, EventUserEntity>().ReverseMap();
    }
}