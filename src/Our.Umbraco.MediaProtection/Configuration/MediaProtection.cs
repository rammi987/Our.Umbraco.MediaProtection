using Our.Umbraco.MediaProtection.Configuration.Models;
using System;
using System.ComponentModel;

namespace Our.Umbraco.MediaProtection.Configuration;

public class MediaProtection
{
    internal const string StaticHashMode = "Hmac256";

    public string Secret { get; set; } = Guid.NewGuid().ToString();
    
    [DefaultValue(StaticHashMode)]
    public HmacHashMode HmacHashMode { get; set; } = global::Umbraco.Cms.Core.Enum<HmacHashMode>.Parse(StaticHashMode);
}
