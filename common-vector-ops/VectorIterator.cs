using System.Collections;
using System.Numerics;

namespace common_vector_ops;

public sealed class VectorIterator<T> : IEnumerator<Vector<T>> where T : struct
{
    private readonly int _resetIndex;
    public int Index { get; private set; }
    public int Increment { get; }
    public T[] VectorizedArray { get; }

    public VectorIterator(T[] array)
    {
        VectorizedArray = array;
        Increment = Vector<T>.Count;
        _resetIndex = -Increment;
        Index = _resetIndex;
    }

    public bool MoveNext()
    {
        Index += Increment;
        return Index <= VectorizedArray.Length - Increment;
    }

    public void Reset()
    {
        Index = _resetIndex;
    }

    public Vector<T> Current
    {
        get
        {
            var v = new Vector<T>(VectorizedArray, Index);
            return v;
        }
    }

    object IEnumerator.Current => Current;

    public void Dispose()
    {
        // Nothing to dispose of
    }
}