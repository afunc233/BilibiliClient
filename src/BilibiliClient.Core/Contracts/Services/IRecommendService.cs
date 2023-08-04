using BilibiliClient.Core.Models.Https.App;

namespace BilibiliClient.Core.Contracts.Services;

public interface IRecommendService
{
    IAsyncEnumerable<RecommendCardItem> GetRecommend();
}