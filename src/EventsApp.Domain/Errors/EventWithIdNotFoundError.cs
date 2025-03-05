using System;
using FluentResults;

namespace EventsApp.Domain.Errors;

public class EventWithIdNotFoundError : Error
{
    public EventWithIdNotFoundError(Guid id) 
        : base($"Событие {id} не найдено")
    {
    }
}
