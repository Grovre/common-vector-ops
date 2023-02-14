namespace common_vector_ops;

internal static class VectorOpsHelper
{
    public static VectorIterator<T> GetVectorIterator<T>(this Span<T> span) where T : struct
        => new VectorIterator<T>(span);

    public static VectorIterator<T> GetVectorIterator<T>(this T[] array) where T : struct
        => array.AsSpan().GetVectorIterator();
}