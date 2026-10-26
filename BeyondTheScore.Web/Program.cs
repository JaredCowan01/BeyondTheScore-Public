using Auth0.AspNetCore.Authentication;
using BeyondTheScore.ServiceDefaults;
using BeyondTheScore.Web.Components;
using BeyondTheScore.Web.Helpers;
using BeyondTheScore.Web.RefitInterface;
using BeyondTheScore.Web.Services.Ai;
using BeyondTheScore.Web.SignalR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.SemanticKernel;
using Radzen;
using Refit;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAuth0WebAppAuthentication(options =>
{
    options.Domain = builder.Configuration["Auth0:Domain"]!;
    options.ClientId = builder.Configuration["Auth0:ClientId"]!;
    options.ClientSecret = builder.Configuration["Auth0:ClientSecret"];
}).WithAccessToken(options =>
{
    options.Audience = builder.Configuration["Auth0:Audience"];
});

builder.Services.Configure<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.LoginPath = "/login";
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<AuthHeaderHandler>();

builder.Services.AddSignalR();
builder.Services.AddOutputCache();
builder.Services.AddRadzenComponents();

builder.Services.AddScoped<UserPreferenceState>();
builder.Services.AddTransient<StudioPanelService>();

builder.Services.AddScoped<Kernel>(_ =>
{
    IKernelBuilder kernelBuilder = Kernel.CreateBuilder();
    
    kernelBuilder.AddAzureOpenAIChatCompletion(
        deploymentName: builder.Configuration["AzureOpenAI:DeploymentName"]!,
        endpoint: builder.Configuration["AzureOpenAI:Endpoint"]!,
        apiKey: builder.Configuration["AzureOpenAI:ApiKey"]!
    );

    return kernelBuilder.Build();
});

builder.Services.AddRefitClient<IFootballAnalyticsApi>()
    .ConfigureHttpClient(client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["FootballApiUrl"]!);
    }).AddHttpMessageHandler<AuthHeaderHandler>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    // Content Security Policy – adjust as needed for your app
    context.Response.Headers["Content-Security-Policy"] =
        "default-src 'self'; " +
        "script-src 'self' 'unsafe-inline'; " +
        "style-src 'self' 'unsafe-inline'; " +
        "img-src 'self' data: https:; " +
        "connect-src 'self' wss: https:; " +
        "frame-ancestors 'self';";

    // Clickjacking protection
    context.Response.Headers["X-Frame-Options"] = "SAMEORIGIN";

    // Control referrer information
    context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";

    // Restrict powerful browser features (update based on what you actually need)
    context.Response.Headers["Permissions-Policy"] =
        "geolocation=(), microphone=(), camera=(), fullscreen=(self), payment=()";

    await next();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/login", async context =>
{
    await context.ChallengeAsync(Auth0Constants.AuthenticationScheme, new AuthenticationProperties { RedirectUri = "/" });
});

app.MapGet("/logout", async context =>
{
    var authenticationProperties = new AuthenticationProperties
    {
        RedirectUri = "/"
    };

    // Sign out of Auth0
    await context.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
    
    // Sign out of the local application cookie
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
});

app.UseAntiforgery();

app.UseOutputCache();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.MapHub<MatchHub>("/matchhub");

app.Run();