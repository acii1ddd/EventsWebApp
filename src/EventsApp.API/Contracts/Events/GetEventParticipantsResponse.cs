using AutoMapper;
using EventsApp.API.Contracts.Users;
using EventsApp.Domain.Models.Participants;

namespace EventsApp.API.Contracts.Events;

public class GetEventParticipantsResponse
{
    public List<GetUserResponse> Participants { get; init; } = [];
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