namespace BilibiliClient.Core.Contracts.Api;

public interface IAuthenticationProvider
{
    Task<bool> IsTokenValidAsync();

    Task<string> GetTokenAsync();
}