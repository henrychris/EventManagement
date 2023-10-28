using System.Net.Http.Headers;
using System.Net.Http.Json;
using EventModule.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.EventModels.Requests;
using Shared.EventModels.Responses;
using Shared.Responses;
using Shared.UserModels.Requests;
using Shared.UserModels.Responses;
using UserModule.Data;

namespace API.IntegrationTests;

public class IntegrationTest
{
    protected HttpClient TestClient = null!;

    [SetUp]
    public void Setup()
    {
        var webApplicationFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // remove dataContext 
                    var descriptorsToRemove = services.Where(
                        d => d.ServiceType == typeof(DbContextOptions<EventDbContext>)
                             || d.ServiceType == typeof(DbContextOptions<UserDbContext>)).ToList();

                    foreach (var descriptor in descriptorsToRemove)
                    {
                        services.Remove(descriptor);
                    }

                    // replace dataContext with in-memory versions
                    services.AddDbContext<EventDbContext>(options => { options.UseInMemoryDatabase("TestEventDB"); });
                    services.AddDbContext<UserDbContext>(options => { options.UseInMemoryDatabase("TestUserDB"); });
                });
            });
        TestClient = webApplicationFactory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("http://localhost/api/")
        });
    }

    protected async Task<EventResponse> CreateEventAsync(CreateEventRequest request)
    {
        var eventResponse = await TestClient.PostAsJsonAsync("Events", request);

        var result = await eventResponse.Content.ReadFromJsonAsync<ApiResponse<EventResponse>>();
        return result?.Data ?? throw new InvalidOperationException("Event Creation failed.");
    }

    protected async Task BuyTicket(string eventId)
    {
        await TestClient.PostAsJsonAsync($"Events/{eventId}/buy-ticket", new TicketPurchaseRequest());
    }

    protected async Task AuthenticateAsync()
    {
        TestClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("bearer", await GetJwtAsync());
    }

    private async Task<string> GetJwtAsync()
    {
        var registerResponse = await TestClient.PostAsJsonAsync("Auth/Register",
            new RegisterRequest("test", "user", "test1@example.com", "Password12@", "User"));

        var result = await registerResponse.Content.ReadFromJsonAsync<ApiResponse<UserAuthResponse>>();
        return result?.Data?.AccessToken ?? throw new InvalidOperationException("Registration failed.");
    }
}