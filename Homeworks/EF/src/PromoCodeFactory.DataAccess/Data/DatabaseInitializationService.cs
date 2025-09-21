using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PromoCodeFactory.Core.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess.Data
{
    public class DatabaseInitializationService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private ILogger<DatabaseInitializationService> _logger;

        public DatabaseInitializationService(
            IServiceProvider serviceProvider,
            ILogger<DatabaseInitializationService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting database initialization service...");
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var initializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();
                await initializer.InitializeAsync();
                _logger.LogInformation("Database initialization completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database initialization failed");
                throw;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Database initialization service stopped");
            return Task.CompletedTask;
        }
    }
}
