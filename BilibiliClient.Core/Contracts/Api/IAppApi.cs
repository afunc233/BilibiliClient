using BilibiliClient.Core.Models.Https.App;

namespace BilibiliClient.Core.Contracts.Api;

public interface IAppApi
{
    // x/v2/search/square
    ValueTask<object?> SearchSquare();

    // x/v2/feed/index
    ValueTask<HomeRecommendInfo?> GetRecommend(RecommendModel recommendModel);
}