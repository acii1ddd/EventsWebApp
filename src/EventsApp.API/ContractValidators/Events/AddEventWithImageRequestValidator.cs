using EventsApp.API.Contracts.Events.Requests;
using FluentValidation;

namespace EventsApp.API.ContractValidators.Events;

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