using Bilibili.App.Dynamic.V2;
using Bilibili.App.Interfaces.V1;
using Bilibili.App.Show.V1;
using Bilibili.App.View.V1;
using BilibiliClient.Core.Api.Configs;
using BilibiliClient.Core.Api.Contracts.Api;
using BilibiliClient.Core.Api.Contracts.ApiHttpClient;
using BilibiliClient.Core.Api.Models;

namespace BilibiliClient.Core.Api;

internal class GrpcApi : IGrpcApi
{
    private readonly IGrpcHttpClient _grpcHttpClient;
    private readonly UserSecretConfig _userSecretConfig;

    public GrpcApi(IGrpcHttpClient grpcHttpClient, UserSecretConfig userSecretConfig)
    {
        _grpcHttpClient = grpcHttpClient;
        _userSecretConfig = userSecretConfig;
    }

    public async ValueTask<PopularReply?> Popular(PopularResultReq popularReq)
    {
        await Task.CompletedTask;
        const string url = "/bilibili.app.show.v1.Popular/Index";

        var request = await _grpcHttpClient.BuildRequestMessage(url, popularReq);

        return await _grpcHttpClient.SendAsync(request, PopularReply.Parser);
    }

    public async ValueTask<RankListReply?> RankRegion(RankRegionResultReq popularReq)
    {
        await Task.CompletedTask;
        const string url = "bilibili.app.show.v1.Rank/RankRegion";

        var request = await _grpcHttpClient.BuildRequestMessage(url, popularReq);

        return await _grpcHttpClient.SendAsync(request, RankListReply.Parser);
    }

    public async ValueTask<CursorV2Reply?> GetMyHistory(Cursor historyCursor, string tabSign)
    {
        if (string.IsNullOrWhiteSpace(_userSecretConfig.AccessToken))
        {
            return default;
        }

        const string url = "bilibili.app.interface.v1.History/CursorV2";

        var req = new CursorV2Req
        {
            Business = tabSign,
            Cursor = historyCursor,
        };

        var request = await _grpcHttpClient.BuildRequestMessage(url, req, _userSecretConfig.AccessToken);

        return await _grpcHttpClient.SendAsync(request, CursorV2Reply.Parser);
    }

    #region 动态

    // bilibili.app.dynamic.v2.Dynamic/DynVideo

    public async ValueTask<DynVideoReply?> GetDynamicVideo(string? offset, string? baseline)
    {
        offset ??= string.Empty;
        baseline ??= string.Empty;
        const string url = "bilibili.app.dynamic.v2.Dynamic/DynVideo";
        var type = string.IsNullOrWhiteSpace(offset) ? Refresh.New : Refresh.History;
        var req = new DynVideoReq
        {
            RefreshType = type,
            LocalTime = 8,
            Offset = offset,
            UpdateBaseline = baseline,
        };

        var request = await _grpcHttpClient.BuildRequestMessage(url, req, _userSecretConfig.AccessToken);
        return await _grpcHttpClient.SendAsync(request, DynVideoReply.Parser);
    }

    public async ValueTask<DynAllReply?> GetDynamicAll(string? offset, string? baseline)
    {
        offset ??= string.Empty;
        baseline ??= string.Empty;
        const string url = "bilibili.app.dynamic.v2.Dynamic/DynAll";

        var type = string.IsNullOrWhiteSpace(offset) ? Refresh.New : Refresh.History;
        var req = new DynAllReq
        {
            RefreshType = type,
            LocalTime = 8,
            Offset = offset,
            UpdateBaseline = baseline,
        };

        var request = await _grpcHttpClient.BuildRequestMessage(url, req, _userSecretConfig.AccessToken);

        return await _grpcHttpClient.SendAsync(request, DynAllReply.Parser);
    }

    #endregion


    #region 播放相关

    private async ValueTask<ViewReply?> GetVideoDetail(ViewReq viewReq)
    {
        const string url = "bilibili.app.view.v1.View/View";

        var request = await _grpcHttpClient.BuildRequestMessage(url, viewReq);

        return await _grpcHttpClient.SendAsync(request, ViewReply.Parser);
    }

    public async ValueTask<ViewReply?> GetVideoDetailByBVId(string? bvId)
    {
        if (string.IsNullOrWhiteSpace(bvId))
        {
            return default;
        }

        var viewRequest = new ViewReq
        {
            Bvid = bvId
        };

        return await GetVideoDetail(viewRequest);
    }

    public async ValueTask<ViewReply?> GetVideoDetailByAVId(string? avId)
    {
        if (string.IsNullOrWhiteSpace(avId))
        {
            return default;
        }

        var viewRequest = new ViewReq
        {
            Bvid = avId
        };

        return await GetVideoDetail(viewRequest);
    }

    #endregion
}