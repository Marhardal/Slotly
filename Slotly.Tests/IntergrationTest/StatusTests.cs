using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Slotly.Server;
using Slotly.Server.Seeds;
using Xunit;

namespace Tests.IntergrationTest;

[Collection("Integration")]
public class StatusesCrudTests : IAsyncLifetime
{
    private const string Route = "/api/Statuses";

    private readonly SlotlyWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly DatabaseResetter _resetter = new();

    public StatusesCrudTests(SlotlyWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    public Task InitializeAsync() => _resetter.ResetAsync(_factory.ConnectionString);
    public Task DisposeAsync() => Task.CompletedTask;

    // ---------- helpers ----------

    private async Task<JsonElement> CreateAsync(
        string name, Category category = Category.Bookings, string? description = null)
    {
        var response = await _client.PostAsJsonAsync(Route, new { name, category, description });
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        return await response.Content.ReadFromJsonAsync<JsonElement>();
    }

    private static Guid IdOf(JsonElement e) => e.GetProperty("id").GetGuid();

    // Respawn wipes seeded rows, so put the default Pending status back when a test needs it
    private async Task EnsurePendingStatusExistsAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DBContext>();

        if (!await db.Statuses.AnyAsync(s => s.ID == StatusSeed.PendingId))
        {
            db.Statuses.Add(new StatusModel
            {
                ID = StatusSeed.PendingId,
                Name = "Pending",
                Category = Category.Bookings
            });
            await db.SaveChangesAsync();
        }
    }

    // ---------- READ ----------

    [Fact]
    public async Task GetAll_returns_404_when_no_statuses_exist()
    {
        var response = await _client.GetAsync(Route);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetAll_returns_all_created_statuses()
    {
        await CreateAsync("Pending");
        await CreateAsync("Confirmed");

        var response = await _client.GetAsync(Route);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var items = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal(2, items.GetArrayLength());
        var body = items.ToString();
        Assert.Contains("Pending", body);
        Assert.Contains("Confirmed", body);
    }

    [Fact]
    public async Task GetById_returns_the_requested_status()
    {
        var created = await CreateAsync("Cancelled", Category.Bookings, "Booking was cancelled");

        var response = await _client.GetAsync($"{Route}/{IdOf(created)}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var item = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("Cancelled", item.GetProperty("name").GetString());
        Assert.Equal("Booking was cancelled", item.GetProperty("description").GetString());
    }

    [Fact]
    public async Task GetById_returns_404_when_status_does_not_exist()
    {
        var response = await _client.GetAsync($"{Route}/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // ---------- CREATE ----------

    [Fact]
    public async Task Create_returns_201_with_location_and_persists_the_status()
    {
        var response = await _client.PostAsJsonAsync(Route,
            new { name = "Completed", category = Category.Bookings, description = "The status is completed" });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Headers.Location);

        var created = await response.Content.ReadFromJsonAsync<JsonElement>();
        var fetched = await _client.GetFromJsonAsync<JsonElement>($"{Route}/{IdOf(created)}");
        Assert.Equal("Completed", fetched.GetProperty("name").GetString());
    }

    [Fact]
    public async Task Create_with_missing_name_returns_400()
    {
        var response = await _client.PostAsJsonAsync(Route, new { category = "Testing" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    // ---------- UPDATE ----------

    [Fact]
    public async Task Update_changes_the_status_name()
    {
        var created = await CreateAsync("Pending", Category.Bookings);
        var id = IdOf(created);
        
        var response = await _client.PutAsJsonAsync($"{Route}/{id}",
            new { id, name = "Approved", category = Category.Bookings, description = "Approved now" });

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var fetched = await _client.GetFromJsonAsync<JsonElement>($"{Route}/{id}");
        Assert.Equal("Approved", fetched.GetProperty("name").GetString());
    }

    [Fact]
    public async Task Update_returns_400_when_route_id_and_body_id_differ()
    {
        var created = await CreateAsync("Pending");

        var response = await _client.PutAsJsonAsync($"{Route}/{IdOf(created)}",
            new { id = Guid.NewGuid(), name = "Mismatch", category = Category.Bookings });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Update_returns_404_when_status_does_not_exist()
    {
        var id = Guid.NewGuid();

        var response = await _client.PutAsJsonAsync($"{Route}/{id}",
            new { id, name = "Ghost", category = Category.Bookings });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // ---------- DELETE ----------

    [Fact]
    public async Task Delete_removes_the_status()
    {
        var created = await CreateAsync("Temporary");
        var id = IdOf(created);

        var response = await _client.DeleteAsync($"{Route}/{id}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var after = await _client.GetAsync($"{Route}/{id}");
        Assert.Equal(HttpStatusCode.NotFound, after.StatusCode);
    }

    [Fact]
    public async Task Delete_returns_404_when_status_does_not_exist()
    {
        var response = await _client.DeleteAsync($"{Route}/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_returns_409_for_the_default_pending_status()
    {
        await EnsurePendingStatusExistsAsync();

        var response = await _client.DeleteAsync($"{Route}/{StatusSeed.PendingId}");

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }
}

[CollectionDefinition("Integration")]
public class IntegrationCollection : ICollectionFixture<SlotlyWebApplicationFactory> { }
