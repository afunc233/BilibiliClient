namespace BilibiliClient.Core.Contracts.Services;

public interface IWindowManagerService
{
    void OpenInNewWindow(string? pageKey, object? parameter = null);

    bool? OpenInDialog(string? pageKey, object? parameter = null);

    void OpenInShallWindow(string? key, object? parameter = null);

    /// <summary>
    /// 关闭窗体
    /// </summary>
    /// <param name="key"></param>
    void CloseWindow(string? key);
}