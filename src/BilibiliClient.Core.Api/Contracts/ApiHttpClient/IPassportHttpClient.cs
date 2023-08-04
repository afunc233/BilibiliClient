using BilibiliClient.Core.Models.Https;

namespace BilibiliClient.Core.Api.Contracts.ApiHttpClient;

internal interface IPassportHttpClient : IHttpClient<ApiResponse>
{
    ValueTask<HttpResponseMessage> Send4ResponseAsync(HttpRequestMessage request);
}