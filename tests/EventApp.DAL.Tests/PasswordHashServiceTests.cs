using EventsApp.BLL.Services.Auth;
using EventsApp.Domain.Abstractions.Auth;
using Xunit.Abstractions;

namespace EventApp.DAL.Tests;

public class PasswordHashServiceTests
{
    private readonly IPasswordHashService _passwordHashService;
    private readonly ITestOutputHelper _testOutputHelper;

    public PasswordHashServiceTests(ITestOutputHelper testOutputHelper)
    {
        _passwordHashService = new PasswordHashService();
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void GetHash_ShouldReturnHash()
    {
        var hash = _passwordHashService.HashPassword("TestPassword");
        _testOutputHelper.WriteLine(hash);
    }
}