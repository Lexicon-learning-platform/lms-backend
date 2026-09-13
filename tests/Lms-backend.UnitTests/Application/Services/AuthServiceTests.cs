using Lms_backend.Application.Models;
using Lms_backend.Application.Services;
using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Enums;
using Lms_backend.UnitTests.TestHelpers;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Lms_backend.UnitTests.Application.Services;

public class AuthServiceTests
{
    private static IConfiguration BuildConfiguration() => new ConfigurationBuilder()
        .AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["JwtSettings:AccessSecret"] = "TestAccessSecretKeyThatIsLongEnoughForHmacSha256Signing1234567890",
            ["JwtSettings:RefreshSecret"] = "TestRefreshSecretKeyThatIsLongEnoughForHmacSha256Signing0987654321",
            ["JwtSettings:Issuer"] = "test-issuer",
            ["JwtSettings:Audience"] = "test-audience",
        })
        .Build();

    private static (FakeUserManager, FakeAuthRepository, AuthService) CreateService()
    {
        var userManager = FakeUserManager.Create();
        var authRepository = new FakeAuthRepository();
        var service = new AuthService(authRepository, BuildConfiguration(), userManager);
        return (userManager, authRepository, service);
    }

    private static ApplicationUser NewUser(Guid? id = null, string userName = "existing.user") => new()
    {
        Id = id ?? Guid.NewGuid(),
        UserName = userName,
        GivenName = "Existing",
        LastName = "User",
    };

    private static string BuildRefreshTokenString(Guid userId, string username, string role)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.Name, username),
            new(ClaimTypes.Role, role),
        };
        var token = new JwtSecurityToken(claims: claims, expires: DateTime.UtcNow.AddDays(7));
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // --- Login ---

    [Fact]
    public async Task Login_ReturnsTokensAndSuccess_WhenCredentialsAreValid()
    {
        var (userManager, authRepository, service) = CreateService();
        var user = NewUser(userName: "jane.doe");
        userManager.UsersList.Add(user);
        userManager.CheckPasswordHandler = (u, p) => u == user && p == "correct-password";
        var model = new LoginDto { Username = "jane.doe", Password = "correct-password", Role = "Student" };

        var (tokens, response) = await service.Login(model);

        Assert.Equal(ActionResponse.Success, response);
        Assert.NotNull(tokens);
        Assert.Equal(2, tokens!.Count);

        var accessToken = tokens[0];
        Assert.Equal(user.Id.ToString(), accessToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
        Assert.Equal(model.Username, accessToken.Claims.First(c => c.Type == ClaimTypes.Name).Value);
        Assert.Equal(model.Role, accessToken.Claims.First(c => c.Type == ClaimTypes.Role).Value);
        Assert.True(accessToken.ValidTo <= DateTime.UtcNow.AddMinutes(15).AddSeconds(5));
        Assert.True(accessToken.ValidTo >= DateTime.UtcNow.AddMinutes(15).AddSeconds(-30));

        var refreshTokenCall = Assert.Single(authRepository.StoreRefreshTokenCalls);
        Assert.Equal(user.Id, refreshTokenCall.UserId);
    }

    [Fact]
    public async Task Login_ReturnsUserNotFound_WhenUserDoesNotExist()
    {
        var (_, authRepository, service) = CreateService();
        var model = new LoginDto { Username = "missing.user", Password = "any", Role = "Student" };

        var (tokens, response) = await service.Login(model);

        Assert.Equal(ActionResponse.UserNotFound, response);
        Assert.Null(tokens);
        Assert.Empty(authRepository.StoreRefreshTokenCalls);
    }

    [Fact]
    public async Task Login_ReturnsPasswordMismatch_WhenPasswordIsIncorrect()
    {
        var (userManager, authRepository, service) = CreateService();
        var user = NewUser(userName: "jane.doe");
        userManager.UsersList.Add(user);
        userManager.CheckPasswordHandler = (_, _) => false;
        var model = new LoginDto { Username = "jane.doe", Password = "wrong-password", Role = "Student" };

        var (tokens, response) = await service.Login(model);

        Assert.Equal(ActionResponse.PasswordMismatch, response);
        Assert.Null(tokens);
        Assert.Empty(authRepository.StoreRefreshTokenCalls);
    }

    // --- GetNewToken ---

    [Fact]
    public void GetNewToken_ReturnsNewAccessTokenWithSameClaims_WhenRefreshTokenIsValid()
    {
        var (_, authRepository, service) = CreateService();
        authRepository.IsRefreshTokenValidResult = true;
        var userId = Guid.NewGuid();
        var refreshTokenString = BuildRefreshTokenString(userId, "jane.doe", "Student");

        var newAccessToken = service.GetNewToken(refreshTokenString);

        Assert.NotNull(newAccessToken);
        Assert.Equal(userId.ToString(), newAccessToken!.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
        Assert.Equal("jane.doe", newAccessToken.Claims.First(c => c.Type == ClaimTypes.Name).Value);
        Assert.Equal("Student", newAccessToken.Claims.First(c => c.Type == ClaimTypes.Role).Value);
        Assert.True(newAccessToken.ValidTo <= DateTime.UtcNow.AddMinutes(15).AddSeconds(5));
        Assert.Equal([refreshTokenString], authRepository.IsRefreshTokenValidCalls);
    }

    [Fact]
    public void GetNewToken_ReturnsNull_WhenRefreshTokenIsInvalid()
    {
        var (_, authRepository, service) = CreateService();
        authRepository.IsRefreshTokenValidResult = false;
        var refreshTokenString = BuildRefreshTokenString(Guid.NewGuid(), "jane.doe", "Student");

        var newAccessToken = service.GetNewToken(refreshTokenString);

        Assert.Null(newAccessToken);
    }

    // --- Logout ---

    [Fact]
    public async Task Logout_ReturnsRepositoryResult_WhenTokenIsRevoked()
    {
        var (_, authRepository, service) = CreateService();
        authRepository.RevokeRefreshTokenResult = ActionResponse.Success;

        var result = await service.Logout("some-refresh-token");

        Assert.Equal(ActionResponse.Success, result);
        Assert.Equal(["some-refresh-token"], authRepository.RevokeRefreshTokenCalls);
    }

    [Fact]
    public async Task Logout_ReturnsRepositoryResult_WhenTokenIsNotFound()
    {
        var (_, authRepository, service) = CreateService();
        authRepository.RevokeRefreshTokenResult = ActionResponse.Failure;

        var result = await service.Logout("unknown-refresh-token");

        Assert.Equal(ActionResponse.Failure, result);
    }

    // --- RegisterStudent ---

    [Fact]
    public async Task RegisterStudent_CreatesStudentAndReturnsSuccess_WhenUsernameIsAvailable()
    {
        var (userManager, _, service) = CreateService();
        var data = new RegisterDto { Username = "new.student", Password = "P@ssw0rd!" };

        var result = await service.RegisterStudent(data);

        Assert.Equal(ActionResponse.Success, result);
        var created = Assert.Single(userManager.CreatedUsers);
        Assert.Equal(data.Username, created.UserName);
        Assert.Equal("Student", created.Role);
        var passwordCall = Assert.Single(userManager.AddPasswordCalls);
        Assert.Equal(created, passwordCall.User);
        Assert.Equal(data.Password, passwordCall.Password);
    }

    [Fact]
    public async Task RegisterStudent_ReturnsUserAlreadyExists_WhenUsernameIsTaken()
    {
        var (userManager, _, service) = CreateService();
        var data = new RegisterDto { Username = "existing.student", Password = "P@ssw0rd!" };
        userManager.UsersList.Add(NewUser(userName: data.Username));

        var result = await service.RegisterStudent(data);

        Assert.Equal(ActionResponse.UserAlreadyExists, result);
        Assert.Empty(userManager.CreatedUsers);
    }
}
