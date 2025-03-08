using AutoMapper;
using EventsApp.DAL.Entities;
using EventsApp.Domain.Models.Participants;

namespace EventsApp.BLL.MappingProfiles;

public class ParticipantProfile : Profile
{
    public ParticipantProfile()
    {
        CreateMap<UserModel, UserEntity>().ReverseMap();
    }
}