using EventsApp.API.Contracts.Events.Requests;
using FluentValidation;

namespace EventsApp.API.ContractValidators.Events;

public class AddImageRequestValidator : AbstractValidator<AddImageRequest>
{
    public AddImageRequestValidator()
    {
        RuleFor(x => x.ImageFile)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Файл должен быть указан")
            .Must(BeAValidImageFile).WithMessage("Неподдерживаемый файл");

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