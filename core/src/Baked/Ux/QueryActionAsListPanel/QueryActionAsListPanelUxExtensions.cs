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
}