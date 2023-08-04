namespace BilibiliClient.Core.Api.HttpsClient;

internal class HttpHeaderHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return base.SendAsync(request, cancellationToken);
    }
}