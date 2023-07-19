using BilibiliClient.Core.Contracts;
using Microsoft.Extensions.Hosting;

namespace BilibiliClient.Core.Services;

public class BilibiliClientCoreHostedService : IHostedService
{
    private readonly IEnumerable<IStartStopHandler> _activationHandlers;

    public BilibiliClientCoreHostedService(IEnumerable<IStartStopHandler> activationHandlers)
    {
        _activationHandlers = activationHandlers.OrderBy(it => it.Order);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (var activationHandler in _activationHandlers)
        {
            await activationHandler.HandleStartAsync();
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        foreach (var activationHandler in _activationHandlers.Reverse())
        {
            await activationHandler.HandleStopAsync();
        }
    }
}