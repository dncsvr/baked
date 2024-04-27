using Do.Architecture;
using Do.Business;
using Do.Orm;

namespace Do.CodingStyle.RichEntity;

public class RichEntityCodingStyleFeature : IFeature<CodingStyleConfigurator>
{
    public void Configure(LayerConfigurator configurator)
    {
        configurator.ConfigureDomainModelBuilder(builder =>
        {
            builder.Conventions.AddTypeMetadata(
                apply: (context, add) =>
                {
                    var query = context.Type;
                    var parameter =
                        query.GetMembers()
                            .Constructors
                            .SelectMany(o => o.Parameters)
                            .First(p => p.ParameterType.IsAssignableTo(typeof(IQueryContext<>)));

                    var entity = parameter.ParameterType.GetGenerics().GenericTypeArguments.First().Model;
                    Type? queryContext = null;
                    entity.Apply(t => queryContext = typeof(IQueryContext<>).MakeGenericType(t));
                    if (queryContext is null) { return; }

                    entity.Apply(t =>
                        add(query, new QueryAttribute(t))
                    );
                    query.Apply(t =>
                        add(entity.GetMetadata(), new EntityAttribute(t, queryContext))
                    );
                    add(entity.GetMetadata(), new ApiInputAttribute());
                },
                when: c =>
                    c.Type.TryGetMembers(out var members) &&
                    members.Constructors.Any(o => o.Parameters.Any(p => p.ParameterType.IsAssignableTo(typeof(IQueryContext<>))))
            );
            builder.Conventions.AddTypeMetadata(new LocatableAttribute(),
                when: c => c.Type.Has<EntityAttribute>()
            );
            builder.Conventions.AddMethodMetadata(new ApiMethodAttribute(),
                when: c =>
                    c.Type.Has<EntityAttribute>() && c.Method.Has<InitializerAttribute>() &&
                    c.Method.Overloads.Any(o => o.IsPublic && !o.IsStatic && !o.IsSpecialName && o.AllParametersAreApiInput()),
                order: 30
            );
        });

        configurator.ConfigureNHibernateInterceptor(interceptor =>
        {
            interceptor.Instantiator = (ctx, id) =>
            {
                var result = ctx.ApplicationServices.GetRequiredServiceUsingRequestServices(ctx.MetaData.MappedClass);

                ctx.MetaData.SetIdentifier(result, id);

                return result;
            };
        });

        configurator.ConfigureApiModelConventions(conventions =>
        {
            var domainModel = configurator.Context.GetDomainModel();

            conventions.Add(new EntityUnderEntitiesConvention());
            conventions.Add(new EntityInitializerIsPostResourceConvention());
            conventions.Add(new TargetEntityFromRouteConvention(domainModel));
        });
    }
}