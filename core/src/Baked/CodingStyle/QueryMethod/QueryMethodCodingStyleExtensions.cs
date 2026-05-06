using Baked.CodingStyle;
using Baked.CodingStyle.QueryMethod;

namespace Baked;

public static class QueryMethodCodingStyleExtensions
{
    extension(CodingStyleConfigurator _)
    {
        public QueryMethodCodingStyleFeature QueryMethod(
            HashSet<string>? takeParameterNames = default,
            HashSet<string>? skipParameterNames = default,
            HashSet<string>? sortingParameterNames = default
        )
        {
            takeParameterNames ??= ["take"];
            skipParameterNames ??= ["skip"];
            sortingParameterNames ??= ["sort"];

            return new(takeParameterNames, skipParameterNames, sortingParameterNames);
        }
    }
}