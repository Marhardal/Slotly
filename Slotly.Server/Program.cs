using Microsoft.AspNetCore.Identity;
using Scalar.AspNetCore;
using Slotly.Server;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var DefaultConnection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContextFactory<DBContext>(
    (sp, options) =>
    {
        options.UseNpgsql(DefaultConnection, sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorCodesToAdd: null
            );

            sqlOptions.CommandTimeout(60);
        });
    },
    ServiceLifetime.Scoped);

    builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<DBContext>()
    .AddDefaultTokenProviders();


// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.Configure<ScalarOptions>(options =>
{
    options.WithOpenApiRoutePattern("/openapi/v1.json");
});

var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
