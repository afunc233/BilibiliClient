namespace BilibiliClient.Core.Api.Contracts.Utils;

public interface IJsonUtils
{
    string ToJson<T>(T? obj);

    T? ToObj<T>(string obj);
}