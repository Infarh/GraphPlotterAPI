namespace GraphPlotterAPI.Infrastructure.Extensions;

public static class WebApplicationEx
{
    public static WebApplication OnDevelopment(this WebApplication app, Action<WebApplication> setup)
    {
        if (app.Environment.IsDevelopment())
            setup(app);
        return app;
    }
}