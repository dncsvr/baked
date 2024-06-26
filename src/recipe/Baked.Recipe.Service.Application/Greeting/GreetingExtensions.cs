﻿using Baked.Architecture;
using Baked.Greeting;

namespace Baked;

public static class GreetingExtensions
{
    public static void AddGreeting(this List<IFeature> source, Func<GreetingConfigurator, IFeature<GreetingConfigurator>> configure) =>
        source.Add(configure(new()));
}