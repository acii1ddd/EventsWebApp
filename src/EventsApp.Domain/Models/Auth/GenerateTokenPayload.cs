using System;

namespace EventsApp.Domain.Models.Auth;

public class GenerateTokenPayload
{
    public Guid UserId { get; set; }
   
    public UserRole UserRole { get; set; }
}