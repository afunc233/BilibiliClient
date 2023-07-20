using BilibiliClient.Core.Models.Https;
using Google.Protobuf;

namespace BilibiliClient.Core.Contracts.ApiHttpClient;

public interface IGrpcHttpClient : IHttpClient<ApiResponse>
{
    /// <summary>
    /// 构建一个 Grpc  的 HttpRequestMessage
    /// </summary>
    /// <param name="requestUri"></param>
    /// <param name="message"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    ValueTask<HttpRequestMessage> BuildRequestMessage(string requestUri, IMessage message, string? token = null);

    /// <summary>
    /// 解析发送并解析 GRPC 的数据
    /// </summary>
    /// <param name="requestMessage"></param>
    /// <param name="parser"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    ValueTask<T> SendAsync<T>(HttpRequestMessage requestMessage, MessageParser<T> parser) where T : IMessage<T>;
}