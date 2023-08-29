namespace BilibiliClient.Core.Api.Contracts.ApiHttpClient;

/// <summary>
/// http 基准接口
/// </summary>
/// <typeparam name="TBaseResponse"></typeparam>
internal interface IHttpClient<out TBaseResponse>
{
    /// <summary>
    /// 构建请求信息
    /// </summary>
    /// <param name="url"></param>
    /// <param name="method"></param>
    /// <param name="paramPairs"></param>
    /// <param name="httpContent"></param>
    /// <returns></returns>
    ValueTask<HttpRequestMessage> BuildRequestMessage(string url, HttpMethod method,
        List<KeyValuePair<string, string>>? paramPairs = null, HttpContent? httpContent = null);


    /// <summary>
    /// 
    /// </summary>
    /// <param name="httpRequestMessage"></param>
    /// <param name="cookie"></param>
    /// <returns></returns>
    ValueTask AddCookie(HttpRequestMessage httpRequestMessage, string? cookie = null);

    /// <summary>
    /// 请求
    /// </summary>
    /// <param name="requestMessage">Request</param>
    /// <param name="customTransform"></param>
    /// <param name="errorCodeHandler"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    ValueTask<T?> SendAsync<T>(HttpRequestMessage requestMessage,
        Func<TBaseResponse, T?>? customTransform = null, Func<long, string?, bool>? errorCodeHandler = null)
        where T : notnull;
}