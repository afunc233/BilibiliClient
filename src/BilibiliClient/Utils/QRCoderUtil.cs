using System.Collections;
using System.Collections.Generic;
using System.IO;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Skia;
using QRCoder;
using SkiaSharp;

namespace BilibiliClient.Utils;


internal static class QRCoderUtil
{
    // ReSharper disable once InconsistentNaming
    private static readonly QRCodeGenerator _qrCodeGenerator;

    static QRCoderUtil()
    {
        _qrCodeGenerator = new QRCodeGenerator();
    }

    public static IImage GetQRCode(string content, int pixelsPerModule = 8, Bitmap? centerBitmap = null,
        bool drawQuietZones = false)
    {
        var qrCodeData = _qrCodeGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.M);
        return GetGraphic(qrCodeData.ModuleMatrix, pixelsPerModule, centerBitmap, drawQuietZones);
    }

    private static Bitmap GetGraphic(IReadOnlyList<BitArray> moduleMatrix, int pixelsPerModule = 8,
        Bitmap? centerBitmap = null,
        bool drawQuietZones = false)
    {
        var size = (moduleMatrix.Count - (drawQuietZones ? 0 : 8)) * pixelsPerModule;
        var offset = drawQuietZones ? 0 : 4 * pixelsPerModule;

        using var lightPaint = new SKPaint();
        lightPaint.Color = new SKColor(255, 255, 255);
        using var darkPaint = new SKPaint();
        darkPaint.Color = new SKColor(0, 0, 0);
        using var renderTarget =
            new WriteableBitmap(new PixelSize(size, size), new Vector(96, 96), PixelFormat.Rgba8888);
        using var lockedBitmap = renderTarget.Lock();

        var info = new SKImageInfo(lockedBitmap.Size.Width, lockedBitmap.Size.Height,
            lockedBitmap.Format.ToSkColorType());
        using var currentSurface = SKSurface.Create(info, lockedBitmap.Address, lockedBitmap.RowBytes);
        var canvas = currentSurface.Canvas;
        canvas.Clear(new SKColor(255, 255, 255));
        for (var x = 0; x < size + offset; x += pixelsPerModule)
        {
            for (var y = 0; y < size + offset; y += pixelsPerModule)
            {
                var module =
                    moduleMatrix[(y + pixelsPerModule) / pixelsPerModule - 1][
                        (x + pixelsPerModule) / pixelsPerModule - 1];

                canvas.DrawRect(x - offset, y - offset, pixelsPerModule, pixelsPerModule,
                    module ? darkPaint : lightPaint);
            }
        }

        if (centerBitmap != null)
        {
            using var icon = new MemoryStream();
            centerBitmap.Save(icon);
            icon.Seek(0, SeekOrigin.Begin);
            using var data = SKData.Create(icon);
            using var img = SKImage.FromEncodedData(data);
            var position = (int)(size - centerBitmap.Size.Width) / 2;
            canvas.DrawImage(img, new SKPoint(position, position));
        }

        var stream = SKImage.FromPixels(info, lockedBitmap.Address, lockedBitmap.RowBytes)
            .Encode(SKEncodedImageFormat.Png, 100)
            .AsStream();

        var qrCode = new Bitmap(stream);
        stream.Close();
        stream.Dispose();
        return qrCode;
    }
}