using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using BilibiliClient.Utils;

namespace BilibiliClient.Converters;

public class Source2QRCodeConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var content = value?.ToString();
        return !string.IsNullOrWhiteSpace(content) ? QRCoderUtil.GetQRCode(content) : BindingOperations.DoNothing;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return BindingOperations.DoNothing;
    }
}