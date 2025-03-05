using System;
using FluentResults;

namespace EventsApp.Domain.Errors;

public class UserWithIdNotFoundError : Error
{
    public UserWithIdNotFoundError(Guid id) 
        : base($"Пользователь {id} не найдено")
    {
    }
}
