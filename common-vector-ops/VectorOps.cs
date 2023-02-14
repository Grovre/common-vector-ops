using System.Numerics;
using System.Runtime.InteropServices;

namespace common_vector_ops;

public static class VectorOps
{
    internal static Span<T> Leftovers<T>(this Span<T> span) where T : struct 
        => span[^(span.Length % Vector<T>.Count)..];

    public static T Sum<T>(this Span<T> span) where T : unmanaged, INumber<T>
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

    public static int Count<T>(this Span<T> span, T o) where T : unmanaged, INumber<T>
    {
        var count = 0;
        var vcmp = new Vector<T>(o);
        var iter = span.GetVectorIterator();
        while (iter.MoveNext())
        {
            count += int.CreateChecked(Vector.Sum(Vector.BitwiseAnd(Vector.Equals(vcmp, iter.Current), Vector<T>.One)));
        }

        foreach (var el in span.Leftovers())
        {
            if (el == o)
                count += 1;
        }

        return count;
    }
}