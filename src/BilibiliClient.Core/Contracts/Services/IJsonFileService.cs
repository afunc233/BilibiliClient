namespace BilibiliClient.Core.Contracts.Services;

public interface IJsonFileService
{
    /// <summary>
    /// 读取文件
    /// </summary>
    /// <param name="folderPath">路径</param>
    /// <param name="fileName">文件名</param>
    /// <param name="text2JsonConverter">Text 转化为  Json  </param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    T? Read<T>(string folderPath, string fileName, Func<string, string>? text2JsonConverter = null);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="folderPath">路径</param>
    /// <param name="fileName">文件名</param>
    /// <param name="content">数据</param>
    /// <param name="json2TextConverter"> Json 转化为 Text</param>
    /// <typeparam name="T"></typeparam>
    void Save<T>(string folderPath, string fileName, T content, Func<string, string>? json2TextConverter = null);

    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="folderPath"></param>
    /// <param name="fileName"></param>
    void Delete(string folderPath, string fileName);
}