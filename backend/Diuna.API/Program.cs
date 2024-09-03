using Microsoft.Extensions.Logging;
using Diuna.API.Hubs;
using Diuna.Services.Background;
using Diuna.Services.Gpio;
using Diuna.Services.Managers;
using Diuna.Services.Switch;
using Diuna.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container
builder.Services.AddControllers();

builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000", "http://172.17.0.3")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
        });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

if (OperatingSystem.IsLinux())
{
    builder.Services.AddSingleton<IGpioService, GpioService>();
}
else if (OperatingSystem.IsWindows())
{
    builder.Services.AddSingleton<IGpioService, MockGpioService>();
}

//builder.Services.AddScoped<IConfigManager, ConfigManager>();
//builder.Services.AddScoped<IStateManager, StateManager>();
builder.Services.AddScoped<IScriptService, ScriptService>();

builder.Services.AddSingleton<IStateManager, StateManager>();
builder.Services.AddSingleton<IConfigManager, ConfigManager>();

builder.Services.AddSingleton<ISwitchService, SwitchService>();

builder.Services.AddHostedService<StateSaveBackgroundService>();

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();

logger.LogInformation($"{DateTime.Now.ToString("[dd.MM.yyyy HH:mm:fffffff]: ")} Application is starting...");

// Explicitly call the Initialize method within a scope
using (var scope = app.Services.CreateScope())
{
    var switchService = scope.ServiceProvider.GetRequiredService<ISwitchService>();
    switchService.Initialize();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UseCors("AllowSpecificOrigins");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<SwitchHub>("/switchhub");

app.Run();
