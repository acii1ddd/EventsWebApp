using AutoMapper;
using EventsApp.API.Contracts.Events;
using EventsApp.Domain.Models.Events;

namespace EventsApp.API.MappingProfiles;

public class AddEventDataProfile : Profile
{
    public AddEventDataProfile()
    {
        CreateMap<AddEventWithImageRequest, EventModel>()
            // парсим EventData (из json) в EventModel 
            .ForMember(dest => dest.Name, opt
                => opt.MapFrom(src => src.EventData.Name))
            .ForMember(dest => dest.Description, opt
                => opt.MapFrom(src => src.EventData.Description))
            .ForMember(dest => dest.StartDate, opt
                => opt.MapFrom(src => src.EventData.StartDate))
            .ForMember(dest => dest.Location, opt
                => opt.MapFrom(src => src.EventData.Location))
            .ForMember(dest => dest.Category, opt
                => opt.MapFrom(src => src.EventData.Category))
            .ForMember(dest => dest.MaxParticipants, opt
                => opt.MapFrom(src => src.EventData.MaxParticipants));
    }
}