using Baked.Business;
using Baked.Domain.Model;
using Baked.Ux;
using Baked.Ux.QueryActionAsDataContainer;

namespace Baked;

public static class QueryActionAsDataContainerUxExtensions
{
    extension(UxConfigurator _)
    {
        public QueryActionAsDataContainerUxFeature QueryActionAsDataContainer() =>
            new();
    }

    extension(ParameterModel parameter)
    {
        public bool IsSkip =>
            parameter.Has<PagingAttribute>() && parameter.Get<PagingAttribute>().RoleOption == PagingAttribute.Role.Skip;

        public bool IsTake =>
            parameter.Has<PagingAttribute>() && parameter.Get<PagingAttribute>().RoleOption == PagingAttribute.Role.Take;
    }
}