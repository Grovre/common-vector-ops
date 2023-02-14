using System.Numerics;

namespace common_vector_ops;

public ref struct VectorIterator<T> where T : struct
{
    public int Index { get; private set; }
    public int Increment { get; }
    public Span<T> VectorizedSpan { get; }

    public VectorIterator() : this(Span<T>.Empty)
    {
    }

    public VectorIterator(T[] array)
    {
        VectorizedSpan = array.AsSpan();
        Increment = Vector<T>.Count;
        Index = -Increment;
    }

    public VectorIterator(Span<T> span)
    {
        VectorizedSpan = span;
        Increment = Vector<T>.Count;
        Index = -Increment;
    }

    public bool MoveNext()
    {
        Index += Increment;
        return Index <= VectorizedSpan.Length - Increment;
    }

    public void Reset()
    {
        Index = -Increment;
    }

    public readonly Vector<T> Current => new Vector<T>(VectorizedSpan[Index..]);
}

public static class VectorIteratorHelper
{
    public static VectorIterator<T> GetVectorIterator<T>(this Span<T> span) where T : struct
        => new VectorIterator<T>(span);

    public static VectorIterator<T> GetVectorIterator<T>(this T[] array) where T : struct
        => array.AsSpan().GetVectorIterator();
}