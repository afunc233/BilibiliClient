using System.Net;

namespace BilibiliClient.Core.Contracts.Services;

public interface ICookieService
{
    ValueTask LoadCookie(List<string> domainList, Func<string, CookieCollection> getCookieCollectionFunc);
}