using AutoMapper;
using EventsApp.DAL.Entities;
using EventsApp.Domain.Models.RefreshTokens;

namespace EventsApp.DAL.MappingProfiles.RefreshTokens;

public class RefreshTokenProfile : Profile
{
    public RefreshTokenProfile()
    {
        CreateMap<RefreshTokenModel, RefreshTokenEntity>().ReverseMap();
    }
}