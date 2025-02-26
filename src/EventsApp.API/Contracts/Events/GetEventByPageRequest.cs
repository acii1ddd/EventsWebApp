namespace EventsApp.API.Contracts.Events;

public class GetEventByPageRequest
{
    public int PageIndex { get; init; }
    
    public int PageSize { get; init; }
}
