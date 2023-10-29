using EventModule.Data.Models;
using EventModule.Validators;
using FluentValidation.TestHelper;

namespace EventModule.Tests.Validators;

[TestFixture]
public class EventValidatorTests
{
    private EventValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new EventValidator();
    }

    [Test]
    public void Should_Have_Error_When_Name_Is_Null()
    {
        var request = new Event
        {
            Name = null,
            Description = "Test Description",
            Date = DateTime.UtcNow.AddDays(1),
            StartTime = DateTime.UtcNow.AddHours(1),
            EndTime = DateTime.UtcNow.AddHours(2),
            Price = 10
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Test]
    public void Should_Have_Error_When_Name_Is_Too_Short()
    {
        var request = new Event
        {
            Name = "a",
            Description = "Test Description",
            Date = DateTime.UtcNow.AddDays(1),
            StartTime = DateTime.UtcNow.AddHours(1),
            EndTime = DateTime.UtcNow.AddHours(2),
            Price = 10
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Test]
    public void Should_Have_Error_When_Name_Is_Too_Long()
    {
        var request = new Event
        {
            Name = new string('a', 101),
            Description = "Test Description",
            Date = DateTime.UtcNow.AddDays(1),
            StartTime = DateTime.UtcNow.AddHours(1),
            EndTime = DateTime.UtcNow.AddHours(2),
            Price = 10
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Test]
    public void Should_Have_Error_When_Description_Is_Null()
    {
        var request = new Event
        {
            Name = "Test Name",
            Description = null,
            Date = DateTime.UtcNow.AddDays(1),
            StartTime = DateTime.UtcNow.AddHours(1),
            EndTime = DateTime.UtcNow.AddHours(2),
            Price = 10
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Test]
    public void Should_Have_Error_When_Description_Is_Too_Short()
    {
        var request = new Event
        {
            Name = "Test Name",
            Description = "a",
            Date = DateTime.UtcNow.AddDays(1),
            StartTime = DateTime.UtcNow.AddHours(1),
            EndTime = DateTime.UtcNow.AddHours(2),
            Price = 10
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Test]
    public void Should_Have_Error_When_Description_Is_Too_Long()
    {
        var request = new Event
        {
            Name = "Test Name",
            Description = new string('a', 501),
            Date = DateTime.UtcNow.AddDays(1),
            StartTime = DateTime.UtcNow.AddHours(1),
            EndTime = DateTime.UtcNow.AddHours(2),
            Price = 10
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Test]
    public void Should_Have_Error_When_Date_Is_In_The_Past()
    {
        var request = new Event
        {
            Name = "Test Name",
            Description = "Test Description",
            Date = DateTime.UtcNow.AddDays(-1),
            StartTime = DateTime.UtcNow.AddHours(1),
            EndTime = DateTime.UtcNow.AddHours(2),
            Price = 10
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Date);
    }

    [Test]
    public void Should_Have_Error_When_StartTime_Is_In_The_Past()
    {
        var request = new Event
        {
            Name = "Test Name",
            Description = "Test Description",
            Date = DateTime.UtcNow.AddDays(1),
            StartTime = DateTime.UtcNow.AddHours(-1),
            EndTime = DateTime.UtcNow.AddHours(2),
            Price = 10
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.StartTime);
    }

    [Test]
    public void Should_Have_Error_When_EndTime_Is_In_The_Past()
    {
        var request = new Event
        {
            Name = "Test Name",
            Description = "Test Description",
            Date = DateTime.UtcNow.AddDays(1),
            StartTime = DateTime.UtcNow.AddHours(1),
            EndTime = DateTime.UtcNow.AddHours(-1),
            Price = 10
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.EndTime);
    }

    [Test]
    public void Should_Have_Error_When_EndTime_Is_Before_StartTime()
    {
        var request = new Event
        {
            Name = "Test Name",
            Description = "Test Description",
            Date = DateTime.UtcNow.AddDays(1),
            StartTime = DateTime.UtcNow.AddHours(2),
            EndTime = DateTime.UtcNow.AddHours(1),
            Price = 10
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.EndTime);
    }

    [Test]
    public void Should_Have_Error_When_Price_Is_Negative()
    {
        var request = new Event
        {
            Name = "Test Name",
            Description = "Test Description",
            Date = DateTime.UtcNow.AddDays(1),
            StartTime = DateTime.UtcNow.AddHours(1),
            EndTime = DateTime.UtcNow.AddHours(2),
            Price = -1
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [Test]
    public void Should_Not_Have_Errors_When_Request_Is_Valid()
    {
        var request = new Event
        {
            Name = "Test Name",
            Description = "Test Description",
            Date = DateTime.UtcNow.AddDays(1),
            StartTime = DateTime.UtcNow.AddDays(1).AddHours(1),
            EndTime = DateTime.UtcNow.AddDays(1).AddHours(2),
            Price = 10,
            TicketsAvailable = 30
        };

        var result = _validator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Should_Have_Error_When_TicketsAvailable_IsNegative()
    {
        var request = new Event
        {
            Name = "Test Name",
            Description = "Test Description",
            Date = DateTime.UtcNow.AddDays(1),
            StartTime = DateTime.UtcNow.AddHours(1),
            EndTime = DateTime.UtcNow.AddHours(2),
            Price = 10,
            TicketsAvailable = -1
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.TicketsAvailable);
    }


    [Test]
    public void Should_Have_Error_When_TicketsAvailable_ExceedCapacity()
    {
        var request = new Event
        {
            Name = "Test Name",
            Description = "Test Description",
            Date = DateTime.UtcNow.AddDays(1),
            StartTime = DateTime.UtcNow.AddHours(1),
            EndTime = DateTime.UtcNow.AddHours(2),
            Price = 10,
            TicketsAvailable = Event.MaxEventAttendance + 100
        };

        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.TicketsAvailable);
    }
}