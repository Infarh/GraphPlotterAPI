using Microsoft.AspNetCore.Mvc;

namespace GraphPlotterAPI.Controllers;

[ApiController, Route("api/[controller]")]
public class ConsoleController : ControllerBase
{
    [HttpGet("clear")] public void Clear() => Console.Clear();

    [HttpGet("write")] public void Write(string Msg) => Console.WriteLine(Msg);
}