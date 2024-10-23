﻿using Baked.Cors;
using Baked.Cors.AllowOrigin;
using Baked.Runtime;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Baked;

public static class AspNetCoreCorsExtensions
{
    /// <summary>
    /// Returns 'AspNetCoreCors' feature with a single policy setup with given origins,
    /// any header and any method
    /// </summary>
    /// <param name="configurator"></param>
    /// CorsConfigurator
    /// <param name="origins">
    /// Allowed origin addresses
    /// </param>
    /// <returns></returns>
    public static AspNetCoreCorsFeature AspNetCore(this CorsConfigurator configurator, params Setting<string>[] origins) =>
        configurator.AspNetCore(
            options => options
                .AddPolicy("allow-origin", policy => policy
                    .WithOrigins(origins.Select(o => o.GetValue()).ToArray())
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                ),
            "allow-origin"
        );

    public static AspNetCoreCorsFeature AspNetCore(this CorsConfigurator _, Action<CorsOptions> optionsBuilder, string defaultPolicyName) =>
        new(optionsBuilder, defaultPolicyName);
}