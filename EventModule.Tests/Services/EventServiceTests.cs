using AutoMapper;
using ErrorOr;
using EventModule.Data.Models;
using EventModule.Interfaces;
using EventModule.Repositories;
using EventModule.ServiceErrors;
using EventModule.Services;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Enums;
using Shared.EventModels.Requests;

namespace EventModule.Tests.Services;

[TestFixture]
public class EventServiceTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IValidator<Event>> _validator;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger<EventService>> loggerMock;
    private readonly Mock<ICurrentUser> _currentUserMock;
    private readonly EventService _eventService;

    public EventServiceTests()
    {
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new EventMappingProfile())).CreateMapper();
        _validator = new Mock<IValidator<Event>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        loggerMock = new Mock<ILogger<EventService>>();
        _currentUserMock = new Mock<ICurrentUser>();
        _eventService = new EventService(_mapper, _validator.Object, _unitOfWorkMock.Object, loggerMock.Object,
            _currentUserMock.Object);
    }

    [Test]
    public async Task CreateEvent_ValidRequest_ReturnsEventResponse()
    {
        // Arrange
        var request = new CreateEventRequest(Name: "Test Event", Description: "This is a test event",
            Date: DateTime.UtcNow.AddDays(7), StartTime: DateTime.UtcNow.AddDays(7).AddHours(1),
            EndTime: DateTime.UtcNow.AddDays(7).AddHours(3), Price: 10.99m, TicketsAvailable: 20);

        _currentUserMock.Setup(x => x.UserId).Returns(new Guid().ToString);
        _validator.Setup(x => x.ValidateAsync(It.IsAny<Event>(), default))
            .ReturnsAsync(new ValidationResult());

        var eventEntity = _mapper.Map<Event>(request);
        _unitOfWorkMock.Setup(uow => uow.Events.AddAsync(It.IsAny<Event>()))
            .Callback<Event>(e => eventEntity = e);

        // Act
        var result = await _eventService.CreateEvent(request);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Guid.Should().Be(eventEntity.Id);
        result.Value.Name.Should().Be(request.Name);
        result.Value.Description.Should().Be(request.Description);
        result.Value.Price.Should().Be(request.Price);
        result.Value.Date.Should().Be(request.Date);
        result.Value.StartTime.Should().Be(request.StartTime);
        result.Value.EndTime.Should().Be(request.EndTime);
        result.Value.EventStatus.Should().Be(EventStatus.Upcoming);
        result.Value.TicketsAvailable.Should().Be(request.TicketsAvailable);
        result.Value.TicketsSold.Should().Be(0);
    }

    [Test]
    public async Task CreateEvent_InvalidRequest_ReturnsErrorList()
    {
        // Arrange
        var request = new CreateEventRequest(Name: "Tes", Description: "This is a test event", Price: 10.99m,
            Date: DateTime.UtcNow.AddDays(7), StartTime: DateTime.UtcNow.AddDays(7).AddHours(1),
            EndTime: DateTime.UtcNow.AddDays(7).AddHours(3));

        _currentUserMock.Setup(x => x.UserId).Returns(new Guid().ToString);
        var validationResult = new ValidationResult(new List<ValidationFailure>
        {
            new("Name", "Name is required")
        });
        _validator.Setup(v => v.ValidateAsync(It.IsAny<Event>(), default))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _eventService.CreateEvent(request);

        // Assert
        result.Errors.Count.Should().Be(1);
        result.Errors.Should().Contain(x => x.Type == ErrorType.Validation);
    }

    [Test]
    public async Task GetEvent_ExistingId_ReturnsEventResponse()
    {
        // Arrange
        const string eventId = "1";
        var eventEntity = new Event { Id = eventId, Name = "Test Event", OrganiserId = new Guid().ToString() };
        _unitOfWorkMock.Setup(uow => uow.Events.GetByIdAsync(eventId)).ReturnsAsync(eventEntity);

        // Act
        var result = await _eventService.GetEvent(eventId);

        // Assert
        result.Value.Guid.Should().Be(eventEntity.Id);
        result.Value.Name.Should().Be(result.Value.Name);
    }

    [Test]
    public async Task GetEvent_NonExistingId_ReturnsNotFoundError()
    {
        // Arrange
        const string eventId = "1";
        _unitOfWorkMock.Setup(uow => uow.Events.GetByIdAsync(eventId)).ReturnsAsync((Event)null);

        // Act
        var result = await _eventService.GetEvent(eventId);

        // Assert
        result.FirstError.Description.Should().Be(Errors.Event.NotFound.Description);
        result.FirstError.Code.Should().Be(Errors.Event.NotFound.Code);
    }

    [Test]
    public async Task UpdateEvent_ExistingIdAndValidRequest_ReturnsEventResponse()
    {
        // Arrange
        const string eventId = "1";
        var existingEvent = new Event { Id = eventId, Name = "Test Event", OrganiserId = new Guid().ToString() };
        var request = new UpdateEventRequest("Updated Test Event", null, null, null, null, null, null, null);

        _unitOfWorkMock.Setup(uow => uow.Events.GetByIdAsync(eventId)).ReturnsAsync(existingEvent);
        // Act
        var result = await _eventService.UpdateEvent(eventId, request);

        // Assert
        result.Value.Guid.Should().Be(result.Value.Guid);
        result.Value.Name.Should().Be(result.Value.Name);
    }

    [Test]
    public async Task UpdateEvent_NonExistingId_ReturnsNotFoundError()
    {
        // Arrange
        const string eventId = "1";
        var request = new UpdateEventRequest("Updated Test Event", null, null, null, null, null, null, null);

        _unitOfWorkMock.Setup(uow => uow.Events.GetByIdAsync(eventId)).ReturnsAsync((Event)null);

        // Act
        var result = await _eventService.UpdateEvent(eventId, request);

        // Assert
        result.FirstError.Description.Should().Be(Errors.Event.NotFound.Description);
        result.FirstError.Code.Should().Be(Errors.Event.NotFound.Code);
    }

    [Test]
    public async Task UpdateEvent_InvalidRequest_ReturnsErrorList()
    {
        // Arrange
        var eventId = "1";
        var existingEvent = new Event { Id = eventId, Name = "Test Event", OrganiserId = new Guid().ToString() };
        var request = new UpdateEventRequest(null, null, -1m, null, null, null, null, null);

        _unitOfWorkMock.Setup(uow => uow.Events.GetByIdAsync(eventId)).ReturnsAsync(existingEvent);

        var validationResult = new ValidationResult(new List<ValidationFailure>
        {
            new(Errors.Event.InvalidTicketPrice.Code, Errors.Event.InvalidTicketPrice.Description)
        });
        _validator.Setup(v => v.ValidateAsync(It.IsAny<Event>(), default))
            .ReturnsAsync(validationResult);
        // Act
        var result = await _eventService.UpdateEvent(eventId, request);

        // Assert
        result.Errors.Should().HaveCount(1);
        result.FirstError.Description.Should().Be(Errors.Event.InvalidTicketPrice.Description);
    }

    [Test]
    public async Task DeleteEvent_ExistingId_ReturnsDeleted()
    {
        // Arrange
        const string eventId = "1";
        var existingEvent = new Event { Id = eventId, OrganiserId = new Guid().ToString() };
        _unitOfWorkMock.Setup(uow => uow.Events.GetByIdAsync(eventId)).ReturnsAsync(existingEvent);

        // Act
        var result = await _eventService.DeleteEvent(eventId);

        // Assert
        result.Value.Should().BeOfType<Deleted>();
    }

    [Test]
    public async Task DeleteEvent_NonExistingId_ReturnsNotFoundError()
    {
        // Arrange
        const string eventId = "1";
        _unitOfWorkMock.Setup(uow => uow.Events.GetByIdAsync(eventId)).ReturnsAsync((Event)null);

        // Act
        var result = await _eventService.DeleteEvent(eventId);

        // Assert
        result.FirstError.Description.Should().Be(Errors.Event.NotFound.Description);
        result.FirstError.Code.Should().Be(Errors.Event.NotFound.Code);
    }

    [Test]
    public async Task BuyTicket_WhenEventNotFound_ReturnsError()
    {
        // Arrange
        const string eventId = "1";
        _unitOfWorkMock.Setup(uow => uow.Events.GetByIdAsync(eventId)).ReturnsAsync((Event)null);

        // Act
        var result = await _eventService.BuyTicket(eventId);

        // Assert
        result.FirstError.Should().Be(Errors.Event.NotFound);
    }

    [Test]
    public async Task BuyTicket_WhenNoTicketsAvailable_ReturnsError()
    {
        // Arrange
        const string eventId = "1";
        var eventObj = new Event { TicketsAvailable = 0, OrganiserId = new Guid().ToString() };
        _unitOfWorkMock.Setup(uow => uow.Events.GetByIdAsync(eventId)).ReturnsAsync(eventObj);

        // Act
        var result = await _eventService.BuyTicket(eventId);

        // Assert
        result.FirstError.Should().Be(Errors.Event.NoTicketsAvailable);
    }

    [Test]
    public async Task BuyTicket_WhenTicketsAvailable_ReturnsTicketPurchaseResponse()
    {
        // Arrange
        const string eventId = "1";
        var eventObj = new Event { TicketsAvailable = 1, Name = "Test", OrganiserId = new Guid().ToString() };
        _unitOfWorkMock.Setup(uow => uow.Events.GetByIdAsync(eventId)).ReturnsAsync(eventObj);

        // Act
        var result = await _eventService.BuyTicket(eventId);

        // Assert
        result.Value.EventName.Should().Be(eventObj.Name);
    }
}