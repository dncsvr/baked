﻿using Baked.Domain.Model;
using Baked.RestApi.Configuration;
using Humanizer;

namespace Baked.CodingStyle.EntitySubclassViaComposition;

public class EntitySubclassUnderEntitiesConvention(DomainModel _domain)
    : IApiModelConvention<ControllerModelContext>
{
    public void Apply(ControllerModelContext context)
    {
        if (!context.Controller.MappedType.TryGetEntityTypeFromSubclass(_domain, out var entityType)) { return; }

        context.Controller.GroupName = entityType.Name.Pluralize();
    }
}