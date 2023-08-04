using System.Security.Cryptography;
using System.Text;

namespace BilibiliClient.Core.Utils;

public static class BuvidUtil
{
    public static string Buvid()
    {
        var mac = new List<string>();
        Random r = new();
        for (int i = 0; i < 6; i++)
        {
            var min = Math.Min(0, 0xff);
            var max = Math.Max(0, 0xff);
            var num = int.Parse((r.Next() * (min - max + 1) + max).ToString()).ToString("x");
            mac.Add(num);
        }

        var md5 = Encoding.UTF8.GetString(MD5.HashData(Encoding.UTF8.GetBytes(string.Join(":", mac))));
        var md5Arr = md5.Split(' ');
        return $"XY${md5Arr[2]}${md5Arr[12]}${md5Arr[22]}${md5}";
    }
}