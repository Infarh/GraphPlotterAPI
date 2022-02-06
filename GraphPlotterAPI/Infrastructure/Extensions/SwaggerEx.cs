using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GraphPlotterAPI.Infrastructure.Extensions;

internal static class SwaggerEx
{
    public static SwaggerGenOptions AddXmlDoc(this SwaggerGenOptions opt, Type type) => opt.AddXmlDoc(type.Assembly);

    public static SwaggerGenOptions AddXmlDoc(this SwaggerGenOptions opt, Assembly asm)
    {
        //var file_name = $"{asm.GetName().Name}.xml";
        //var path = Path.Combine(AppContext.BaseDirectory, file_name);
        var asm_path = asm.Location;
        var path = Path.ChangeExtension(asm_path, ".xml");
        if (File.Exists(path))
            opt.IncludeXmlComments(path);
        return opt;
    }
}