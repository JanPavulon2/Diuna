using Diuna.Services.Gpio;
using Diuna.Services.Managers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if(OperatingSystem.IsLinux())
{
    builder.Services.AddSingleton<IGpioService, GpioService>();
}
else if(OperatingSystem.IsWindows())
{
    builder.Services.AddSingleton<IGpioService, MockGpioService>();
}

// Register AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSingleton<IConfigManager, ConfigManager>();
builder.Services.AddSingleton<IStateManager, StateManager>();
builder.Services.AddSingleton<ISwitchService, SwitchService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
