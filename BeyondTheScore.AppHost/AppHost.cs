var builder = DistributedApplication.CreateBuilder(args);

builder.AddAzureAppServiceEnvironment("app-service-env");

var dbConnectionString = builder.AddParameter("DefaultConnection", secret: true);

var auth0Domain = builder.AddParameter("Auth0Domain");
var auth0ClientId = builder.AddParameter("Auth0ClientId");
var auth0ClientSecret = builder.AddParameter("Auth0ClientSecret", secret: true);
var auth0Audience = builder.AddParameter("Auth0Audience");

var openAiDeployment = builder.AddParameter("AzureOpenAIDeploymentName");
var openAiEndpoint = builder.AddParameter("AzureOpenAIEndpoint");
var openAiApiKey = builder.AddParameter("AzureOpenAIApiKey", secret: true);

var footballApiUrl = builder.AddParameter("FootballApiUrl");

var apiService = builder.AddProject<Projects.BeyondTheScore_ApiService>("apiservice")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithEnvironment("ConnectionStrings__DefaultConnection", dbConnectionString)
    .WithEnvironment("Auth0__Domain", auth0Domain)
    .WithEnvironment("Auth0__ClientId", auth0ClientId)
    .WithEnvironment("Auth0__ClientSecret", auth0ClientSecret)
    .WithEnvironment("Auth0__Audience", auth0Audience);

builder.AddProject<Projects.BeyondTheScore_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService)
    .WithEnvironment("AzureOpenAI__DeploymentName", openAiDeployment)
    .WithEnvironment("AzureOpenAI__Endpoint", openAiEndpoint)
    .WithEnvironment("AzureOpenAI__ApiKey", openAiApiKey)
    .WithEnvironment("Auth0__Domain", auth0Domain)
    .WithEnvironment("Auth0__ClientId", auth0ClientId)
    .WithEnvironment("Auth0__ClientSecret", auth0ClientSecret)
    .WithEnvironment("Auth0__Audience", auth0Audience)
    .WithEnvironment("FootballApiUrl", footballApiUrl);

builder.Build().Run();