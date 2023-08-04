using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using BilibiliClient.Core.Api.Contracts.Utils;

namespace BilibiliClient.Core.Utils;


public class CustomDateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var data = reader.GetString();
        return data == null
            ? default(DateTime)
            : DateTime.ParseExact(data, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
    }


    public override void Write(Utf8JsonWriter writer, DateTime dateTimeValue, JsonSerializerOptions options) =>
        writer.WriteStringValue(dateTimeValue.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
}

public class AutoNumberToStringConverter : JsonConverter<string?>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeof(string) == typeToConvert;
    }

    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Number:
                return reader.TryGetInt64(out long l) ? l.ToString() : reader.GetDouble().ToString(CultureInfo.InvariantCulture);
            case JsonTokenType.String:
                return reader.GetString();
            case JsonTokenType.None:
            case JsonTokenType.StartObject:
            case JsonTokenType.EndObject:
            case JsonTokenType.StartArray:
            case JsonTokenType.EndArray:
            case JsonTokenType.PropertyName:
            case JsonTokenType.Comment:
            case JsonTokenType.True:
            case JsonTokenType.False:
            case JsonTokenType.Null:
            default:
            {
                using JsonDocument document = JsonDocument.ParseValue(ref reader);
                return document.RootElement.Clone().ToString();
            }
        }
    }

    public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}

public class TextJsonUtils : IJsonUtils
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public TextJsonUtils()
    {
        _jsonSerializerOptions = new JsonSerializerOptions()
        {
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        };
        _jsonSerializerOptions.Converters.Add(new CustomDateTimeConverter());
        _jsonSerializerOptions.Converters.Add(new AutoNumberToStringConverter());
    }

    string IJsonUtils.ToJson<T>(T? obj) where T : default
    {
        return JsonSerializer.Serialize(obj, _jsonSerializerOptions);
    }

    T? IJsonUtils.ToObj<T>(string obj) where T : default
    {
        if (typeof(T) == typeof(string) && obj is T value)
        {
            // 规避 string 转换未 string 的情况
            return value;
        }

        return JsonSerializer.Deserialize<T>(obj, _jsonSerializerOptions);
    }
}
