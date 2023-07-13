using System.Security.Cryptography;
using System.Text;

namespace BilibiliClient.Core.Utils;

/// <summary>
/// https://github.com/SocialSisterYi/bilibili-API-collect/blob/master/docs/misc/sign/APPKey.md
///
/// https://github.com/SocialSisterYi/bilibili-API-collect/blob/master/docs/misc/sign/APP.md
/// </summary>
public class AppSigner
{
    public class AppSignerKeySec
    {
        public string APP_KEY { get; }
        public string APP_SEC { get; }

        private AppSignerKeySec(string appKey, string appSec)
        {
            APP_KEY = appKey;
            APP_SEC = appSec;
        }


        public static AppSignerKeySec TestAppSignerKeySec =
            new AppSignerKeySec("1d8b6e7d45233436", "560c52ccd288fed045859ed18bffd973");
        
        public static AppSignerKeySec TVAppSignerKeySec =
            new AppSignerKeySec("1d8b6e7d45233436", "560c52ccd288fed045859ed18bffd973");
    }
    // private const string APP_KEY = "4409e2ce8ffd12b8";
    //
    // private const string APP_SEC = "59b43e04ad6965f34319062b478f83dd";


    // private static final String APP_KEY = "1d8b6e7d45233436";
    // private static final String APP_SEC = "560c52ccd288fed045859ed18bffd973";

    private readonly string _appKey;
    private readonly string _appSec;

    public AppSigner(AppSignerKeySec appSignerKeySec)
    {
        _appKey = appSignerKeySec.APP_KEY;
        _appSec = appSignerKeySec.APP_SEC;
    }

    public String appSign(List<(string, string)> paramList)
    {
        // 为请求参数进行 APP 签名
        paramList.Add(("appkey", _appKey));
        // 序列化参数
        StringBuilder queryBuilder = new StringBuilder();
        foreach (var entry in paramList.OrderBy(it => it.Item1))
        {
            if (queryBuilder.Length > 0)
            {
                queryBuilder.Append('&');
            }

            queryBuilder.Append(Uri.EscapeDataString(entry.Item1))
                .Append('=')
                .Append(Uri.EscapeDataString(entry.Item2));
        }

        return generateMD5(queryBuilder.Append(_appSec).ToString());
    }

    private static String generateMD5(String input)
    {
        MD5 md = MD5.Create();
        byte[] digest = md.ComputeHash(Encoding.UTF8.GetBytes(input));
        StringBuilder sb = new StringBuilder();
        foreach (byte b in digest)
        {
            sb.Append($"{b:x2}");
        }

        return sb.ToString();
    }
}