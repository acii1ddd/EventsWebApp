using EventsApp.API.Contracts.Validators;
using FluentValidation;

namespace EventsApp.API.Contracts.Events;

public class GetEventByFilterRequest
{
    public DateTime? Date { get; init; }
    
    public string? Location { get; init; } = string.Empty;
    
    public string? Category { get; init; } = string.Empty;
    
    public GetEventByPageRequest GetEventByPageRequest { get; init; } = null!;
}

public class GetEventByFilterRequestValidator : AbstractValidator<GetEventByFilterRequest>
{
    public GetEventByFilterRequestValidator()
    {
        RuleFor(x => x.GetEventByPageRequest)
            .NotNull().WithMessage("Данные для пагинации не могут быть пустыми")
            .SetValidator(new GetEventByPageRequestValidator());
    }
}