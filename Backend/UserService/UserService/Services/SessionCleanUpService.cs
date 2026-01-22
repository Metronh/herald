using UserService.Interfaces.Repository;

namespace UserService.Services;

public class SessionCleanUpService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SessionCleanUpService> _logger;

    public SessionCleanUpService(ILogger<SessionCleanUpService> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await CleanUpSession();
            await Task.Delay(TimeSpan.FromMinutes(1));
            
        }
    }

    private async Task CleanUpSession()
    {
        using var scope = _serviceProvider.CreateScope();
        var userService = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        await userService.SessionCleanUp();
    }
}