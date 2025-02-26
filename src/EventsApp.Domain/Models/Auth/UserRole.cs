namespace EventsApp.Domain.Models.Auth;

public enum UserRole
{
    /// <summary>
    /// Обычный участник
    /// </summary>
    Default = 0,
    
    /// <summary>
    /// Администратор
    /// </summary>
    Admin = 1,
}