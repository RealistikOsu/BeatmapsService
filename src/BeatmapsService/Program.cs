using BeatmapsService;
using BeatmapsService.Api;
using BeatmapsService.Caching;
using BeatmapsService.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Refit;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

DotEnv.Load(Path.Combine(Directory.GetCurrentDirectory(), ".env"));
builder.Configuration.AddEnvironmentVariables();

builder.Services
    .AddOptions<BeatmapOptions>()
    .Bind(builder.Configuration)
    .ValidateDataAnnotations()
    .ValidateOnStart();

var beatmapOptions = builder.Configuration.Get<BeatmapOptions>()!;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks()
    .AddRedis(
        beatmapOptions.RedisConnectionString,
        name: "redis",
        failureStatus: HealthStatus.Unhealthy,
        timeout: TimeSpan.FromSeconds(1));

builder.Services.AddSingleton<ICache, Cache>();

builder.Services
    .AddRefitClient<IOsuApi>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://osu.ppy.sh"));

builder.Services.Decorate<IOsuApi, ResilientOsuApi>();

builder.Services.AddSingleton<IOsuService, OsuService>();
builder.Services.Decorate<IOsuService, CachingOsuService>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = beatmapOptions.RedisConnectionString;
});

builder.Services.AddHttpClient();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddHttpContextAccessor();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseExceptionHandler();
app.MapHealthChecks("/_health");

app.Run($"http://{beatmapOptions.ServiceHost}:{beatmapOptions.ServicePort}");