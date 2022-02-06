using System.Reflection;

using GraphPlotterAPI.Models;
using GraphPlotterAPI.Services.Interfaces;
using MathCore.MathParser;

using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace GraphPlotterAPI.Services;

public class PlotterService : IPlotterService
{
    public static readonly Dictionary<string, OxyColor> __NamedColors = 
        typeof(OxyColors)
           .GetFields(BindingFlags.Static | BindingFlags.Public)
           .ToDictionary(
                color => color.Name, 
                field => (OxyColor)field.GetValue(null)!, 
                StringComparer.OrdinalIgnoreCase);

    public PlotModel Plot(FunctionsPlotModel Model)
    {
        var model = new PlotModel
        {
            Title = Model.Title,
            Background = OxyColors.White,
            Axes =
            {
                new LinearAxis
                {
                    Position = AxisPosition.Bottom,
                    Title = "X",
                    MinorGridlineColor = OxyColors.LightGray,
                    MinorGridlineStyle = LineStyle.Dot,
                    MajorGridlineColor = OxyColors.Gray,
                    MajorGridlineStyle = LineStyle.Dash,
                },
                new LinearAxis
                {
                    Position = AxisPosition.Left,
                    Title = "Y",
                    MinorGridlineColor = OxyColors.LightGray,
                    MinorGridlineStyle = LineStyle.Dot,
                    MajorGridlineColor = OxyColors.Gray,
                    MajorGridlineStyle = LineStyle.Dash,
                },
            }
        };

        var interval = Model.ArgInterval;

        var default_points_count = Model.PointsCount;

        var parser = new ExpressionParser();

        foreach (var function_model in Model.Functions)
        {
            var function_string = function_model.Function;
            var function_expression = parser.Parse(function_string);

            var arg_name = function_model.ArgumentName ?? "x";
            var func = (Func<double, double>)function_expression.Compile(arg_name);

            var points_count = function_model.PointsCount ?? default_points_count;
            var samples = interval.Samplete(func, points_count);

            var series = new LineSeries
            {
                DataFieldX = nameof(Sample.X),
                DataFieldY = nameof(Sample.Y),
                ItemsSource = samples
            };

            if (function_model.Name is { Length: > 0 } name)
                series.Title = name;

            if (function_model.Color is { Length: > 0 } color)
                series.Color = typeof(OxyColors).GetField(color, BindingFlags.Static | BindingFlags.Public) is { } color_field
                    ? (OxyColor)color_field.GetValue(null)!
                    : OxyColor.Parse(color);

            model.Series.Add(series);
        }

        return model;
    }
}