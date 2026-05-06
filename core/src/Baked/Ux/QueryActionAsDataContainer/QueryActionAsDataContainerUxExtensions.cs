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
}