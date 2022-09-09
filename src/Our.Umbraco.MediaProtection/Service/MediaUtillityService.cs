using Microsoft.Extensions.Options;
using Our.Umbraco.MediaProtection.Configuration;
using Our.Umbraco.MediaProtection.Configuration.Models;
using SixLabors.ImageSharp.Web;
using System.Text;

namespace Our.Umbraco.MediaProtection.Service;

public class MediaUtillityService
{
    private readonly Configuration.MediaProtection _mediaProtectionConfig;

    public MediaUtillityService(IOptions<Configuration.MediaProtection> mediaProtectionConfig) 
        => _mediaProtectionConfig = mediaProtectionConfig.Value;
    
    public byte[] GetHMACSecretKey() => Encoding.ASCII.GetBytes(_mediaProtectionConfig.Secret);

    public string GetHmac(string uri)=> _mediaProtectionConfig.HmacHashMode switch
    {
        HmacHashMode.Hmac384 => HMACUtilities.ComputeHMACSHA384(uri, this.GetHMACSecretKey()),
        HmacHashMode.Hmac512 => HMACUtilities.ComputeHMACSHA512(uri, this.GetHMACSecretKey()),
        _ => HMACUtilities.ComputeHMACSHA256(uri, this.GetHMACSecretKey()),
    };
}
