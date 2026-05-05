using Baked.Architecture;
using Baked.Business;
using Baked.RestApi.Model;
using Baked.Ui;

using static Baked.Theme.Default.DomainComponents;

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
                when: c => c.Type.Has<QueryAttribute>() && c.Method.Has<ActionModelAttribute>() && c.Method.DefaultOverload.ReturnsList(),
                where: cc => cc.Path.EndsWith("Contents", "*", "*", nameof(Content.Component)),
                component: (c, cc) => MethodDataContainer(c.Method, cc)
            );
            builder.Conventions.AddMethodComponentConfiguration<DataContainer>(
                component: (dp, c, cc) =>
                {
                    foreach (var parameter in c.Method.DefaultOverload.Parameters)
                    {
                        var input = parameter.GenerateRequiredSchema<Input>(cc.Drill(nameof(DataContainer), nameof(DataContainer.Inputs)));
                        input.QueryBound = true;

                        dp.Schema.Inputs.Add(input);
                    }
                }
            );
            builder.Conventions.AddMethodComponentConfiguration<DataContainer>(
                when: c => c.Method.DefaultOverload.Parameters.Any(p => p.IsTake) && c.Method.DefaultOverload.Parameters.Any(p => p.IsSkip),
                component: (dp, c, cc) =>
                {
                    var skipParameter = c.Method.DefaultOverload.Parameters.First(p => p.IsSkip);

                    var skipInput = dp.Schema.Inputs.First(i => i.Name == skipParameter.Name);
                    skipInput.Component.Data += Datas.Context.Page(o =>
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
            builder.Conventions.AddParameterComponent(
                when: c => c.Parameter.Has<PagingAttribute>() && c.Parameter.Get<PagingAttribute>().RoleOption == PagingAttribute.Role.Skip,
                component: () => B.ServerPaginator()
            );
            builder.Conventions.AddParameterComponentConfiguration<ServerPaginator>(
                when: c => c.Parameter.Has<PagingAttribute>() && c.Parameter.Get<PagingAttribute>().RoleOption == PagingAttribute.Role.Skip,
                component: paginator =>
                {
                    paginator.Data = Datas.Context.Page(o =>
                    {
                        o.Prop = _lengthContextKey;
                        o.TargetProp = "length";
                    });
                    paginator.Data += Datas.Inline(new { take = 10 });

                    paginator.ReloadWhen(_lengthContextKey);
                }
            );
            builder.Conventions.AddParameterSchemaConfiguration<Input>(
                when: c => c.Parameter.Has<PagingAttribute>() && c.Parameter.Get<PagingAttribute>().RoleOption == PagingAttribute.Role.Skip,
                schema: input =>
                {
                    input.Required = true;
                    input.Numeric = true;
                },
                order: 10
            );

            //Take
            builder.Conventions.AddParameterComponent(
                when: c => c.Parameter.Has<PagingAttribute>() && c.Parameter.Get<PagingAttribute>().RoleOption == PagingAttribute.Role.Take,
                component: (c, cc) =>
                {
                    cc = cc.Drill(nameof(Select));
                    var (_, l) = cc;

                    return B.Select(l("Take"), Datas.Inline(new[] { 10, 20, 50, 100 }, options: i => i.RequireLocalization = false));
                }
            );
            builder.Conventions.AddParameterSchemaConfiguration<Input>(
                when: c => c.Parameter.Has<PagingAttribute>() && c.Parameter.Get<PagingAttribute>().RoleOption == PagingAttribute.Role.Take,
                schema: input =>
                {
                    input.Required = true;
                    input.Numeric = true;
                },
                order: 10
            );
            builder.Conventions.AddParameterComponentConfiguration<Select>(
                when: c => c.Parameter.Has<PagingAttribute>() && c.Parameter.Get<PagingAttribute>().RoleOption == PagingAttribute.Role.Take,
                component: s =>
                {
                    s.Schema.ShowClear = false;
                    s.Schema.Stateful = true;
                    s.Schema.NoFloatLabel = true;
                    s.Action = Actions.Publish.PageContextValue(_takeContextKey, o => o.Data = Datas.Context.Model());
                },
                order: 20
            );
        });
    }
}