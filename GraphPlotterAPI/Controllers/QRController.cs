using MathCore.Monads.WorkFlow;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using QRCoder;

namespace GraphPlotterAPI.Controllers;

[ApiController, Route("api/[controller]")]
public class QRController : ControllerBase
{
    [HttpGet]
    public IActionResult Get(string Text, QRCodeGenerator.ECCLevel Level = QRCodeGenerator.ECCLevel.Q, int PixelSize = 20)
    {
        StringSegment ss = Text;
        var qq = ss.Split(new [] { ' ' });
        


        var generator = new QRCodeGenerator();
        var data = generator.CreateQrCode(Text, Level);

        var code = new BitmapByteQRCode(data);
        var bytes = code.GetGraphic(PixelSize);
        return File(bytes, "image/bmp");
    }
}