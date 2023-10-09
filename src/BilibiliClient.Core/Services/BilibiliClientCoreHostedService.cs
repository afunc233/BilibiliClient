using BilibiliClient.Core.Contracts;
using Microsoft.Extensions.Hosting;

namespace BilibiliClient.Core.Services;

public class BilibiliClientCoreHostedService(IEnumerable<IStartStopHandler> activationHandlers) : IHostedService
{
    private readonly IEnumerable<IStartStopHandler> _startStopHandlers = activationHandlers.OrderBy(it => it.Order);

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (var activationHandler in _startStopHandlers)
        {
            await activationHandler.HandleStartAsync();
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        foreach (var activationHandler in _startStopHandlers.Reverse())
        {
            await activationHandler.HandleStopAsync();
        }
    }
}