﻿using Baked.Architecture;
using Baked.CodeGeneration;
using Baked.Domain.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

using static Baked.Runtime.RuntimeLayer;

namespace Baked.Runtime;

public class RuntimeLayer : LayerBase<BuildConfiguration, AddServices, PostBuild>
{
    readonly IServiceCollection _services = new ServiceCollection();
    readonly ILoggingBuilder _loggingBuilder;
    readonly IFileProviderCollection _fileProviders = new FileProviderCollection();

    public RuntimeLayer()
    {
        _loggingBuilder = new LoggingBuilder(_services);
    }

    protected override PhaseContext GetContext(BuildConfiguration phase) =>
        phase.CreateContext<IConfigurationBuilder>(Context.GetConfigurationManager());

    protected override PhaseContext GetContext(AddServices phase)
    {
        var services = Context.GetServiceCollection();
        services.AddLogging();
        services.AddSingleton<ServiceProviderAccessor>();

        return phase.CreateContextBuilder()
            .Add(_services)
            .Add(_loggingBuilder)
            .Add(_fileProviders)
            .OnDispose(() =>
            {
                foreach (var (key, provider) in _fileProviders)
                {
                    services.AddKeyedSingleton(key, provider);
                    services.AddKeyedSingleton(FromFileProviderCollectionAttribute.FILE_PROVIDERS_KEY, provider);
                    services.AddSingleton(provider);
                }

                services.AddSingleton<IFileProvider>(sp =>
                    new CompositeFileProvider(sp.UsingCurrentScope().GetKeyedServices<IFileProvider>(FromFileProviderCollectionAttribute.FILE_PROVIDERS_KEY))
                );
            })
            .Build()
        ;
    }

    protected override PhaseContext GetContext(PostBuild phase) =>
        phase.CreateContext(Context.GetServiceProvider());

    protected override IEnumerable<IPhase> GetPhases()
    {
        yield return new BuildConfiguration();
        yield return new AddServices(_services);
        yield return new PostBuild();
    }

    public class BuildConfiguration()
        : PhaseBase<ConfigurationManager>(PhaseOrder.Earliest)
    {
        protected override void Initialize(ConfigurationManager configurationManager)
        {
            Settings.SetConfigurationRoot(configurationManager);
        }
    }

    public class AddServices(IServiceCollection _services)
        : PhaseBase<DomainModel, GeneratedAssemblyProvider>(PhaseOrder.Early)
    {
        protected override void Initialize(DomainModel _, GeneratedAssemblyProvider __)
        {
            Context.Add(_services);
        }
    }

    public class PostBuild
        : PhaseBase<IServiceProvider>
    {
        protected override void Initialize(IServiceProvider _) { }
    }
}