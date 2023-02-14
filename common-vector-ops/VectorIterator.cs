using System.Collections;
using System.Diagnostics;
using System.Numerics;

namespace common_vector_ops;

public ref struct VectorIterator<T> where T : struct
{
    public int Index { get; private set; }
    public int Increment { get; }
    public Span<T> VectorizedMemoryRegion { get; }

    public VectorIterator() : this(Span<T>.Empty)
    {
    }

    public VectorIterator(T[] array)
    {
        VectorizedMemoryRegion = array.AsSpan();
        Increment = Vector<T>.Count;
        Index = -Increment;
    }

    public VectorIterator(Span<T> span)
    {
        VectorizedMemoryRegion = span;
        Increment = Vector<T>.Count;
        Index = -Increment;
    }

    public bool MoveNext()
    {
        Index += Increment;
        return Index <= VectorizedMemoryRegion.Length - Increment;
    }

    public void Reset()
    {
        Index = -Increment;
    }

    public Vector<T> Current => new Vector<T>(VectorizedMemoryRegion[Index..]);
}