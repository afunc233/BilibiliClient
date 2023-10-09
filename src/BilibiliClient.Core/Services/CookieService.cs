using System.Net;
using BilibiliClient.Core.Contracts.Services;

namespace BilibiliClient.Core.Services;

public class CookieService(CookieContainer cookieContainer) : ICookieService
{
    private readonly CookieContainer _cookieContainer = cookieContainer;

    public async ValueTask LoadCookie(List<string> domainList, Func<string, CookieCollection> getCookieCollectionFunc)
    {
        await Task.CompletedTask;

        foreach (var cookieCollection in domainList.Select(getCookieCollectionFunc.Invoke))
        {
            _cookieContainer.Add(cookieCollection);
        }
    }
}