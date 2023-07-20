using BilibiliClient.Core.Models.Https;

namespace BilibiliClient.Core.Contracts.ApiHttpClient;

public interface IPassportHttpClient : IHttpClient<ApiResponse>
{
    ValueTask<HttpResponseMessage> Send4ResponseAsync(HttpRequestMessage request);
}