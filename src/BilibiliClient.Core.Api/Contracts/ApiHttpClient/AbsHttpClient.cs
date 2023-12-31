﻿using System.Net.Http.Headers;
using BilibiliClient.Core.Api.Contracts.Api;
using BilibiliClient.Core.Api.Contracts.Utils;
using Microsoft.Extensions.Logging;

namespace BilibiliClient.Core.Api.Contracts.ApiHttpClient;

/// <summary>
/// HttpClient 封装，泛型的返回数据的主体
/// </summary>
/// <typeparam name="TBaseResponse"></typeparam>
internal abstract class AbsHttpClient<TBaseResponse> : IHttpClient<TBaseResponse>
{
    private const string DefaultAcceptString =
        "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";

    // ReSharper disable once InconsistentNaming
    protected readonly HttpClient _httpClient;

    // ReSharper disable once InconsistentNaming
    // ReSharper disable once MemberCanBePrivate.Global
    protected readonly IJsonUtils _jsonUtils;


    // ReSharper disable once NotAccessedField.Global
    // ReSharper disable once InconsistentNaming
    protected readonly ILogger _logger;

    // ReSharper disable once InconsistentNaming
    private readonly IEnumerable<IApiErrorHandler> _apiErrorHandlers;

    protected AbsHttpClient(HttpClient httpClient, IJsonUtils jsonUtils,
        IEnumerable<IApiErrorHandler> apiErrorHandlers, ILogger logger)
    {
        _httpClient = httpClient;
        _jsonUtils = jsonUtils;
        _apiErrorHandlers = apiErrorHandlers;
        _logger = logger;

        _httpClient.DefaultRequestHeaders.CacheControl =
            new CacheControlHeaderValue { NoCache = false, NoStore = false };
        _httpClient.DefaultRequestHeaders.Add("accept", DefaultAcceptString);
    }

    /// <summary>
    /// 从实体获取 ErrorCode
    /// </summary>
    /// <param name="apiModel"></param>
    /// <returns></returns>
    protected abstract long GetErrorCode(TBaseResponse apiModel);

    /// <summary>
    /// 是否是错误的 Code
    /// </summary>
    /// <param name="apiModel"></param>
    /// <returns></returns>
    protected abstract bool IsErrorCode(TBaseResponse apiModel);

    /// <summary>
    /// 从实体获取 ErrorMessage
    /// </summary>
    /// <param name="apiModel"></param>
    /// <returns></returns>
    protected abstract string? GetErrorMessage(TBaseResponse apiModel);

    /// <summary>
    /// 从实体 获取 内容
    /// </summary>
    /// <param name="apiModel"></param>
    /// <returns></returns>
    protected abstract object? GetContent(TBaseResponse apiModel);

    public virtual ValueTask<HttpRequestMessage> BuildRequestMessage(string requestUri, HttpMethod httpMethod,
        List<KeyValuePair<string, string>>? listParas = null, HttpContent? httpContent = null)
    {
        var requestUriWithParam = requestUri;
        if (listParas != null && listParas.Any())
        {
            requestUriWithParam =
                $"{requestUri}?{string.Join("&", listParas.Select(it => $"{it.Key}={Uri.EscapeDataString(it.Value)}"))}";
        }

        return ValueTask.FromResult(new HttpRequestMessage(httpMethod, requestUriWithParam) { Content = httpContent });
    }

    public async ValueTask AddCookie(HttpRequestMessage httpRequestMessage, string? cookie)
    {
        await Task.CompletedTask;
        var cookieString = cookie;
        if (string.IsNullOrWhiteSpace(cookie))
        {
        }

        httpRequestMessage.Headers.Add("Cookie", cookieString);
    }

    public virtual async ValueTask<T?> SendAsync<T>(HttpRequestMessage requestMessage,
        Func<TBaseResponse, T?>? customTransform = null, Func<long, string?, bool>? errorCodeHandler = null)
        where T : notnull
    {
        var response = await _httpClient.SendAsync(requestMessage);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();
        var baseResponse = _jsonUtils.ToObj<TBaseResponse>(result);
        if (baseResponse == null) return default;

        if (IsErrorCode(baseResponse))
        {
            var code = GetErrorCode(baseResponse);
            var message = GetErrorMessage(baseResponse);
            var isHandled = errorCodeHandler?.Invoke(code, message) ??
                            false;

            if (!isHandled)
            {
                await HandlerApiError(code, message);
            }

            return default;
        }
        else
        {
            return customTransform != null ? customTransform(baseResponse) : Transform2T<T>(baseResponse);
        }
    }

    protected virtual async ValueTask HandlerApiError(long errorCode, string? errorMessage)
    {
        foreach (var apiErrorHandler in _apiErrorHandlers)
        {
            if (!apiErrorHandler.CanHanded(errorCode)) continue;
            var hasHandError = await apiErrorHandler.HandError(errorCode, errorMessage);
            if (hasHandError)
            {
                break;
            }
        }
    }

    /// <summary>
    /// 转换 实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="apiModel"></param>
    /// <returns></returns>
    protected virtual T? Transform2T<T>(TBaseResponse apiModel) where T : notnull
    {
        if (IsErrorCode(apiModel))
            return default;

        var content = GetContent(apiModel);
        if (content == null)
        {
            return default;
        }

        if (typeof(T).IsValueType)
        {
            return (T)content;
        }
        else if (content is T value)
        {
            return value;
        }
        else if (typeof(object) == typeof(T))
        {
            return (T)content;
        }
        else
        {
            return _jsonUtils.ToObj<T>(content.ToString()!);
        }
    }
}