using System.Security.Cryptography;
using System.Text;

namespace BilibiliClient.Core.Utils;

public static class DESUtil
{
    private static readonly byte[] Key = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };
    private static readonly byte[] IV = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };

    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="plainText">加密的文本</param>
    /// <param name="key"></param>
    /// <param name="iv"></param>
    /// <returns></returns>
    public static string Encrypt(string plainText, byte[]? key = null, byte[]? iv = null)
    {
        
        key ??= Key;
        iv ??= IV;
        byte[] inputBytes = Encoding.UTF8.GetBytes(plainText);
        using var des = DES.Create();
        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, des.CreateEncryptor(key, iv), CryptoStreamMode.Write);
        cs.Write(inputBytes, 0, inputBytes.Length);
        cs.FlushFinalBlock();
        return Convert.ToBase64String(ms.ToArray());
    }

    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="cipherText">解密的文本</param>
    /// <param name="key"></param>
    /// <param name="iv"></param>
    /// <returns></returns>
    public static string Decrypt(string cipherText, byte[]? key = null, byte[]? iv = null)
    {
        key ??= Key;
        iv ??= IV;
        byte[] inputBytes = Convert.FromBase64String(cipherText);
        using var des = DES.Create();
        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, des.CreateDecryptor(key, iv), CryptoStreamMode.Write);
        cs.Write(inputBytes, 0, inputBytes.Length);
        cs.FlushFinalBlock();
        return Encoding.UTF8.GetString(ms.ToArray());
    }
}