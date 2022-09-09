using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Our.Umbraco.MediaProtection.Middleware;
using Our.Umbraco.MediaProtection.Service;
using SixLabors.ImageSharp.Web.Middleware;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Web.Common.ApplicationBuilder;

namespace Our.Umbraco.MediaProtection.Composer;
public class MediaProcetionServiceBuilder : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.Services.AddTransient<IConfigureOptions<ImageSharpMiddlewareOptions>, ConfigureImageSharpProtectionMiddlewareOptions>();
        builder.Services.AddTransient<MediaUtillityService>();

        builder.Services.Configure<Configuration.MediaProtection>(builder.Config.GetSection(nameof(MediaProtection)));

        builder.Services.Configure<UmbracoPipelineOptions>(options =>
        {
            options.AddFilter(new UmbracoPipelineFilter(
                "MediaProtection",
                app =>
                {
                    app.UseWhen(x => x.Request.Path.StartsWithSegments("/media"), (app) =>
                    {
                        app.UseMiddleware<MediaProtectionMiddleware>();
                    });
                },
                app => { },
                app => { }
            ));
        });
    }
}
