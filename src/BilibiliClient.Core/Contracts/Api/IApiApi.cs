namespace BilibiliClient.Core.Contracts.Api;

public interface IApiApi
{
    #region 播放相关

    ValueTask<object?> GetVideoPlayUrl(string  avId, string cId);

    #endregion
}