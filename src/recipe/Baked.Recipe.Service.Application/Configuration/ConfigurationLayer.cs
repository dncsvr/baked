﻿using Baked.Architecture;
using Microsoft.Extensions.Configuration;

using static Baked.Configuration.ConfigurationLayer;

namespace Baked.Configuration;

public class ConfigurationLayer : LayerBase<BuildConfiguration>
{
    protected override PhaseContext GetContext(BuildConfiguration phase) =>
        phase.CreateContext<IConfigurationBuilder>(Context.GetConfigurationManager());

    protected override IEnumerable<IPhase> GetPhases()
    {
        yield return new BuildConfiguration();
    }

    public class BuildConfiguration()
        : PhaseBase<ConfigurationManager>(PhaseOrder.Earliest)
    {
        protected override void Initialize(ConfigurationManager configurationManager)
        {
            Settings.SetConfigurationRoot(configurationManager);
        }
    }
}