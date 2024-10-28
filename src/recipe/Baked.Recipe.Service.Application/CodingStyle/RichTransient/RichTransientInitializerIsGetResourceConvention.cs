﻿using Baked.Business;
using Baked.RestApi.Configuration;
using Humanizer;

namespace Baked.CodingStyle.RichTransient;

public class RichTransientInitializerIsGetResourceConvention : IApiModelConvention<ActionModelContext>
{
    public void Apply(ActionModelContext context)
    {
        if (!context.Controller.MappedType.TryGetMembers(out var members)) { return; }
        if (!members.Has<LocatableAttribute>()) { return; }
        if (!members.Properties.Any(p => p.IsPublic)) { return; }
        if (context.Action.MappedMethod is null) { return; }
        if (!context.Action.MappedMethod.Has<InitializerAttribute>()) { return; }
        if (!context.Action.Parameter.TryGetValue("id", out var parameter)) { return; }

        parameter.IsInvokeMethodParameter = true;
        parameter.From = RestApi.Model.ParameterModelFrom.Route;
        parameter.RoutePosition = 1;

        var targetParameter = context.Action.Parameter["target"];
        targetParameter.Name = "newTarget";
        targetParameter.Type = $"Func<{targetParameter.Type}>";

        context.Action.Method = HttpMethod.Get;
        context.Action.RouteParts = [context.Controller.MappedType.Name.Pluralize()];
        context.Action.FindTargetStatement = "newTarget()";
    }
}