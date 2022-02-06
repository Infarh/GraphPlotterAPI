using Microsoft.AspNetCore.Mvc;

namespace GraphPlotterAPI.Infrastructure.Extensions;

internal static class ControllerEx
{
    public static IActionResult Return<T>(
        this T Controller,
        Func<T, IActionResult> Selector
        ) 
        where T : ControllerBase
    {
        return Selector(Controller);
    }
}