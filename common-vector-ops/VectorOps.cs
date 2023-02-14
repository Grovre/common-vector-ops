using System.Numerics;

namespace common_vector_ops;

public static class VectorOps
{
    public static T Sum<T>(this Span<T> span) where T : unmanaged, INumber<T>
    {
        var vsum = Vector<T>.Zero;
        foreach (var v in span.GetVectorIterator())
            vsum += v;

        var sum = Vector.Sum(vsum);
        foreach (var n in span.VectorLeftovers())
        {
            sum += n;
        }

        return sum;
    }

    public static int CountWithInt<T>(this Span<T> span, T toCount) where T : unmanaged, INumber<T>
    {
        var count = 0;
        var compareVector = new Vector<T>(toCount);
        var oneVector = Vector<T>.One;
        foreach (var v in span.GetVectorIterator())
        {
            var vcmp = Vector.Equals(v, compareVector);
            vcmp = Vector.BitwiseAnd(vcmp, oneVector);
            count += int.CreateChecked(Vector.Sum(vcmp));
        }

        foreach (var el in span.VectorLeftovers())
        {
            if (el == toCount)
                count += 1;
        }

        return count;
    }

    public static T CountWithType<T>(this Span<T> span, T toCount) where T : unmanaged, INumber<T>
    {
        var vcount = Vector<T>.Zero;
        var compareVector = new Vector<T>(toCount);
        var oneVector = Vector<T>.One;
        var iter = span.GetVectorIterator();
        foreach (var v in iter)
        {
            var vcmp = Vector.Equals(v, compareVector);
            vcmp = Vector.BitwiseAnd(vcmp, oneVector);
            vcount += vcmp;
        }

        var count = Vector.Sum(vcount);
        foreach (var el in iter.Leftovers)
            if (el == toCount)
                count += T.One;

        return count;
    }
}