using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Our.Umbraco.MediaProtection.Service;
using SixLabors.ImageSharp.Web.Middleware;
using System.Security.Claims;
using System.Threading.Tasks;
using Umbraco.Extensions;

namespace Our.Umbraco.MediaProtection.Middleware;

public class MediaProtectionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ImageSharpMiddlewareOptions _imageSharpMiddlewareOptions;
    private readonly MediaUtillityService _mediaUtillityService;

    public MediaProtectionMiddleware(RequestDelegate next, IOptions<ImageSharpMiddlewareOptions> imageSharpMiddlewareOptions, MediaUtillityService mediaUtillityService)
    {
        _next = next;
        _imageSharpMiddlewareOptions = imageSharpMiddlewareOptions.Value;
        _mediaUtillityService = mediaUtillityService;
    }

    public async Task Invoke(HttpContext context)
    {
        // set it at first
        _imageSharpMiddlewareOptions.HMACSecretKey = _mediaUtillityService.GetHMACSecretKey();

        // This code is from Umbraco.Core Preview Auth
        CookieAuthenticationOptions? cookieOptions = context.RequestServices
            .GetRequiredService<IOptionsSnapshot<CookieAuthenticationOptions>>()
            .Get(global::Umbraco.Cms.Core.Constants.Security.BackOfficeAuthenticationType);

        if (cookieOptions != null && cookieOptions.Cookie.Name != null && context.Request.Cookies.TryGetValue(cookieOptions.Cookie.Name, out var cookie))
        {
            AuthenticationTicket? unprotected = cookieOptions.TicketDataFormat.Unprotect(cookie);
            ClaimsIdentity? backOfficeIdentity = unprotected?.Principal.GetUmbracoIdentity();

            if (backOfficeIdentity != null)
            {
                _imageSharpMiddlewareOptions.HMACSecretKey = null;
            }
        }

        await _next.Invoke(context);
    }
}
