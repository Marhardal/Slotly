namespace Tests;
using System.Net;

public class StatusTests : IClassFixture<SlotlyWebApplicationFactory>
{
    private readonly SlotlyWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly DatabaseResetter _resetter = new();

    public StatusTests(SlotlyWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    public Task InitializeAsync() => _resetter.ResetAsync(_factory.ConnectionString);
    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task Api_starts_and_talks_to_postgres()
    {
        var response = await _client.GetAsync("/Statuses"); // use any simple endpoint you have
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}


[CollectionDefinition("Integration")]
public class IntegrationCollection : ICollectionFixture<SlotlyWebApplicationFactory> { }
