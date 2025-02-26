using AutoMapper;
using EventsApp.Domain.Models.Participants;

namespace EventsApp.API.Contracts.Events;

public class GetEventParticipantsResponse
{
    public List<UserModel> Participants { get; init; } = [];
}

public class GetEventParticipantsResponseProfile : Profile
{
    public GetEventParticipantsResponseProfile()
    {
        CreateMap<List<UserModel>, GetEventParticipantsResponse>()
            .ForMember(dest => dest.Participants, opt 
                => opt.MapFrom(src => src));
    }
}