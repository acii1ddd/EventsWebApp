using AutoMapper;
using EventsApp.Domain.Models.Events;
using FluentValidation;

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

// validators
public class AddEventWithImageRequestValidator : AbstractValidator<AddEventWithImageRequest>
{
    public AddEventWithImageRequestValidator()
    {
        RuleFor(x => x.EventData)
            .NotNull().WithMessage("Все данные события должны быть указаны")
            .SetValidator(new AddEventRequestValidator());

        RuleFor(x => x.ImageFile)
            .Must(BeAValidImageFile).WithMessage("Неподдерживаемый файл")
            .When(x => x.ImageFile is not null);
    }

    private static bool BeAValidImageFile(IFormFile? arg)
    {
        if (arg is null)
            return false;
        
        var allowedExtensions = new [] { ".jpg", ".jpeg", ".png", ".gif" };
        var extension = Path.GetExtension(arg.FileName);
        return allowedExtensions.Contains(extension);
    }
}

public class AddEventRequestValidator : AbstractValidator<AddEventRequest>
{
    public AddEventRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название события должно быть указано");
        
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Описание события должно быть указано");
        
        RuleFor(x => x.StartDate)
            .GreaterThan(DateTime.Now).WithMessage("Дата проведения события должна быть в будущем");
        
        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Место проведения события должно быть указано");
        
        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Категория события события должна быть указана");

        RuleFor(x => x.MaxParticipants)
            .GreaterThan(0).WithMessage("Максимальное количество участников события должно быть больше 0.");
    }
}

// mapper
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
