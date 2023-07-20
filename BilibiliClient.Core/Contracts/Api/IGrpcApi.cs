using Bilibili.App.Interfaces.V1;
using Bilibili.App.Show.V1;

namespace BilibiliClient.Core.Contracts.Api;

public interface IGrpcApi
{
    ValueTask<PopularReply?> Popular(PopularResultReq popularResultReq);

    ValueTask<RankListReply?> RankRegion(RankRegionResultReq popularReq);
    
    
    ValueTask<CursorV2Reply?> GetMyHistory(Cursor historyCursor, string tabSign = "archive");
}