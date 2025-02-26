using FluentResults;

namespace EventsApp.Domain.Errors;

public class UserWithEmailNotFoundError : Error
{
    public UserWithEmailNotFoundError(string email)
        : base($"Неверный email. Пользователь с email {email} не найден")
    {
    }
}
