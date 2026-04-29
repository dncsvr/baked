using Baked.Architecture;
using Baked.Business;
using Baked.RestApi.Model;
using Baked.Ui;
using Humanizer;

using static Baked.Theme.Default.DomainComponents;
using static Baked.Theme.Default.DomainDatas;

using B = Baked.Ui.Components;

namespace Baked.Ux.QueryActionAsListPanel;

public class QueryActionAsListPanelUxFeature : IFeature<UxConfigurator>
{
    public void Configure(LayerConfigurator configurator)
    {
        configurator.Domain.ConfigureDomainModelBuilder(builder =>
        {
            builder.Conventions.AddMethodComponent(
                when: c => c.Type.Has<QueryAttribute>() && c.Method.Has<ActionModelAttribute>() && c.Method.DefaultOverload.ReturnsList(),
                where: cc => cc.Path.EndsWith("Contents", "*", "*", nameof(Content.Component)),
                component: (c, cc) => MethodListPanel(c.Method, cc)
            );
            builder.Conventions.AddMethodSchema(
                where: cc => cc.Path.EndsWith(nameof(ListPanel), nameof(ListPanel.Title)),
                schema: (c, cc) => MethodNameInline(c.Method, cc)
            );
            builder.Conventions.AddMethodComponentConfiguration<ListPanel>(
                component: (dp, c, cc) =>
                {
                    foreach (var parameter in c.Method.DefaultOverload.Parameters)
                    {
                        var input = parameter.GetRequiredSchema<Input>(cc.Drill(nameof(ListPanel), nameof(ListPanel.Inputs)));
                        input.QueryBound = true;

                        dp.Schema.Inputs.Add(input);
                    }
                }
            );
            builder.Conventions.AddMethodComponentConfiguration<DataTable>(
                where: cc => cc.Path.Contains(nameof(ListPanel)),
                component: datatable =>
                {
                    datatable.Schema.Paginator = false;
                    datatable.Schema.VirtualScrollerOptions = default;
                    datatable.Schema.DataLengthContextKey = nameof(DataTable.DataLengthContextKey).Kebaberize();
                }
            );

            // Skip
            builder.Conventions.AddParameterComponent(
                when: c => c.Parameter.Name == "skip",
                component: () => B.ServerPaginator()
            );
            builder.Conventions.AddParameterComponentConfiguration<ServerPaginator>(
                when: c => c.Parameter.Name == "skip",
                component: paginator =>
                {
                    paginator.Data = Datas.Context.Page(o =>
                    {
                        o.Prop = nameof(DataTable.DataLengthContextKey).Kebaberize(); ;
                        o.TargetProp = "length";
                    });
                    paginator.Data += Datas.Context.Page(o =>
                    {
                        o.Prop = "list-panel-take";
                        o.TargetProp = "take";
                    });
                    paginator.ReloadWhen(nameof(DataTable.DataLengthContextKey).Kebaberize());
                    paginator.ReloadWhen("list-panel-take");
                }
            );
            builder.Conventions.AddParameterSchemaConfiguration<Input>(
                when: c => c.Parameter.Name == "skip",
                schema: input =>
                {
                    input.Required = true;
                }
            );

            //Take
            builder.Conventions.AddParameterComponent(
                when: c => c.Parameter.Name == "take",
                component: (c, cc) =>
                {
                    cc = cc.Drill(nameof(Select));
                    var (_, l) = cc;

                    return B.Select(l("Take"), Datas.Inline(new[] { "10", "20", "50", "100" }, options: i => i.RequireLocalization = false));
                }
            );
            builder.Conventions.AddParameterComponentConfiguration<Select>(
                when: c => c.Parameter.Name == "take",
                component: s =>
                {
                    s.Schema.Stateful = true;
                    s.Schema.NoFloatLabel = true;
                    s.Action = Actions.Publish.PageContextValue("list-panel-take", o => o.Data = Datas.Context.Model());
                }
            );
            builder.Conventions.AddParameterSchemaConfiguration<Input>(
                when: c => c.Parameter.Name == "take",
                schema: input =>
                {
                    input.Default = Datas.Computed.Use("FirstValue");
                    input.Required = true;
                }
            );
        });
    }
}