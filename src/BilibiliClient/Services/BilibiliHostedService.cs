using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace BilibiliClient.Services;

public class BilibiliHostedService : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}