using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace EventsApp.API.Controllers;

public class BaseController : ControllerBase
{
    protected Guid AuthorizedUserId
    {
        get
        {
            var userId = User.FindFirstValue("userId");
            return userId is null ? Guid.Empty : Guid.Parse(userId);
        }
    }
}