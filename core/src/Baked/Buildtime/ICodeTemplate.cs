using System.Reflection;

namespace Baked.Buildtime;

public interface ICodeTemplate
{
    IEnumerable<Assembly> References { get; }

    IEnumerable<string> Render();
}