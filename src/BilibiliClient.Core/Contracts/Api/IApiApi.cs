using BilibiliClient.Core.Models.Https.Api;

namespace BilibiliClient.Core.Contracts.Api;

public interface IApiApi
{
    #region 播放相关

    ValueTask<VideoPlayUrlResult?> GetVideoPlayUrl(string  avId, string cId);

    #endregion
}