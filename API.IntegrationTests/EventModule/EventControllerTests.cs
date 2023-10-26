using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
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
        var response = await TestClient.GetAsync($"/Events/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task GetEvent_ShouldReturnEvent_WhenEventExistsInDb()
    {
        // Arrange
        await AuthenticateAsync();

        // Act
        // todo: create an event, then fetch it. Test event Creation separately.
        const string id = "cecb7257-6764-4a5c-a9f8-6412d158214a";
        var response = await TestClient.GetAsync($"/Events/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var returnedEvent = await response.Content.ReadFromJsonAsync<ApiResponse<EventResponse>>();
        returnedEvent?.Data?.Guid.Should().Be(id);
    }
}