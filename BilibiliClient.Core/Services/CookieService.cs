using System.Net;
using BilibiliClient.Core.Contracts.Services;

namespace BilibiliClient.Core.Services;

public class CookieService : ICookieService
{
    private readonly CookieContainer _cookieContainer;

    public CookieService(CookieContainer cookieContainer)
    {
        _cookieContainer = cookieContainer;
    }

    public async ValueTask LoadCookie(List<string> domainList, Func<string, CookieCollection> getCookieCollectionFunc)
    {
        await Task.CompletedTask;

        foreach (var domain in domainList)
        {
            var cookieCollection = getCookieCollectionFunc?.Invoke(domain);
            if (cookieCollection != null)
            {
                _cookieContainer.Add(cookieCollection);
            }
        }
    }
}