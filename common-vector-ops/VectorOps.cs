using System.Numerics;

namespace common_vector_ops;

public static class VectorOps
{
    internal static Span<T> Leftovers<T>(this Span<T> span) where T : struct
    {
        return span[^(span.Length % Vector<T>.Count)..];
    }
    public static T Sum<T>(this Span<T> span) where T : struct, INumber<T>
    {
        var blockSize = Vector<T>.Count;
        var blocks = span.Length / blockSize;
        var vsum = Vector<T>.Zero;
        for (var block = 0; block < blocks; block++)
        {
            var blockIndex = block * blockSize;
            var blockVector = new Vector<T>(span[blockIndex..]);
            vsum += blockVector;
        }

        var sum = Vector.Sum(vsum);
        foreach (var n in span.Leftovers())
        {
            sum += n;
        }

        return sum;
    }
}