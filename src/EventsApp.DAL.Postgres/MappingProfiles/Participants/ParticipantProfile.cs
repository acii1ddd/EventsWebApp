using AutoMapper;
using EventsApp.DAL.Entities;
using EventsApp.Domain.Models;

namespace EventsApp.DAL.MappingProfiles.Participants;

public class ParticipantProfile : Profile
{
    public ParticipantProfile()
    {
        CreateMap<ParticipantModel, ParticipantEntity>().ReverseMap();
    }
}