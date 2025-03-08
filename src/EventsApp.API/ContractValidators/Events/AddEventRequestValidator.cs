using EventsApp.API.Contracts.Events.Requests;
using FluentValidation;

namespace EventsApp.API.ContractValidators.Events;

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