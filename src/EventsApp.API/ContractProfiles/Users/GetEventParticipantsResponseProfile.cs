using AutoMapper;
using EventsApp.API.Contracts.Users;
using EventsApp.Domain.Models.Participants;

namespace EventsApp.API.ContractProfiles.Users;

public class GetEventParticipantsResponseProfile : Profile
{
    public GetEventParticipantsResponseProfile()
    {
        CreateMap<List<UserModel>, GetEventParticipantsResponse>()
            .ForMember(dest => dest.Participants, opt 
                => opt.MapFrom(src => src));
    }
}