using Baked.CodingStyle;
using Baked.CodingStyle.QueryStyle;

namespace Baked;

public static class QueryMethodCodingStyleExtensions
{
    extension(CodingStyleConfigurator _)
    {
        public QueryMethodCodingStyleFeature QueryMethod() =>
            new();
    }
}