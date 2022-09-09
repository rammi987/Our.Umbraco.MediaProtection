using Microsoft.Extensions.Options;
using Our.Umbraco.MediaProtection.Service;
using SixLabors.ImageSharp.Web.Middleware;
using System.Threading.Tasks;
using System.Web;

namespace Our.Umbraco.MediaProtection.Middleware;

public sealed class ConfigureImageSharpProtectionMiddlewareOptions : IConfigureOptions<ImageSharpMiddlewareOptions>
{
    private readonly MediaUtillityService _mediaUtillityService;

    public ConfigureImageSharpProtectionMiddlewareOptions(MediaUtillityService mediaUtillityService)
        => _mediaUtillityService = mediaUtillityService;

    public void Configure(ImageSharpMiddlewareOptions options)
    {
        options.HMACSecretKey = _mediaUtillityService.GetHMACSecretKey();

        options.OnComputeHMACAsync = (context, secret) =>
        {
            var query = HttpUtility.ParseQueryString(context.Context.Request.QueryString.ToString());
            query.Remove("hmac");

            return Task.FromResult(_mediaUtillityService.GetHmac(context.Context.Request.Path + "?" + query));
        };
    }
}
