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

    public static bool Compare<T>(this Span<T> span1, Span<T> other) where T : struct
    {
        if (span1.Length != other.Length)
            return false;

        var iter = span1.GetVectorIterator();
        var iter2 = other.GetVectorIterator();
        foreach (var v1 in iter) // I do not understand why this works
        {
            iter.MoveNext();
            iter2.MoveNext();
            var v2 = iter2.Current;
            if (!Vector.EqualsAll(v1, v2))
                return false;
        }

        var leftovers1 = iter.Leftovers;
        var leftovers2 = iter.Leftovers;
        for (var i = 0; i < leftovers1.Length; i++)
        {
            if (!leftovers1[i].Equals(leftovers2[i]))
                return false;
        }

        return true;
    }
}