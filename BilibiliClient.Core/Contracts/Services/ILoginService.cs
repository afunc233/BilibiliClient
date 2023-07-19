namespace BilibiliClient.Core.Contracts.Services;

public interface ILoginService
{
    Task<string?> GetLoginQRCode();

    Task CheckHasLogin();
}