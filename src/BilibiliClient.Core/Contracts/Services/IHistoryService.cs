using Bilibili.App.Interfaces.V1;

namespace BilibiliClient.Core.Contracts.Services;

public interface IHistoryService
{
    /// <summary>
    /// 加载下一页
    /// </summary>
    /// <returns></returns>
    Task<List<CursorItem>?> LoadNextPage();


    /// <summary>
    /// 是否有更多数据
    /// </summary>
    /// <returns></returns>
    bool HasMore { get; }

    /// <summary>
    /// 重置
    /// </summary>
    void ResetCursor();
}