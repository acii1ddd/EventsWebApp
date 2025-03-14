using AutoMapper;
using EventsApp.API.Contracts.Users;
using EventsApp.Domain.Models.Participants;

namespace EventsApp.API.ContractProfiles.Users;

public class SignUpRequestProfile : Profile
{
    public SignUpRequestProfile()
    {
        CreateMap<SignUpRequest, UserModel>()
            .ForMember(dest => dest.PasswordHash, opt
                => opt.MapFrom(src => src.Password));
    }
}