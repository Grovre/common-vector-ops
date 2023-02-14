using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Testing;

public static class RandomHelper
{
    public static T NextStruct<T>(this Random random) where T : struct
    {
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            throw new Exception("Cannot generate a struct containing a ref");
        
        var o = default(T);
        random.NextBytes(MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref o, 1)));
        return o;
    }
}