using Baked.Architecture;
using Baked.Business;

namespace Baked.CodingStyle.QueryStyle;

public class QueryMethodCodingStyleFeature : IFeature<CodingStyleConfigurator>
{
    static readonly HashSet<string> _takeParameterNames = ["take"];
    static readonly HashSet<string> _skipParameterNames = ["skip"];
    static readonly HashSet<string> _sortingParameterNames = ["sort"];

    public void Configure(LayerConfigurator configurator)
    {
        configurator.Domain.ConfigureDomainModelBuilder(builder =>
        {
            builder.Index.Method.Add<QueryMethodAttribute>();
            builder.Index.Parameter.Add<PagingAttribute>();
            builder.Index.Parameter.Add<SortingAttribute>();

            builder.Conventions.SetMethodAttribute(
                when: c => c.Type.Has<QueryAttribute>() && c.Method.Name == "By",
                attribute: () => new QueryMethodAttribute(),
                requiresIndex: true
            );

            builder.Conventions.AddMethodAttributeConfiguration<QueryMethodAttribute>(
                when: c => c.Method.DefaultOverload.Parameters.All(p => p.IsOptional),
                attribute: qm => qm.AllParametersAreOptional = true
            );

            builder.Conventions.SetParameterAttribute(
                when: c => c.Method.Has<QueryMethodAttribute>() && _takeParameterNames.Contains(c.Parameter.Name),
                attribute: p => new PagingAttribute(PagingAttribute.Role.Take),
                requiresIndex: true,
                order: 35
            );

            builder.Conventions.SetParameterAttribute(
                when: c => c.Method.Has<QueryMethodAttribute>() && _skipParameterNames.Contains(c.Parameter.Name),
                attribute: p => new PagingAttribute(PagingAttribute.Role.Skip),
                requiresIndex: true,
                order: 35
            );

            builder.Conventions.SetParameterAttribute(
                when: c => c.Method.Has<QueryMethodAttribute>() && _sortingParameterNames.Contains(c.Parameter.Name),
                attribute: () => new SortingAttribute(),
                requiresIndex: true,
                order: 35
            );
        });
    }
}