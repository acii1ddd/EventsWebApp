using EventsApp.API.Contracts.Events.Requests;
using FluentValidation;

namespace EventsApp.API.ContractValidators.Events;

public class GetEventByPageRequestValidator : AbstractValidator<GetEventByPageRequest>
{
    public GetEventByPageRequestValidator()
    {
        RuleFor(x => x.PageIndex)
            .GreaterThan(0).WithMessage("Номер страницы должен быть больше 0.");
        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Количество элементов на странице должен быть больше 0.");
    }
}