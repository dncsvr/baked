﻿using Baked.Architecture;

namespace Baked.Test.Architecture.Feature;

public class ConfiguringLayers : ArchitectureSpec
{
    public record ConfigurationA
    {
        public string? Value { get; set; }
    }

    public class FeatureA(string _value)
        : IFeature
    {
        public void Configure(LayerConfigurator configurator)
        {
            configurator.Configure((ConfigurationA configuration) => configuration.Value = _value);
        }
    }

    [Test]
    public void Features_configure_layer_using_a_layer_configurator_that_can_apply_given_action_to_its_target_object()
    {
        var configuration = new ConfigurationA();
        var configurator = GiveMe.ALayerConfigurator(target: configuration);

        var featureA = new FeatureA("test");

        featureA.Configure(configurator);

        configuration.Value.ShouldBe("test");
    }

    public class FeatureConfigurator;

    [Test]
    public void EmptyFeature_does_not_configure_layers()
    {
        var configuration = new ConfigurationA();
        var configurator = GiveMe.ALayerConfigurator(target: configuration);

        var emptyFeature = Baked.Architecture.Feature.Empty<FeatureConfigurator>();

        emptyFeature.Configure(configurator);

        configuration.Value.ShouldBeNull();
    }

    [Test]
    public void Layer_configurator_does_not_apply_given_action_when_given_type_does_not_match_current_target_type()
    {
        var configurator = GiveMe.ALayerConfigurator(target: new ConfigurationA());

        configurator.Configure((object value) => this.ShouldFail());

        this.ShouldPass("didn't configure as expected");
    }

    [Test]
    public void Layer_configurator_can_restrict_a_target_into_one_of_its_base_class_or_interfaces()
    {
        var configurator = GiveMe.ALayerConfigurator<object>(target: new ConfigurationA());

        var configured = false;
        configurator.Configure((object value) => configured = true);
        configurator.Configure((ConfigurationA value) => configured = false);

        configured.ShouldBeTrue("should've configured only for the first call");
    }

    [Test]
    public void Layer_configurator_accepts_two_parameters_in_a_given_action()
    {
        var configurator = GiveMe.ALayerConfigurator(target1: "test", target2: 10);

        var configured = false;
        configurator.Configure((string str, int i) => configured = true);
        configurator.Configure((object str, object i) => configured = false);

        configured.ShouldBeTrue("should've configured only for the first call");
    }

    [Test]
    public void Layer_configurator_accepts_three_parameters_in_a_given_action()
    {
        var configurator = GiveMe.ALayerConfigurator(target1: "test", target2: 10, target3: false);

        var configured = false;
        configurator.Configure((string str, int i, bool b) => configured = true);
        configurator.Configure((object str, object i, object b) => configured = false);

        configured.ShouldBeTrue("should've configured only for the first call");
    }
}