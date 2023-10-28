using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Shared.EventModels.Requests;
using Shared.EventModels.Responses;
using Shared.Responses;

namespace API.IntegrationTests.EventModule;

[TestFixture]
public class EventControllerTests : IntegrationTest
{
    [Test]
    public async Task GetEvent_ShouldReturnUnauthorized_WhenAccessTokenIsMissing()
    {
        const string id = "cecb7257-6764-4a5c-a9f8-6412d158214a";
        var response = await TestClient.GetAsync($"Events/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task GetEvent_ShouldReturnEvent_WhenEventExistsInDb()
    {
        // Arrange
        await AuthenticateAsync();

        // Act
        var createdEvent = await CreateEventAsync(new CreateEventRequest(Name: "Test Event",
            Description: "This is a test event", Price: 10.99m,
            Date: DateTime.UtcNow.AddDays(7), StartTime: DateTime.UtcNow.AddDays(7).AddHours(1),
            EndTime: DateTime.UtcNow.AddDays(7).AddHours(3)));

        var response = await TestClient.GetAsync($"Events/{createdEvent.Guid}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var returnedEvent = await response.Content.ReadFromJsonAsync<ApiResponse<EventResponse>>();
        returnedEvent?.Data?.Guid.Should().Be(createdEvent.Guid);
        returnedEvent?.Data?.Name.Should().Be(createdEvent.Name);
    }

    [Test]
    public async Task GetEventsWithAvailableTickets_ShouldReturnOneEvent_AfterBuyingTicket()
    {
        // Arrange
        await AuthenticateAsync();

        var eventWithTickets = new CreateEventRequest(Name: "Test Event",
            Description: "This is a test event", Price: 10.99m,
            Date: DateTime.UtcNow.AddDays(7), StartTime: DateTime.UtcNow.AddDays(7).AddHours(1),
            EndTime: DateTime.UtcNow.AddDays(7).AddHours(3), TicketsAvailable: 20);

        var eventWithOneTicket = new CreateEventRequest(Name: "Test Event 2",
            Description: "This is a test event", Price: 10.99m,
            Date: DateTime.UtcNow.AddDays(7), StartTime: DateTime.UtcNow.AddDays(7).AddHours(1),
            EndTime: DateTime.UtcNow.AddDays(7).AddHours(3), TicketsAvailable: 1);

        // Create two events
        await CreateEventAsync(eventWithTickets);
        var createdEvent2 = await CreateEventAsync(eventWithOneTicket);

        // Act
        await BuyTicket(createdEvent2.Guid);
        var response = await TestClient.GetAsync("Events/available-tickets");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<SearchEventResponse>>();
        apiResponse.Should().NotBeNull();
        apiResponse!.Data.Should().NotBeNull();
        apiResponse.Data!.Count.Should().Be(1);

        var returnedEvent = apiResponse.Data.Events.First();
        returnedEvent.Should().NotBeNull();
        returnedEvent.Name.Should().Be(eventWithTickets.Name);
        returnedEvent.TicketsAvailable.Should().Be(eventWithTickets.TicketsAvailable);
    }
}