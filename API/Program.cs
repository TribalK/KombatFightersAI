using KombatFightersAI.Core.Endpoints;
using KombatFightersAI.Domain.Commands;
using KombatFightersAI.Domain.Configuration;
using KombatFightersAI.Domain.DTO;
using KombatFightersAI.Services.AiBehavior;
using KombatFightersAI.Services.Implementations;
using KombatFightersAI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var appSettingsSection = builder.Configuration.GetSection(nameof(AppSettings));
builder.Services.Configure<AppSettings>(appSettingsSection);

var appSettings = appSettingsSection.Get<AppSettings>();

builder.Services.AddSingleton(appSettings);
builder.Services.AddSingleton<IJsonConfigDeserializer, JsonConfigDeserializer>();
builder.Services.AddSingleton<ICharacterLoader, CharacterLoader>();
builder.Services.AddTransient<ISimulationGameLogic, SimulationGameLogic>();
builder.Services.AddTransient<MinimaxPrune>();
builder.Services.AddSingleton<EndpointHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.MapControllers();
app.MapGet("/api/AllCharacterData", (EndpointHandler handler) =>
{
    return handler.GetAllCharacterDataHandler();
});

app.MapGet("/api/SelectedCharacterData/{p1CharId:int}/{p2CharId:int}", (int p1CharId, int p2CharId, EndpointHandler handler) =>
{
    return handler.GetSelectedCharacterDataHandler(p1CharId, p2CharId);
});

app.MapPost("/api/RunSimulation", async (RunSimulationRequest request, EndpointHandler handler) =>
{
    await handler.RunSimulationAsyncHandler(request);

    return Results.Ok();
});

app.Run();
