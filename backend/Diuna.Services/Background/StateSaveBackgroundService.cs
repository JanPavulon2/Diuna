using Diuna.Services.Managers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Diuna.Services.Background
{
    public class StateSaveBackgroundService : BackgroundService
    {
        private readonly IStateManager _stateManager;
        private readonly TimeSpan _saveInterval = TimeSpan.FromMinutes(5);
        private readonly ILogger<StateSaveBackgroundService> _logger;

        public StateSaveBackgroundService(IStateManager stateManager, ILogger<StateSaveBackgroundService> logger)
        {
            _stateManager = stateManager;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var serviceName = "[StateSaveBackgroundService]";

            while (!stoppingToken.IsCancellationRequested)
            {
                var timestamp = DateTime.Now.ToString();

                _logger.LogInformation($"{serviceName} [{timestamp}] execution started. " +
                    $"Application state will be saved to json file.");

                _stateManager.SaveStateToFile();
                
                await Task.Delay(_saveInterval, stoppingToken);
            }
        }
    }
}
