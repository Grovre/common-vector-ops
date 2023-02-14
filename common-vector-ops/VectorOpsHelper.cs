using System.Numerics;

namespace common_vector_ops;

internal static class VectorOpsHelper
{
    public static Span<T> VectorLeftovers<T>(this Span<T> span) where T : struct 
        => span[^(span.Length % Vector<T>.Count)..];
}