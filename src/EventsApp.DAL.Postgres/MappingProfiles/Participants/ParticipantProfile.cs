using AutoMapper;
using EventsApp.DAL.Entities;
using EventsApp.Domain.Models;
using EventsApp.Domain.Models.Participants;

namespace EventsApp.DAL.MappingProfiles.Participants;

public class ParticipantProfile : Profile
{
    public ParticipantProfile()
    {
        CreateMap<UserModel, UserEntity>().ReverseMap();
    }
}