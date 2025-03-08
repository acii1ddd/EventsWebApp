using AutoMapper;
using EventsApp.API.Contracts.Users;
using EventsApp.Domain.Models.Participants;

namespace EventsApp.API.ContractProfiles.Users;

public class GetUserResponseProfile : Profile
{
    public GetUserResponseProfile()
    {
        CreateMap<UserModel, GetUserResponse>();
    }
}