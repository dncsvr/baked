﻿using Do.Architecture;
using Do.Core;
using Do.Greeting;
using Do.Logging;

namespace Do;

public static class ForgeExtensions
{
    public static Application Service(this Forge source,
        Func<CoreConfigurator, IFeature>? core = default,
        Func<GreetingConfigurator, IFeature>? greeting = default,
        Func<LoggingConfigurator, IFeature>? logging = default,
        Action<ApplicationDescriptor>? configure = default
    )
    {
        core ??= c => c.Dotnet();
        greeting ??= c => c.Swagger();
        logging ??= c => c.RequestLogging();
        configure ??= _ => { };

        return source.Application(app =>
            {
                app.Layers.AddConfiguration();
                app.Layers.AddDataAccess();
                app.Layers.AddDependencyInjection();
                app.Layers.AddHttpServer();
                app.Layers.AddMonitoring();
                app.Layers.AddRestApi();

                app.Features.AddCore(core);
                app.Features.AddGreeting(greeting);
                app.Features.AddLogging(logging);

                configure(app);
            });
    }
}
