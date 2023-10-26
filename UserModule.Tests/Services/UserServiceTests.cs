using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using UserModule.Data.Models;
using UserModule.ServiceErrors;
using UserModule.Services;

namespace UserModule.Tests.Services;

[TestFixture]
public class UserServiceTests
{
    private Mock<UserManager<ApplicationUser>> _userManagerMock;
    private UserService _userService;

    [SetUp]
    public void Setup()
    {
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null!, null!,
            null!, null!, null!, null!, null!, null!);
        _userService = new UserService(_userManagerMock.Object);
    }

    [Test]
    public async Task GetUser_ReturnsUserResponse_WhenUserExists()
    {
        // Arrange
        const string userId = "1";
        var user = new ApplicationUser
        {
            Id = userId,
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@example.com",
            WalletBalance = 100,
            Role = "User"
        };
        _userManagerMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(user);

        // Act
        var result = await _userService.GetUser(userId);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Id.Should().BeEquivalentTo(userId);
        result.Value.FirstName.Should().BeEquivalentTo(user.FirstName);
        result.Value.LastName.Should().BeEquivalentTo(user.LastName);
        result.Value.EmailAddress.Should().BeEquivalentTo(user.Email);
        result.Value.WalletBalance.Should().Be(user.WalletBalance);
        result.Value.Role.Should().BeEquivalentTo(user.Role);
    }

    [Test]
    public async Task GetUser_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        const string userId = "1";
        _userManagerMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync((ApplicationUser)null!);

        // Act
        var result = await _userService.GetUser(userId);

        // Assert
        result.FirstError.Should().BeEquivalentTo(Errors.User.NotFound);
    }
}