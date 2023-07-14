using BilibiliClient.Core.Contracts.ApiHttpClient;
using BilibiliClient.Core.Contracts.Services;
using BilibiliClient.Core.Contracts.Utils;
using Microsoft.Extensions.Logging;

namespace BilibiliClient.Core.ApiHttpClient;

public abstract class AbsHttpClient<TBaseResponse> : IHttpClient<TBaseResponse>
{
    private readonly HttpClient _httpClient;
    protected readonly IJsonUtils _jsonUtils;


    protected readonly ILogger<AbsHttpClient<TBaseResponse>> _logger;

    protected readonly IApiErrorCodeHandlerService _apiErrorCodeHandlerService;

    protected AbsHttpClient(HttpClient httpClient, IJsonUtils jsonUtils,
        IApiErrorCodeHandlerService apiErrorCodeHandlerService, ILogger<AbsHttpClient<TBaseResponse>> logger)
    {
        _httpClient = httpClient;
        _jsonUtils = jsonUtils;
        _apiErrorCodeHandlerService = apiErrorCodeHandlerService;
        _logger = logger;
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
                $"{requestUri}?{string.Join("&", listParas.Select(it => $"{it.Key}={it.Value}"))}";
        }

        return ValueTask.FromResult(new HttpRequestMessage(httpMethod, requestUriWithParam) { Content = httpContent });
    }

    public virtual async ValueTask<T?> SendAsync<T>(HttpRequestMessage requestMessage,
        Func<TBaseResponse, T?>? customTransform = null) where T : notnull
    {
        var response = await _httpClient.SendAsync(requestMessage);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();
        var baseResponse = _jsonUtils.ToObj<TBaseResponse>(result);
        if (baseResponse == null) return default;

        if (IsErrorCode(baseResponse))
        {
            await _apiErrorCodeHandlerService.HandlerApiError(GetErrorCode(baseResponse),
                GetErrorMessage(baseResponse));

            return default;
        }
        else
        {
            return customTransform != null ? customTransform(baseResponse) : Transform2T<T>(baseResponse);
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