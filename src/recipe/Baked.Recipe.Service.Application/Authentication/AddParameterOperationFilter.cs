﻿using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Baked.Authentication;

public class AddParameterOperationFilter<T>(string _name, ParameterLocation _in, bool _required, string _documentName)
    : IOperationFilter where T : Attribute
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (!string.IsNullOrWhiteSpace(_documentName) && context.DocumentName != _documentName) { return; }
        if (!context.MethodInfo.CustomAttributes.Any(a => a.AttributeType == typeof(T))) { return; }

        operation.Parameters.Add(new()
        {
            Required = _required,
            In = _in,
            Name = _name
        });
    }
}