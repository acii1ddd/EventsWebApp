using AutoMapper;
using EventsApp.API.Contracts.Auth;
using EventsApp.Domain.Models.Auth;

namespace EventsApp.API.ContractProfiles.Auth;

public class SignInResponseProfile : Profile
{
    public SignInResponseProfile()
    {
        CreateMap<AuthAccessTokenModel, SignInResponse>();
    }
}