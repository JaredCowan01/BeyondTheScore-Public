using BeyondTheScore.ApiService;
using BeyondTheScore.ApiService.Interfaces;
using BeyondTheScore.ApiService.Services;
using BeyondTheScore.ServiceDefaults;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}/";
    options.Audience = builder.Configuration["Auth0:Audience"];
});

builder.Services.AddAuthorization();

builder.Services.AddProblemDetails();

// OpenAPI
builder.Services.AddOpenApi();

builder.Services.AddControllers();

builder.Services.AddScoped<IMatchImportService, MatchImportService>();
builder.Services.AddScoped<IMatchAnalyticsService, MatchAnalyticsService>();
builder.Services.AddScoped<IPromptQuotaService, PromptQuotaService>();

builder.Services.AddDbContext<FootballDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    // Generates /openapi/v1.json
    app.MapOpenApi();

    // Maps Scalar UI
    app.MapScalarApiReference();
}

app.MapDefaultEndpoints();
app.MapControllers();

app.Run();