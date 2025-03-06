using AutoMapper;
using EventsApp.API.Contracts.Auth;
using EventsApp.Domain.Models.Auth;

namespace EventsApp.API.ContractProfiles.Auth;

public class SignInProfile : Profile
{
    public SignInProfile()
    {
        CreateMap<SignInRequest, SignInModel>();
    }
}