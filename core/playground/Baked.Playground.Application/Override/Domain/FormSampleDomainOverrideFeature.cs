using Baked.Architecture;
using Baked.Playground.Orm;
using Baked.Playground.Theme;
using Baked.Theme.Default;
using Baked.Ui;

namespace Baked.Playground.Override.Domain;

public class FormSampleDomainOverrideFeature : IFeature
{
    public void Configure(LayerConfigurator configurator)
    {
        configurator.Domain.ConfigureDomainModelBuilder(builder =>
        {
            builder.Conventions.AddMethodAttributeConfiguration<ActionAttribute>(
                when: c => c.Type.Is<FormSample>() && c.Method.Name == nameof(FormSample.NewParent),
                attribute: (a, c) => a.RoutePathBack = "/form-sample"
            );
            builder.Conventions.AddMethodComponentConfiguration<FormPage>(
                when: c => c.Type.Is<FormSample>() && c.Method.Name == nameof(FormSample.NewParent),
                component: fp => fp.Schema.Inputs.Move("name", 0)
            );

            builder.Conventions.AddMethodAttributeConfiguration<ActionAttribute>(
                when: c => c.Type.Is<Parent>() && c.Method.Name.Contains("Child"),
                attribute: a => a.HideInLists = true
            );
            builder.Conventions.RemoveMethodComponent<DataPanel>(
                when: c => c.Type.Is<FormSample>() && c.Method.Name == nameof(FormSample.GetParents)
            );
            builder.Conventions.AddMethodComponent(
                when: c => c.Type.Is<FormSample>() && c.Method.Name == nameof(FormSample.GetParents),
                where: cc => cc.Path.EndsWith("Contents", "*", "*", nameof(Content.Component)),
                component: (c, cc) => DomainComponents.MethodListPanel(c.Method, cc)
            );
        });
    }
}