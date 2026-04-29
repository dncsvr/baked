using Baked.Business;
using Baked.Domain.Model;
using Baked.Ux;
using Baked.Ux.QueryActionAsListPanel;

namespace Baked;

public static class QueryActionAsListPanelUxExtensions
{
    extension(UxConfigurator _)
    {
        public QueryActionAsListPanelUxFeature QueryActionAsListPanel() =>
            new();
    }

    extension(ParameterModel parameter)
    {
        public bool IsSkip =>
            parameter.Has<PagingAttribute>() && parameter.Get<PagingAttribute>().RoleOption == PagingAttribute.Role.Skip;

        public bool IsTake =>
            parameter.Has<PagingAttribute>() && parameter.Get<PagingAttribute>().RoleOption == PagingAttribute.Role.Skip;
    }
}