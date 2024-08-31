using Diuna.Services.Gpio;
using Diuna.Services.Managers;
using Diuna.SignalR.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod();
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
else if(OperatingSystem.IsWindows())
{
    builder.Services.AddSingleton<IGpioService, MockGpioService>();
}

builder.Services.AddScoped<IConfigManager, ConfigManager>();
builder.Services.AddScoped<IStateManager, StateManager>();
builder.Services.AddScoped<ISwitchService, SwitchService>();

var app = builder.Build();

app.UseCors("AllowFrontend");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<SwitchHub>("/switchhub");

app.Run();
