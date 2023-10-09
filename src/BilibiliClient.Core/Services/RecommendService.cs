using BilibiliClient.Core.Api.Contracts.Api;
using BilibiliClient.Core.Contracts.Services;
using BilibiliClient.Core.Models.Https.App;

namespace BilibiliClient.Core.Services;

internal class RecommendService(IAppApi appApi) : IRecommendService
{
    private long _idx = 0;
    private readonly IAppApi _appApi = appApi;

    public async IAsyncEnumerable<RecommendCardItem> GetRecommend()
    {
        var homeRecommend = await _appApi.GetRecommend(new RecommendModel()
        {
            Idx = _idx,
        });
        if (homeRecommend is not { Items: not null }) yield break;
        foreach (var homeRecommendItem in homeRecommend.Items)
        {
            yield return homeRecommendItem;
        }

        var newIdx = homeRecommend.Items.LastOrDefault()?.Idx ?? 0;
        if (_idx < newIdx)
        {
            _idx = newIdx;
        }
    }
}