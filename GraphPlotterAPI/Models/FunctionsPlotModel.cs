using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json.Serialization;

using Microsoft.Extensions.Primitives;

namespace GraphPlotterAPI.Models;

/// <summary>Параметры построения графиков функций</summary>
public class FunctionsPlotModel
{
    /// <summary>Заголовок графика</summary>
    [DefaultValue(null)]
    public string? Title { get; set; }

    /// <summary>Интервал по оси ОХ</summary>
    [DefaultValue(typeof(ValueInterval), "-10;10")]
    public ValueInterval ArgInterval { get; set; } = new();

    /// <summary>Интервал по оси ОУ</summary>
    [DefaultValue(typeof(ValueInterval), null)]
    public ValueInterval? ValueInterval { get; set; }

    /// <summary>Формат результата</summary>
    [DefaultValue(ImageFormat.png)]
    [EnumDataType(typeof(ImageFormat))]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ImageFormat Format { get; set; } = ImageFormat.png;

    /// <summary>Количество точек разбиения</summary>
    [DefaultValue(100)]
    public int PointsCount { get; set; } = 100;

    /// <summary>Ширина картинки</summary>
    [DefaultValue(800)]
    public double Width { get; set; } = 800;

    /// <summary>Высота картинки</summary>
    [DefaultValue(600)]
    public double Height { get; set; } = 600;

    /// <summary>Разрешение</summary>
    [DefaultValue(96)]
    public double Resolution { get; set; } = 96;

    /// <summary>Список функций, выводимых на график</summary>
    [Required]
    public ICollection<FunctionModel> Functions { get; set; } = new List<FunctionModel>();
}

/// <summary>Информация о функции</summary>
public class FunctionModel
{
    /// <summary>Название</summary>
    [DefaultValue(null)]
    public string? Name { get; set; }

    /// <summary>Текст функции</summary>
    [Required]
    public string Function { get; set; } = "x^2";

    /// <summary>Название аргумента</summary>
    [DefaultValue("x")]
    public string? ArgumentName { get; set; }

    /// <summary>Число точек</summary>
    [DefaultValue(null)]
    public int? PointsCount { get; set; }

    /// <summary>Цвет</summary>
    [DefaultValue(null)]
    public string? Color { get; set; }
}

/// <summary>Формат изображения</summary>
public enum ImageFormat
{
    /// <summary>PNG</summary>
    png,
    /// <summary>JPEG</summary>
    jpg,
    /// <summary>BMP</summary>
    bmp,
    /// <summary>SVG</summary>
    svg,
    /// <summary>GIF</summary>
    gif,
    /// <summary>TIFF</summary>
    tiff
}

/// <summary>Интервал значений</summary>
/// <param name="Min">Минимум</param>
/// <param name="Max">Максимум</param>
[TypeConverter(typeof(Converter))]
public readonly record struct ValueInterval(double Min = -10, double Max = 10)
{
    public IEnumerable<Sample> Samplete(Func<double, double> Func, int Count)
    {
        var min = Min;
        var dx = (Max - min) / (Count + 1);
        for (var i = 0; i < Count; i++)
        {
            var x = i * dx + min;
            yield return new(x, Func(x));
        }
    }

    private static readonly char[] __Separator = { ';' };
    public static ValueInterval Parse(string str)
    {
        var s = (StringSegment)str;
        var values = s.Split(__Separator);
        var i = -1;
        var min = double.NaN;
        var max = double.NaN;
        foreach (var value in values)
            if (++i == 0)
            {
                min = double.Parse(value);
            }
            else if (i == 1)
            {
                max = double.Parse(value);
            }
            else 
                break;

        return new(min, max);
    }

    public static implicit operator ValueInterval(string str) => Parse(str);

    public class Converter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type source)
        {
            if (source == typeof(string)) return true;
            return base.CanConvertFrom(context, source);
        }

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            switch (value)
            {
                case string str:
                    return Parse(str);
                default:
                    return base.ConvertFrom(context, culture, value);
            } 
        }
    }
}

/// <summary>Значение функции</summary>
/// <param name="X">Координата по оси ОХ</param>
/// <param name="Y">Координата по оси ОУ</param>
public readonly record struct Sample(double X, double Y);