using BilibiliClient.Core.Contracts.Services;

namespace BilibiliClient.Core.Services;

public class ApiErrorCodeHandlerService : IApiErrorCodeHandlerService
{
    private IEnumerable<IApiErrorHandler> _apiErrorHandlers;

    public ApiErrorCodeHandlerService(IEnumerable<IApiErrorHandler> apiErrorHandlers)
    {
        _apiErrorHandlers = apiErrorHandlers;
    }

    public async ValueTask HandlerApiError(long errorCode, string? errorMessage)
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
}