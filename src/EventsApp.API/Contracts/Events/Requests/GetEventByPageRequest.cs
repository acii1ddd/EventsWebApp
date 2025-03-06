namespace EventsApp.API.Contracts.Events.Requests;

public class GetEventByPageRequest
{
    public int PageIndex { get; init; }
    
    public int PageSize { get; init; }
}
