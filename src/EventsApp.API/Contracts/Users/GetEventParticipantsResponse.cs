namespace EventsApp.API.Contracts.Users;

public class GetEventParticipantsResponse
{
    public List<GetUserResponse> Participants { get; init; } = [];
}