using System.Net.Http.Headers;
using System.Net.Http.Json;
using EventModule.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
                    services.RemoveAll(typeof(EventDbContext));
                    services.RemoveAll(typeof(UserDbContext));
                    services.AddDbContext<EventDbContext>(options => { options.UseInMemoryDatabase("TestEventDB"); });
                    services.AddDbContext<UserDbContext>(options => { options.UseInMemoryDatabase("TestUserDB"); });
                });
            });
        TestClient = webApplicationFactory.CreateClient();
    }

    protected async Task AuthenticateAsync()
    {
        TestClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("bearer", await GetJwtAsync());
    }

    private async Task<string> GetJwtAsync()
    {
        // the in-memory database wasn't reset after the tests ran.
        // use login for now, till we switch to SQlServer DBs

        // var registerResponse = await TestClient.PostAsJsonAsync("/Auth/Register",
        //     new RegisterRequest("test", "user", "test1@example.com", "Password12@", "User"));
        var registerResponse = await TestClient.PostAsJsonAsync("/Auth/Login",
            new LoginRequest("test1@example.com", "Password12@"));

        var result = await registerResponse.Content.ReadFromJsonAsync<ApiResponse<UserAuthResponse>>();
        return result?.Data?.AccessToken ?? throw new InvalidOperationException("Registration failed.");
    }
}