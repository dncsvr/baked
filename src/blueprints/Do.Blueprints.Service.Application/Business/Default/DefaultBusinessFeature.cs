﻿using Do.Architecture;
using Do.Domain.Model;
using Do.RestApi.Model;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace Do.Business.Default;

public class DefaultBusinessFeature(List<Assembly> _domainAssemblies)
    : IFeature<BusinessConfigurator>
{
    const BindingFlags _defaultMemberBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;

    public void Configure(LayerConfigurator configurator)
    {
        configurator.ConfigureDomainAssemblyCollection(assemblies =>
        {
            foreach (var assembly in _domainAssemblies)
            {
                assemblies.Add(assembly);
            }
        });

        configurator.ConfigureDomainBuilderOptions(options =>
        {
            options.ConstuctorBindingFlags = _defaultMemberBindingFlags;
            options.MethodBindingFlags = _defaultMemberBindingFlags;
            options.PropertyBindingFlags = _defaultMemberBindingFlags;
        });

        configurator.ConfigureDomainMetaData(metadata =>
        {
            metadata.Type.Add(
                add: new DataClassAttribute(),
                when: type => type.Methods.Contains("<Clone>$"), // if type is record
                order: int.MinValue
            );
        });

        configurator.ConfigureServiceCollection(services =>
        {
            var domainModel = configurator.Context.GetDomainModel();
            foreach (var type in domainModel.Types.Where(t => !t.IsIgnored()))
            {
                if (type.IsTransient())
                {
                    type.Apply(t =>
                    {
                        services.AddTransientWithFactory(t);
                        type.Interfaces
                            .Where(i => i.IsBusinessType)
                            .Apply(i => services.AddTransientWithFactory(i, t));
                    });
                }
                else if (type.IsScoped())
                {
                    type.Apply(t =>
                    {
                        services.AddScopedWithFactory(t);
                        type.Interfaces
                            .Where(i => i.IsBusinessType)
                            .Apply(i => services.AddScopedWithFactory(i, t));
                    });
                }
                else if (type.IsSingleton())
                {
                    type.Apply(t =>
                    {
                        services.AddSingleton(t);
                        type.Interfaces
                            .Where(i => i.IsBusinessType)
                            .Apply(i => services.AddSingleton(i, t, forward: true));
                    });
                }
            }
        });

        configurator.ConfigureApiModel(api =>
        {
            api.References.AddRange(_domainAssemblies);

            var domainModel = configurator.Context.GetDomainModel();
            foreach (var type in domainModel.Types.Where(t => !t.IsIgnored()))
            {
                if (type.FullName is null) { continue; }
                if (!type.IsSingleton()) { continue; } // TODO for now only singleton

                var controller = new ControllerModel(type.Name);
                foreach (var method in type.Methods.Where(m => !m.IsConstructor && m.Overloads.Count(o => o.IsPublic) > 0))
                {
                    var overload = method.Overloads.OrderByDescending(o => o.Parameters.Count).First();
                    if (overload.ReturnType is null) { continue; }

                    if (overload.Parameters.Count > 0) { continue; } // TODO for now only parameterless
                    if (overload.ReturnType.FullName != typeof(void).FullName &&
                        overload.ReturnType.FullName != typeof(Task).FullName) { continue; } // TODO for now only void

                    controller.Actions.Add(
                        new(
                            Name: method.Name,
                            Method: HttpMethod.Post,
                            Route: $"generated/{type.Name}/{method.Name}",
                            Return: new(async: overload.ReturnType.FullName == typeof(Task).FullName),
                            Statements: new(
                                FindTarget: "target",
                                InvokeMethod: new(method.Name)
                            )
                        )
                        { Parameters = [new(ParameterModelFrom.Services, type.FullName, "target")] }
                    );
                }

                api.Controllers.Add(controller);
            }
        });

        configurator.ConfigureMvcNewtonsoftJsonOptions(options =>
        {
            options.SerializerSettings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
        });
    }
}
