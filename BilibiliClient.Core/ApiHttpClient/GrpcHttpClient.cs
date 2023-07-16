using System.Net.Http.Headers;
using Bili.Models.gRPC;
using BilibiliClient.Core.Contracts.Api;
using BilibiliClient.Core.Contracts.ApiHttpClient;
using BilibiliClient.Core.Contracts.Services;
using BilibiliClient.Core.Contracts.Utils;
using Google.Protobuf;
using Microsoft.Extensions.Logging;

namespace BilibiliClient.Core.ApiHttpClient;

public class GrpcHttpClient : AbsHttpClient, IGrpcHttpClient
{
    private readonly IAuthenticationProvider _authenticationProvider;

    public GrpcHttpClient(HttpClient httpClient, IAuthenticationProvider authenticationProvider, IJsonUtils jsonUtils,
        IApiErrorCodeHandlerService apiErrorCodeHandlerService, ILogger<GrpcHttpClient> logger) : base(httpClient,
        jsonUtils,
        apiErrorCodeHandlerService, logger)
    {
        _authenticationProvider = authenticationProvider;
        httpClient.BaseAddress = new Uri(ApiConstants.GrpcUrl);
    }

    public async ValueTask<HttpRequestMessage> BuildRequestMessage(string requestUri, IMessage grpcMessage,
        bool needToken = false)
    {
        await Task.CompletedTask;

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);

        var isTokenValid = await _authenticationProvider.IsTokenValidAsync();
        var token = string.Empty;
        if (needToken || isTokenValid)
        {
            token = await _authenticationProvider.GetTokenAsync();
        }

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

    public async ValueTask<T> SendAsync<T>(HttpRequestMessage requestMessage, MessageParser<T> parser) where T : IMessage<T>
    {
        var response = await _httpClient.SendAsync(requestMessage);
        
        var bytes = await response.Content.ReadAsByteArrayAsync();
        return parser.ParseFrom(bytes.Skip(5).ToArray());
    }

