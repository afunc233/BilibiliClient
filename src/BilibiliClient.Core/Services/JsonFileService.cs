using System.Text;
using BilibiliClient.Core.Api.Contracts.Utils;
using BilibiliClient.Core.Contracts.Services;

namespace BilibiliClient.Core.Services;

public class JsonFileService(IJsonUtils jsonUtils) : IJsonFileService
{
    private readonly IJsonUtils _jsonUtils = jsonUtils;

    public T? Read<T>(string folderPath, string fileName)
    {
        var path = Path.Combine(folderPath, fileName);
        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            return _jsonUtils.ToObj<T>(json);
        }

        return default;
    }

    public void Save<T>(string folderPath, string fileName, T content)
    {
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        var fileContent = _jsonUtils.ToJson(content);
        File.WriteAllText(Path.Combine(folderPath, fileName), fileContent, Encoding.UTF8);
    }

    public T? Read<T>(string folderPath, string fileName, Func<string, string>? text2JsonConverter)
    {
        var path = Path.Combine(folderPath, fileName);
        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            if (text2JsonConverter != null)
            {
                json = text2JsonConverter.Invoke(json);
            }

            return _jsonUtils.ToObj<T>(json);
        }

        return default;
    }

    public void Save<T>(string folderPath, string fileName, T content, Func<string, string>? json2TextConverter)
    {
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        var json = _jsonUtils.ToJson(content);

        if (json2TextConverter != null)
        {
            json = json2TextConverter.Invoke(json);
        }

        File.WriteAllText(Path.Combine(folderPath, fileName), json, Encoding.UTF8);
    }

    public void Delete(string folderPath, string? fileName)
    {
        if (fileName != null && File.Exists(Path.Combine(folderPath, fileName)))
        {
            File.Delete(Path.Combine(folderPath, fileName));
        }
    }
}