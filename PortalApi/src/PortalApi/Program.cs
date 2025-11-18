using PortalApi;

var builder = WebApplication.CreateBuilder(args);

// Add ABP services
builder.Host.AddAppSettingsSecretsJson()
    .UseAutofac();

await builder.AddApplicationAsync<PortalApiModule>();

var app = builder.Build();

await app.InitializeApplicationAsync();

app.MapGet("/", () => "Hello from ABP Portal API!");

await app.RunAsync();
