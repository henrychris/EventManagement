using ErrorOr;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.UserModels.Requests;
using Shared.UserModels.Responses;
using UserModule.Data.Models;
using UserModule.Interfaces;
using UserModule.ServiceErrors;
using AuthenticationService = UserModule.Services.AuthenticationService;

namespace UserModule.Tests.Services;

[TestFixture]
public class AuthenticationServiceTests
{
    private Mock<ITokenService> _tokenServiceMock;
    private Mock<UserManager<ApplicationUser>> _userManagerMock;
    private Mock<SignInManager<ApplicationUser>> _signInManagerMock;
    private Mock<ILogger<AuthenticationService>> _loggerMock;
    private Mock<IValidator<RegisterRequest>> _validatorMock;
    private AuthenticationService _authenticationService;

    [SetUp]
    public void Setup()
    {
        _tokenServiceMock = new Mock<ITokenService>();
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(
            Mock.Of<IUserStore<ApplicationUser>>(),
            null!, null!, null!, null!, null!, null!, null!, null!);
        _signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
            _userManagerMock.Object,
            Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(), null!, null!, null!, null!);

        _loggerMock = new Mock<ILogger<AuthenticationService>>();
        _validatorMock = new Mock<IValidator<RegisterRequest>>();
        _authenticationService = new AuthenticationService(
            _tokenServiceMock.Object,
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _loggerMock.Object,
            _validatorMock.Object);
    }

    [Test]
    public async Task RegisterAsync_ShouldReturnError_WhenUserAlreadyExists()
    {
        // Arrange
        var request = new RegisterRequest
        (
            "John",
            "Doe",
            "test@example.com",
            "password",
            "User"
        );

        _userManagerMock.Setup(x => x.FindByEmailAsync(request.EmailAddress))
            .ReturnsAsync(new ApplicationUser());

        // Act
        var result = await _authenticationService.RegisterAsync(request);

        // Assert
        result.FirstError.Should().BeEquivalentTo(Errors.User.DuplicateEmail);
    }

    [Test]
    public async Task RegisterAsync_ShouldReturnError_WhenRequestIsInvalid()
    {
        // Arrange
        var request = new RegisterRequest
        (
            "John",
            "Doe",
            "testexample.com",
            "password",
            "User"
        );

        _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<RegisterRequest>(), new CancellationToken()))
            .ReturnsAsync(new ValidationResult(new[]
            {
                new ValidationFailure(Errors.User.InvalidEmailAddress.Code, Errors.User.InvalidEmailAddress.Description)
            }));
        // Act
        var result = await _authenticationService.RegisterAsync(request);

        // Assert that a validation error is returned
        result.Errors.Should().Contain(x => x.Type == ErrorType.Validation);
    }

    [Test]
    public async Task RegisterAsync_ShouldCreateUser_WhenRequestIsValid()
    {
        // Arrange
        var request = new RegisterRequest
        (
            "John",
            "Doe",
            "test@example.com",
            "Password123@",
            "User"
        );
        var newUser = new ApplicationUser
        {
            Id = "1",
            FirstName = "John",
            LastName = "Doe",
            Email = request.EmailAddress,
            UserName = request.EmailAddress,
            Role = request.Role
        };

        _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<RegisterRequest>(), new CancellationToken()))
            .ReturnsAsync(new ValidationResult());

        _userManagerMock.Setup(x => x.FindByEmailAsync(request.EmailAddress))
            .ReturnsAsync((ApplicationUser)null!);

        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), request.Password))
            .ReturnsAsync(IdentityResult.Success)
            .Callback<ApplicationUser, string>((user, _) =>
            {
                user.Id = newUser.Id;
                user.FirstName = newUser.FirstName;
                user.LastName = newUser.LastName;
                user.Email = newUser.Email;
                user.UserName = newUser.UserName;
                user.Role = newUser.Role;
            });

        // Act
        var result = await _authenticationService.RegisterAsync(request);

        // Assert
        result.Value.Should().BeEquivalentTo(new UserAuthResponse
        (
            newUser.Id,
            newUser.FirstName,
            newUser.LastName,
            newUser.Email,
            newUser.WalletBalance,
            newUser.Role,
            It.IsAny<string>()
        ));
    }

    [Test]
    public async Task RegisterAsync_ShouldReturnError_WhenCreateAsyncFailsForValidRequest()
    {
        // Arrange
        var request = new RegisterRequest
        (
            "John",
            "Doe",
            "test@example.com",
            "Password123@",
            "User"
        );

        _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<RegisterRequest>(), new CancellationToken()))
            .ReturnsAsync(new ValidationResult());

        _userManagerMock.Setup(x => x.FindByEmailAsync(request.EmailAddress))
            .ReturnsAsync((ApplicationUser)null!);

        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), request.Password))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError
            {
                Code = Errors.Auth.LoginFailed.Code,
                Description = Errors.Auth.LoginFailed.Description
            }));

        // Act
        var result = await _authenticationService.RegisterAsync(request);
        
        // Assert
        result.Errors.Should().Contain(x => x.Type == ErrorType.Validation);
    }

    [Test]
    public async Task LoginAsync_ShouldReturnError_WhenUserNotFound()
    {
        // Arrange
        var request = new LoginRequest("test@example.com", "password");
        _userManagerMock.Setup(x => x.FindByEmailAsync(request.EmailAddress))
            .ReturnsAsync((ApplicationUser)null!);

        // Act
        var result = await _authenticationService.LoginAsync(request);

        // Assert
        result.FirstError.Should().BeEquivalentTo(Errors.User.NotFound);
    }

    [Test]
    public async Task LoginAsync_ShouldReturnError_WhenPasswordIsIncorrect()
    {
        // Arrange
        var request = new LoginRequest("test@example.com", "password");
        var user = new ApplicationUser { Id = "1", Email = request.EmailAddress };
        _userManagerMock.Setup(x => x.FindByEmailAsync(request.EmailAddress))
            .ReturnsAsync(user);
        _signInManagerMock.Setup(x => x.CheckPasswordSignInAsync(user, request.Password, false))
            .ReturnsAsync(SignInResult.Failed);

        // Act
        var result = await _authenticationService.LoginAsync(request);

        // Assert
        result.FirstError.Should().BeEquivalentTo(Errors.Auth.LoginFailed);
    }

    [Test]
    public async Task LoginAsync_ShouldReturnUserAuthResponse_WhenLoginIsSuccessful()
    {
        // Arrange
        var request = new LoginRequest("test@example.com", "password");
        var user = new ApplicationUser
        {
            Id = "1",
            FirstName = "John",
            LastName = "Doe",
            Email = request.EmailAddress,
            UserName = request.EmailAddress,
            Role = "User"
        };
        _userManagerMock.Setup(x => x.FindByEmailAsync(request.EmailAddress))
            .ReturnsAsync(user);
        _signInManagerMock.Setup(x => x.CheckPasswordSignInAsync(user, request.Password, false))
            .ReturnsAsync(SignInResult.Success);
        _tokenServiceMock.Setup(x => x.CreateUserJwt(user.Email, user.Role, user.Id))
            .Returns(It.IsAny<string>());

        // Act
        var result = await _authenticationService.LoginAsync(request);

        // Assert
        result.Value.Should().BeEquivalentTo(new UserAuthResponse
        (
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.WalletBalance,
            user.Role,
            It.IsAny<string>()
        ));
    }

    [Test]
    public async Task LoginAsync_WithLockedOutUser_ReturnsIsLockedOutError()
    {
        // Arrange
        var request = new LoginRequest(
            "test@example.com",
            "password"
        );

        var user = new ApplicationUser
        {
            Id = "1",
            FirstName = "John",
            LastName = "Doe",
            Email = "test@example.com",
            Role = "User",
            WalletBalance = 100,
            LockoutEnd = DateTimeOffset.UtcNow.AddMinutes(30)
        };

        _userManagerMock.Setup(x => x.FindByEmailAsync(request.EmailAddress)).ReturnsAsync(user);
        _signInManagerMock.Setup(x => x.CheckPasswordSignInAsync(user, request.Password, false))
            .ReturnsAsync(SignInResult.LockedOut);

        // Act
        var result = await _authenticationService.LoginAsync(request);

        // Assert
        result.FirstError.Should().BeEquivalentTo(Errors.User.IsLockedOut);
    }

    [Test]
    public async Task LoginAsync_WithNotAllowedUser_ReturnsIsNotAllowedError()
    {
        // Arrange
        var request = new LoginRequest
        (
            "test@example.com",
            "password"
        );
        var user = new ApplicationUser
        {
            Id = "1",
            FirstName = "John",
            LastName = "Doe",
            Email = "test@example.com",
            Role = "User",
            WalletBalance = 100
        };
        _userManagerMock.Setup(x => x.FindByEmailAsync(request.EmailAddress)).ReturnsAsync(user);
        _signInManagerMock.Setup(x => x.CheckPasswordSignInAsync(user, request.Password, false))
            .ReturnsAsync(SignInResult.NotAllowed);

        // Act
        var result = await _authenticationService.LoginAsync(request);

        // Assert
        result.FirstError.Should().BeEquivalentTo(Errors.User.IsNotAllowed);
    }
}