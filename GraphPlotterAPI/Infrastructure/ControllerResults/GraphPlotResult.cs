using System.ComponentModel;
using System.Text;
using GraphPlotterAPI.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using OxyPlot;
using OxyPlot.ImageSharp;
using ImageFormat = GraphPlotterAPI.Models.ImageFormat;

namespace GraphPlotterAPI.Infrastructure.ControllerResults;

public class GraphPlotResult : FileResult
{
    private readonly PlotModel _Model;
    private readonly int _Width;
    private readonly int _Height;
    private readonly double _Resolution;
    private readonly ImageFormat _Format;

    public GraphPlotResult(
        PlotModel Model, 
        int Width, 
        int Height, 
        double Resolution, 
        ImageFormat Format)
        : base(Format.ToMIME())
    {
        _Model = Model;
        _Width = Width;
        _Height = Height;
        _Resolution = Resolution;
        _Format = Format;

        FileDownloadName = new StringBuilder()
           .Append(Model.Title is { Length: > 0 } title ? title : "file")
           .Append(".png")
           .ToString();
    }

    /// <inheritdoc />
    public override Task ExecuteResultAsync(ActionContext context)
    {
        if (context is null)
            throw new ArgumentNullException(nameof(context));

        var response = context.HttpContext.Response;
        response.ContentType = "image/png";
        var headers = response.Headers;
        headers.ContentDisposition = $"attachment; filename={FileDownloadName}";
        //headers.Add("Content-Disposition", new[] { "attachment; filename=" + FileDownloadName });

        IExporter encoder = _Format switch
        {
            ImageFormat.png => new PngExporter(_Width, _Height, _Resolution),
            ImageFormat.svg => new SvgExporter { Width = _Width, Height = _Height },
            ImageFormat.jpg => new JpegExporter(_Width, _Height, _Resolution),
            _ => throw new InvalidEnumArgumentException(nameof(_Format), (int)_Format, typeof(ImageFormat))
        };
        
        encoder.Export(_Model, response.BodyWriter.AsStream(true));

        return Task.CompletedTask;
    }
}
