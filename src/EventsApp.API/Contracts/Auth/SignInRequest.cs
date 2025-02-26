using AutoMapper;
using EventsApp.Domain.Models.Auth;

namespace EventsApp.API.Contracts.Auth;

public class SignInRequest
{
    public string Email { get; init; } = string.Empty;
    
    public string Password { get; init; } = string.Empty;
}

public class SignInProfile : Profile
{
    public SignInProfile()
    {
        CreateMap<SignInRequest, SignInModel>();
    }
}
