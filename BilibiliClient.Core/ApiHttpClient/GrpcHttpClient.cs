using System.Net.Http.Headers;
using BilibiliClient.Core.Contracts.Api;
using BilibiliClient.Core.Contracts.ApiHttpClient;
using BilibiliClient.Core.Contracts.Services;
using BilibiliClient.Core.Contracts.Utils;
using BilibiliClient.Models.gRPC;
using Google.Protobuf;
using Microsoft.Extensions.Logging;

namespace BilibiliClient.Core.ApiHttpClient;

public class GrpcHttpClient : AbsHttpClient, IGrpcHttpClient
{
    public GrpcHttpClient(HttpClient httpClient, IJsonUtils jsonUtils,
        IApiErrorCodeHandlerService apiErrorCodeHandlerService, ILogger<GrpcHttpClient> logger) : base(httpClient,
        jsonUtils,
        apiErrorCodeHandlerService, logger)
    {
        httpClient.BaseAddress = new Uri(ApiConstants.GrpcUrl);
    }

    public async ValueTask<HttpRequestMessage> BuildRequestMessage(string requestUri, IMessage grpcMessage,
        string? token = null)
    {
        await Task.CompletedTask;
        token ??= string.Empty;
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);


        var grpcConfig = new GRPCConfig(token);
        var userAgent = $"bili-universal/62800300 "
                        + $"os/ios model/{GRPCConfig.Model} mobi_app/iphone "
                        + $"osVer/{GRPCConfig.OSVersion} "
                        + $"network/{GRPCConfig.NetworkType} "
                        + $"grpc-objc/1.32.0 grpc-c/12.0.0 (ios; cronet_http)";

        if (!string.IsNullOrEmpty(token))
        {
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("identify_v1", token);
        }

        requestMessage.Headers.Add("User-Agent", userAgent);
        requestMessage.Headers.Add("APP-KEY", "iphone");
        requestMessage.Headers.Add("x-bili-device-bin", grpcConfig.GetDeviceBin());
        requestMessage.Headers.Add("x-bili-fawkes-req-bin", grpcConfig.GetFawkesreqBin());
        requestMessage.Headers.Add("x-bili-locale-bin", grpcConfig.GetLocaleBin());
        requestMessage.Headers.Add("x-bili-metadata-bin", grpcConfig.GetMetadataBin());
        requestMessage.Headers.Add("x-bili-network-bin", grpcConfig.GetNetworkBin());
        requestMessage.Headers.Add("x-bili-restriction-bin", grpcConfig.GetRestrictionBin());
        requestMessage.Headers.Add("grpc-accept-encoding", "identity,deflate,gzip");
        requestMessage.Headers.Add("grpc-timeout", "20100m");
        requestMessage.Headers.Add("env", "prod");
        requestMessage.Headers.Add("Transfer-Encoding", "chunked");
        requestMessage.Headers.Add("TE", "trailers");

        var messageBytes = grpcMessage.ToByteArray();

        // 校验用?第五位为数组长度
        var stateBytes = new byte[] { 0, 0, 0, 0, (byte)messageBytes.Length };

        // 合并两个字节数组
        var bodyBytes = new byte[5 + messageBytes.Length];
        stateBytes.CopyTo(bodyBytes, 0);
        messageBytes.CopyTo(bodyBytes, 5);

        var byteArrayContent = new ByteArrayContent(bodyBytes);
        byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("application/grpc");
        byteArrayContent.Headers.ContentLength = bodyBytes.Length;

        requestMessage.Content = byteArrayContent;
        return requestMessage;
    }

    public async ValueTask<T> SendAsync<T>(HttpRequestMessage requestMessage, MessageParser<T> parser)
        where T : IMessage<T>
    {
        var response = await _httpClient.SendAsync(requestMessage);

        var bytes = await response.Content.ReadAsByteArrayAsync();
        return parser.ParseFrom(bytes.Skip(5).ToArray());
    }
}