    // private class GRPCConfig
    // {
    //     /// <summary>
    //     /// 系统版本.
    //     /// </summary>
    //     public const string OSVersion = "14.6";
    //
    //     /// <summary>
    //     /// 厂商.
    //     /// </summary>
    //     public const string Brand = "Apple";
    //
    //     /// <summary>
    //     /// 手机系统.
    //     /// </summary>
    //     public const string Model = "iPhone 11";
    //
    //     /// <summary>
    //     /// 应用版本.
    //     /// </summary>
    //     public const string AppVersion = "6.7.0";
    //
    //     /// <summary>
    //     /// 构建标识.
    //     /// </summary>
    //     public const int Build = 6070600;
    //
    //     /// <summary>
    //     /// 频道.
    //     /// </summary>
    //     public const string Channel = "bilibili140";
    //
    //     /// <summary>
    //     /// 网络状况.
    //     /// </summary>
    //     public const int NetworkType = 2;
    //
    //     /// <summary>
    //     /// 未知.
    //     /// </summary>
    //     public const int NetworkTF = 0;
    //
    //     /// <summary>
    //     /// 未知.
    //     /// </summary>
    //     public const string NetworkOid = "46007";
    //
    //     /// <summary>
    //     /// 未知.
    //     /// </summary>
    //     public const string Cronet = "1.21.0";
    //
    //     /// <summary>
    //     /// 未知.
    //     /// </summary>
    //     public const string Buvid = "XZFD48CFF1E68E637D0DF11A562468A8DC314";
    //
    //     /// <summary>
    //     /// 应用类型.
    //     /// </summary>
    //     public const string MobileApp = "iphone";
    //
    //     /// <summary>
    //     /// 移动平台.
    //     /// </summary>
    //     public const string Platform = "iphone";
    //
    //     /// <summary>
    //     /// 产品环境.
    //     /// </summary>
    //     public const string Envorienment = "prod";
    //
    //     /// <summary>
    //     /// 应用Id.
    //     /// </summary>
    //     public const int AppId = 1;
    //
    //     /// <summary>
    //     /// 国家或地区.
    //     /// </summary>
    //     public const string Region = "CN";
    //
    //     /// <summary>
    //     /// 语言.
    //     /// </summary>
    //     public const string Language = "zh";
    //
    //     /// <summary>
    //     /// Initializes a new instance of the <see cref="GRPCConfig"/> class.
    //     /// </summary>
    //     /// <param name="accessToken">访问令牌.</param>
    //     public GRPCConfig(string accessToken)
    //     {
    //         this.AccessToken = accessToken;
    //     }
    //
    //     /// <summary>
    //     /// 访问令牌.
    //     /// </summary>
    //     public string AccessToken { get; set; }
    //
    //     /// <summary>
    //     /// 获取客户端在Fawkes系统中的信息标头.
    //     /// </summary>
    //     /// <returns>Base64字符串.</returns>
    //     public string GetFawkesreqBin()
    //     {
    //         var msg = new FawkesReq();
    //         msg.Appkey = MobileApp;
    //         msg.Env = Envorienment;
    //         return ToBase64(msg.ToByteArray());
    //     }
    //
    //     /// <summary>
    //     /// 获取元数据标头.
    //     /// </summary>
    //     /// <returns>Base64字符串.</returns>
    //     public string GetMetadataBin()
    //     {
    //         var msg = new Metadata();
    //         msg.AccessKey = AccessToken;
    //         msg.MobiApp = MobileApp;
    //         msg.Build = Build;
    //         msg.Channel = Channel;
    //         msg.Buvid = Buvid;
    //         msg.Platform = Platform;
    //         return ToBase64(msg.ToByteArray());
    //     }
    //
    //     /// <summary>
    //     /// 获取设备标头.
    //     /// </summary>
    //     /// <returns>Base64字符串.</returns>
    //     public string GetDeviceBin()
    //     {
    //         var msg = new Device();
    //         msg.AppId = AppId;
    //         msg.MobiApp = MobileApp;
    //         msg.Build = Build;
    //         msg.Channel = Channel;
    //         msg.Buvid = Buvid;
    //         msg.Platform = Platform;
    //         msg.Brand = Brand;
    //         msg.Model = Model;
    //         msg.Osver = OSVersion;
    //         return ToBase64(msg.ToByteArray());
    //     }
    //
    //     /// <summary>
    //     /// 获取网络标头.
    //     /// </summary>
    //     /// <returns>Base64字符串.</returns>
    //     public string GetNetworkBin()
    //     {
    //         var msg = new Network();
    //         msg.Type = Bilibili.Metadata.Network.NetworkType.Wifi;
    //         msg.Oid = NetworkOid;
    //         return ToBase64(msg.ToByteArray());
    //     }
    //
    //     /// <summary>
    //     /// 获取限制标头.
    //     /// </summary>
    //     /// <returns>Base64字符串.</returns>
    //     public string GetRestrictionBin()
    //     {
    //         var msg = new Restriction();
    //
    //         return ToBase64(msg.ToByteArray());
    //     }
    //
    //     /// <summary>
    //     /// 获取本地化标头.
    //     /// </summary>
    //     /// <returns>Base64字符串.</returns>
    //     public string GetLocaleBin()
    //     {
    //         var msg = new Locale();
    //         msg.CLocale = new LocaleIds();
    //         msg.SLocale = new LocaleIds();
    //         msg.CLocale.Language = Language;
    //         msg.CLocale.Region = Region;
    //         msg.SLocale.Language = Language;
    //         msg.SLocale.Region = Region;
    //         return ToBase64(msg.ToByteArray());
    //     }
    //
    //     /// <summary>
    //     /// 将数据转换为Base64字符串.
    //     /// </summary>
    //     /// <param name="data">数据.</param>
    //     /// <returns>Base64字符串.</returns>
    //     public string ToBase64(byte[] data)
    //     {
    //         return Convert.ToBase64String(data).TrimEnd('=');
    //     }
    // }
}