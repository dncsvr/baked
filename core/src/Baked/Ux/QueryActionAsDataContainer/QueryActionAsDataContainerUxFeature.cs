using Baked.Architecture;
using Baked.Business;
using Baked.Ui;
using Humanizer;

using static Baked.Theme.Default.DomainComponents;
using static Baked.Ui.Actions;
using static Baked.Ui.Datas;

using B = Baked.Ui.Components;

namespace Baked.Ux.QueryActionAsDataContainer;

public class QueryActionAsDataContainerUxFeature : IFeature<UxConfigurator>
{
    static readonly string _lengthContextKey = "length-context-key";
    static readonly string _takeContextKey = "take-context-key";

    public void Configure(LayerConfigurator configurator)
    {
        configurator.Domain.ConfigureDomainModelBuilder(builder =>
        {
            builder.Conventions.AddMethodComponent(
                when: c => c.Method.Has<QueryMethodAttribute>(),
                where: cc => cc.Path.EndsWith("Contents", "*", "*", nameof(Content.Component)),
                component: (c, cc) => MethodDataContainer(c.Method, cc)
            );
            builder.Conventions.AddMethodComponentConfiguration<DataContainer>(
                component: (dc, c, cc) =>
                {
                    foreach (var parameter in c.Method.DefaultOverload.Parameters)
                    {
                        var input = parameter.GenerateRequiredSchema<Input>(cc.Drill(nameof(DataContainer), nameof(DataContainer.Inputs)));
                        input.QueryBound = true;

                        dc.Schema.Inputs.Add(input);
                    }
                }
            );
            builder.Conventions.AddMethodComponentConfiguration<DataContainer>(
                when: c => c.Method.DefaultOverload.Parameters.Having<PagingAttribute>().Any(p => p.Get<PagingAttribute>().IsTake),
                component: (dc, c, cc) =>
                {
                    var skipParameter = c.Method.DefaultOverload.Parameters.Having<PagingAttribute>().FirstOrDefault(p => p.Get<PagingAttribute>().IsSkip);
                    if (skipParameter is null)
                    {
                        return;
                    }

                    var skipInput = dc.Schema.Inputs.First(i => i.Name == skipParameter.Name);
                    skipInput.Component.Data += Context.Page(o =>
                    {
                        o.Prop = _takeContextKey;
                        o.TargetProp = "take";
                    });
                    skipInput.Component.ReloadWhen(_takeContextKey);
                },
                order: 20
            );

            builder.Conventions.AddMethodComponentConfiguration<DataTable>(
                where: cc => cc.Path.Contains(nameof(DataContainer)),
                component: datatable =>
                {
                    datatable.Schema.Paginator = false;
                    datatable.Schema.VirtualScrollerOptions = default;
                    datatable.Schema.DataLengthContextKey = _lengthContextKey;
                }
            );

            // Skip
            builder.Conventions.AddParameterSchemaConfiguration<Input>(
                when: c => c.Parameter.TryGet<PagingAttribute>(out var paging) && paging.IsSkip,
                schema: input =>
                {
                    input.Required = true;
                    input.Numeric = true;
                },
                order: 10
            );

            builder.Conventions.AddParameterComponent(
                when: c => c.Parameter.TryGet<PagingAttribute>(out var paging) && paging.IsSkip,
                component: () => B.Paginator()
            );
            builder.Conventions.AddParameterComponentConfiguration<Paginator>(
                when: c => c.Parameter.TryGet<PagingAttribute>(out var paging) && paging.IsSkip,
                component: paginator =>
                {
                    paginator.Data = Context.Page(o =>
                    {
                        o.Prop = _lengthContextKey;
                        o.TargetProp = "length";
                    });
                    paginator.Data += Inline(new { take = 10 });

                    paginator.ReloadWhen(_lengthContextKey);
                }
            );

            //Take
            builder.Conventions.AddParameterSchemaConfiguration<Input>(
                when: c => c.Parameter.TryGet<PagingAttribute>(out var paging) && paging.IsTake,
                schema: input =>
                {
                    input.Required = true;
                    input.Numeric = true;
                },
                order: 10
            );

            builder.Conventions.AddParameterComponent(
                when: c => c.Parameter.TryGet<PagingAttribute>(out var paging) && paging.IsTake,
                component: (c, cc) =>
                {
                    cc = cc.Drill(nameof(Select));
                    var (_, l) = cc;

                    return B.Select(l(c.Parameter.Name.Titleize()), Inline(new[] { 10, 20, 50, 100 }, options: i => i.RequireLocalization = false));
                }
            );
            builder.Conventions.AddParameterComponentConfiguration<Select>(
                when: c => c.Parameter.TryGet<PagingAttribute>(out var paging) && paging.IsTake,
                component: s => s.Override(B.PageSize())
            );
            builder.Conventions.AddParameterComponentConfiguration<Select>(
                when: c => c.Parameter.TryGet<PagingAttribute>(out var paging) && paging.IsTake,
                component: s =>
                {
                    s.Schema.ShowClear = null;
                    s.Schema.Stateful = true;
                    s.Schema.NoFloatLabel = true;
                    s.Action = Publish.PageContextValue(_takeContextKey, o => o.Data = Context.Model());
                },
                order: 20
            );
        });
    }
}