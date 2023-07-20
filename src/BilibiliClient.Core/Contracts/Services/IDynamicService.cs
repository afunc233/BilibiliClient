using BilibiliClient.Core.Contracts.Models;

namespace BilibiliClient.Core.Contracts.Services;

public interface IDynamicService
{
    /// <summary>
    /// 加载下一页
    /// </summary>
    /// <returns></returns>
    Task<List<object>> LoadNextPage(DynamicDataType dynamicDataType);

    /// <summary>
    /// 是否有更多数据
    /// </summary>
    bool HasMore { get; }

    /// <summary>
    /// 
    /// </summary>
    void ResetParam();
}