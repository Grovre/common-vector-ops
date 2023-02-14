using System.Numerics;

namespace common_vector_ops;

public static class VectorOps
{
    public static T Sum<T>(this Span<T> span) where T : unmanaged, INumber<T>
    {
        var iter = new VectorIterator<T>(span);
        var vsum = Vector<T>.Zero;
        while (iter.MoveNext())
        {
            vsum += iter.Current;
        }

        var sum = Vector.Sum(vsum);
        foreach (var n in span.VectorLeftovers())
        {
            sum += n;
        }

        return sum;
    }

    public static int Count<T>(this Span<T> span, T toCount) where T : unmanaged, INumber<T>
    {
        var count = 0;
        var compareVector = new Vector<T>(toCount);
        var iter = span.GetVectorIterator();
        while (iter.MoveNext())
        {
            count += int.CreateChecked(Vector.Sum(Vector.BitwiseAnd(Vector.Equals(compareVector, iter.Current), Vector<T>.One)));
        }

        foreach (var el in span.VectorLeftovers())
        {
            if (el == toCount)
                count += 1;
        }

        return count;
    }
}