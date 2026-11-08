using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Slotly.Server;
using Testcontainers.PostgreSql;
using Xunit;

public class SlotlyWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
        .WithImage("postgres:17-alpine")          // match the version you run in production
        .WithDatabase("slotly_test")
        .WithUsername("postgres")
        .WithPassword("LuxTech@dev!2026")
        .Build();

    public string ConnectionString => _postgres.GetConnectionString();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            // Remove the real DbContext registration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<DBContext>));
            if (descriptor is not null) services.Remove(descriptor);

            services.AddDbContext<DBContext>(o => o.UseNpgsql(ConnectionString));
        });
    }

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();

        // Create the schema once the container is up
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DBContext>();
        await db.Database.MigrateAsync();   // or EnsureCreatedAsync() if you have no migrations yet
    }

    public new async Task DisposeAsync() => await _postgres.DisposeAsync();
}
