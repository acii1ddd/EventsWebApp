using FluentResults;

namespace EventsApp.API.Errors;

public class NotFoundError : Error
{
    public NotFoundError(Guid id) 
        : base($"Событие {id} не найдено")
    {
        
    }
}
