namespace Testing;

public static class Generator
{
    public delegate T IntFunction<T>(int n);
    
    public static IEnumerable<T> GenerateEnumerable<T>(int length, IntFunction<T> indexKeyGenerator)
    {
        var array = new T[length];
        var i = 0;
        foreach (ref var el in array.AsSpan())
        {
            el = indexKeyGenerator(i++);
        }

        return array;
    }

    public static IEnumerable<T> GenerateEnumerable<T>(int length, Func<T> keyGenerator)
        => GenerateEnumerable(length, _ => keyGenerator());

    public static T[] GenerateArray<T>(int length, IntFunction<T> indexKeyGenerator)
        => GenerateEnumerable(length, indexKeyGenerator).ToArray();
}