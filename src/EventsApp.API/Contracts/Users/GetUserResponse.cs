using AutoMapper;
using EventsApp.API.Contracts.Events;
using EventsApp.Domain.Models.Auth;
using EventsApp.Domain.Models.Participants;

namespace EventsApp.API.Contracts.Users;

public class GetUserResponse
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string Surname { get; set; } = string.Empty;
    
    public DateTime BirthDate { get; set; }
    
    public DateTime EventRegistrationDate { get; set; }
    
    public string Email { get; set; } = string.Empty;
    
    public string PasswordHash { get; set; } = string.Empty;
    
    public UserRole Role { get; set; }
}

public class GetUserResponseProfile : Profile
{
    public GetUserResponseProfile()
    {
        CreateMap<UserModel, GetUserResponse>();
    }
}
