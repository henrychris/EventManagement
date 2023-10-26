using FluentValidation.TestHelper;
using Shared.UserModels.Requests;
using UserModule.Validators;

namespace UserModule.Tests.Validators;

[TestFixture]
public class RegisterRequestValidatorTests
{
    private RegisterRequestValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new RegisterRequestValidator();
    }

    [Test]
    public void Should_Have_Error_When_FirstName_Is_Too_Short()
    {
        var request = new RegisterRequest
        (
            "ab",
            "Doe",
            "test@example.com",
            "password",
            "User"
        );
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    [Test]
    public void Should_Have_Error_When_FirstName_Is_Too_Long()
    {
        var request = new RegisterRequest
        (
            "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz",
            "Doe",
            "test@example.com",
            "password",
            "User"
        );
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    [Test]
    public void Should_Have_Error_When_LastName_Is_Too_Short()
    {
        var request = new RegisterRequest
        (
            "John",
            "ab",
            "test@example.com",
            "password",
            "User"
        );
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }

    [Test]
    public void Should_Have_Error_When_LastName_Is_Too_Long()
    {
        var request = new RegisterRequest
        (
            "John",
            "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz",
            "test@example.com",
            "password",
            "User"
        );
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }

    [Test]
    public void Should_Have_Error_When_EmailAddress_Is_Invalid()
    {
        var request = new RegisterRequest
        (
            "John",
            "Doe",
            "invalid-email-address",
            "password",
            "User"
        );

        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.EmailAddress);
    }

    [Test]
    public void Should_Not_Have_Error_When_Request_Is_Valid()
    {
        var request = new RegisterRequest
        (
            "John",
            "Doe",
            "test@example.com",
            "password",
            "User"
        );
        var result = _validator.TestValidate(request);
        result.ShouldNotHaveAnyValidationErrors();
    }
}