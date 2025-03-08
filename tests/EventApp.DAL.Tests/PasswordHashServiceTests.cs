using EventsApp.BLL.Interfaces.Auth;
using EventsApp.BLL.Services.Auth;
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