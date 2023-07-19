namespace BilibiliClient.Core.Contracts;

public interface IStartStopHandler
{
    /// <summary>
    /// 排序
    /// </summary>
    int Order { get; }

    /// <summary>
    /// 开始时
    /// </summary>
    /// <returns></returns>
    Task HandleStartAsync();

    /// <summary>
    /// 关闭时
    /// </summary>
    /// <returns></returns>
    Task HandleStopAsync();
}