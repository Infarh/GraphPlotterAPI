using System.ComponentModel;
using GraphPlotterAPI.Models;

namespace GraphPlotterAPI.Infrastructure.Extensions;

internal static class ImageFormatEx
{
    public static string ToMIME(this ImageFormat Format) =>
        Format switch
        {
            ImageFormat.png => "image/png",
            ImageFormat.bmp => "image/bmp",
            ImageFormat.jpg => "image/jpeg",
            ImageFormat.gif => "image/gif",
            ImageFormat.tiff => "image/tiff",
            _ => throw new InvalidEnumArgumentException(nameof(Format), (int)Format, typeof(ImageFormat))
        };
}