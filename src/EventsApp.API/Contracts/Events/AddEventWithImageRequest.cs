using AutoMapper;
using EventsApp.Domain.Models.Events;

namespace EventsApp.API.Contracts.Events;

public class AddEventWithImageRequest
{
    public AddEventRequest EventData { get; init; } = null!;

    public IFormFile? ImageFile { get; init; } = null!;
}

public class AddEventRequest
{
    public string Name { get; init; } = string.Empty;
    
    public string Description { get; init; } = string.Empty;
    
    public DateTime StartDate { get; init; }
    
    public string Location { get; init; } = string.Empty;
    
    public string Category { get; init; } = string.Empty;
    
    public int MaxParticipants { get; init; }
}

public class AddEventDataProfile : Profile
{
    public AddEventDataProfile()
    {
        CreateMap<AddEventWithImageRequest, EventModel>()
            // парсим EventData (из AddEventWithImageRequest) в EventModel 
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
                => opt.MapFrom(src => src.EventData.MaxParticipants))
            .ForMember(dest => dest.ImageFile, opt
                => opt.Ignore())
            .ForMember(dest => dest.EventUsers, opt
                => opt.Ignore());
    }
}
