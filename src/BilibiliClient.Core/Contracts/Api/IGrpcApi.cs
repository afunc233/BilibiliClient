using Bilibili.App.Dynamic.V2;
using Bilibili.App.Interfaces.V1;
using Bilibili.App.Show.V1;
using Bilibili.App.View.V1;

namespace BilibiliClient.Core.Contracts.Api;

public interface IGrpcApi
{
    ValueTask<PopularReply?> Popular(PopularResultReq popularResultReq);

    ValueTask<RankListReply?> RankRegion(RankRegionResultReq popularReq);


    ValueTask<CursorV2Reply?> GetMyHistory(Cursor historyCursor, string tabSign = "archive");


    #region 动态

    /// <summary>
    /// 动态-视频
    /// </summary>
    /// <returns></returns>
    ValueTask<DynVideoReply?> GetDynamicVideo(string? offset, string? baseline);

    /// <summary>
    /// 动态-综合
    /// </summary>
    /// <returns></returns>
    ValueTask<DynAllReply?> GetDynamicAll(string? offset, string? baseline);

    #endregion


    #region 播放相关

    ValueTask<ViewReply?> GetVideoDetailByBVId(string? bvId);
    ValueTask<ViewReply?> GetVideoDetailByAVId(string? avId);



    #endregion
}