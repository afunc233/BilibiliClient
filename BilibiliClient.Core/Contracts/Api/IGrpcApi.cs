using Bilibili.App.Show.V1;

namespace BilibiliClient.Core.Contracts.Api;

public interface IGrpcApi
{
    ValueTask<PopularReply?> Popular(PopularResultReq popularResultReq);
}