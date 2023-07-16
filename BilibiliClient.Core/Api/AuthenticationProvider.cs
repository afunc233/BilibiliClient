using BilibiliClient.Core.Contracts.Api;

namespace BilibiliClient.Core.Api;

public class AuthenticationProvider : IAuthenticationProvider
{
    public async Task<bool> IsTokenValidAsync()
    {
        await Task.CompletedTask;
        return true;
    }

    public async Task<string> GetTokenAsync()
    {
        await Task.CompletedTask;

        return "";
    }
}