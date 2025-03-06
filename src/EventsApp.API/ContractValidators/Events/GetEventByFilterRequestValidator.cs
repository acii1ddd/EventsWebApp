using EventsApp.API.Contracts.Events.Requests;
using FluentValidation;

namespace EventsApp.API.ContractValidators.Events;

public class GetEventByFilterRequestValidator : AbstractValidator<GetEventByFilterRequest>
{
    public GetEventByFilterRequestValidator()
    {
        RuleFor(x => x.GetEventByPageRequest)
            .NotNull().WithMessage("Данные для пагинации не могут быть пустыми")
            .SetValidator(new GetEventByPageRequestValidator());
    }
}