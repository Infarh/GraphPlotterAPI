using GraphPlotterAPI.Infrastructure.Extensions;
using GraphPlotterAPI.Services;
using GraphPlotterAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;

/* ========================================================================== */

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("Identity"));

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(opt =>
{
    opt.AddXmlDoc(typeof(Program));
    opt.UseInlineDefinitionsForEnums();

    opt.UseAllOfToExtendReferenceSchemas();
    opt.IncludeXmlCommentsFromInheritDocs(includeRemarks: true, excludedTypes: typeof(string));

    opt.AddEnumsWithValuesFixFilters();
});

services.Configure<KestrelServerOptions>(opt =>
{
    //opt.AllowSynchronousIO = true;
});

services.AddScoped<IPlotterService, PlotterService>();

/* ========================================================================== */

var app = builder.Build();

app.OnDevelopment(a => a
   .UseSwagger()
   .UseSwaggerUI()
);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

/* ========================================================================== */

app.Run();
