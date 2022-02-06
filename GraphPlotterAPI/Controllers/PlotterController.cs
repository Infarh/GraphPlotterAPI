using System.Data.Common;
using GraphPlotterAPI.Infrastructure.ControllerResults;
using GraphPlotterAPI.Infrastructure.Extensions;
using GraphPlotterAPI.Models;
using GraphPlotterAPI.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;
using ControllerBase = Microsoft.AspNetCore.Mvc.ControllerBase;
using ImageFormat = GraphPlotterAPI.Models.ImageFormat;

namespace GraphPlotterAPI.Controllers;

[ApiController, Route("api/[controller]")]
public class PlotterController : ControllerBase
{
    private readonly IPlotterService _Plotter;

    public PlotterController(IPlotterService Plotter) => _Plotter = Plotter;

    /// <summary>Построение графика функций</summary>
    /// <param name="Model">Параметры построения</param>
    /// <response code="200">Если всё хорошо</response>
    [HttpGet]
    [Produces("image/png")]
    public IActionResult Functions(FunctionsPlotModel Model)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var plot = _Plotter.Plot(Model);

        return new GraphPlotResult(
            plot,
            (int)Model.Width, 
            (int)Model.Height, 
            Model.Resolution,
            Model.Format);
    }
}