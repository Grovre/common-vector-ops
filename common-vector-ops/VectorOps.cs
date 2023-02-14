using System.Numerics;
using System.Runtime.InteropServices;

namespace common_vector_ops;

public static class VectorOps
{
    internal static Span<T> Leftovers<T>(this Span<T> span) where T : struct
    {
        return span[^(span.Length % Vector<T>.Count)..];
    }
    
    public static T Sum<T>(this Span<T> span) where T : struct, INumber<T>
    {
        var iter = new VectorIterator<T>(span);
        var vsum = Vector<T>.Zero;
        while (iter.MoveNext())
        {
            vsum += iter.Current;
        }

        var sum = Vector.Sum(vsum);
        foreach (var n in span.Leftovers())
        {
            sum += n;
        }

        return sum;
    }
}