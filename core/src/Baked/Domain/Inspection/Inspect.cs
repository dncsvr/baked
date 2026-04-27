using System.Linq.Expressions;

namespace Baked.Domain.Inspection;

public class Inspect
{
    public void Attribute<T>(
        Expression<Func<T, object?>>? attribute = default
    ) where T : Attribute
    {
        attribute ??= x => x;

        Inspection.Current = new(typeof(T), c => attribute.Compile().Invoke((T)c), attribute.ToString());
    }
}