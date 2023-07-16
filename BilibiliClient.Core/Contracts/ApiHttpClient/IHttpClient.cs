﻿namespace BilibiliClient.Core.Contracts.ApiHttpClient;

/// <summary>
/// http 基准接口
/// </summary>
/// <typeparam name="TBaseResponse"></typeparam>
public interface IHttpClient<out TBaseResponse>
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
    /// 请求
    /// </summary>
    /// <param name="requestMessage">Request</param>
    /// <param name="customTransform"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    ValueTask<T?> SendAsync<T>(HttpRequestMessage requestMessage,
        Func<TBaseResponse, T?>? customTransform = null) where T : notnull;
}