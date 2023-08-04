using Bilibili.App.Card.V1;

namespace BilibiliClient.Core.Contracts.Services;

public interface IPopularService
{
    IAsyncEnumerable<Card> Popular();
}