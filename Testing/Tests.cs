using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using common_vector_ops;

namespace Testing;

[SuppressMessage("Assertion", "NUnit2005:Consider using Assert.That(actual, Is.EqualTo(expected)) instead of Assert.AreEqual(expected, actual)")]
[SuppressMessage("Assertion", "NUnit2002:Consider using Assert.That(expr, Is.False) instead of Assert.IsFalse(expr)")]
[SuppressMessage("Assertion", "NUnit2003:Consider using Assert.That(expr, Is.True) instead of Assert.IsTrue(expr)")]
public class Tests
{
    public const int SignedMinArrayValues = -1_000;
    public const int SignedMaxArrayValues = 1_500;
    public const int UnsignedMinArrayValues = 0;
    public const int UnsignedMaxArrayValues = 2_500;
    public const int MinArrayLength = 17;
    public const int MaxArrayLength = 10_000;

    public static byte[] byteArray = Array.Empty<byte>();
    public static sbyte[] sbyteArray = Array.Empty<sbyte>();
    public static short[] shortArray = Array.Empty<short>();
    public static ushort[] ushortArray = Array.Empty<ushort>();
    public static int[] intArray = Array.Empty<int>();
    public static uint[] uintArray = Array.Empty<uint>();
    public static long[] longArray = Array.Empty<long>();
    public static ulong[] ulongArray = Array.Empty<ulong>();
    public static Int128[] i128Array = Array.Empty<Int128>(); // Not unmanaged
    public static UInt128[] u128Array = Array.Empty<UInt128>(); // Not unmanaged
    public static float[] floatArray = Array.Empty<float>();
    public static double[] doubleArray = Array.Empty<double>();
    public static decimal[] decimalArray = Array.Empty<decimal>();
    
    [SetUp]
    public void Setup()
    {
        var random = new Random();
        byteArray = Generator.GenerateArray(random.Next(MinArrayLength, MaxArrayLength), _ => (byte)random.Next(20));
        sbyteArray = Generator.GenerateArray(random.Next(MinArrayLength, MaxArrayLength), _ => (sbyte)random.Next(-10, 10));
        shortArray = Generator.GenerateArray(random.Next(MinArrayLength, MaxArrayLength), _ => (short)random.Next(-50, 50));
        ushortArray = Generator.GenerateArray(random.Next(MinArrayLength, MaxArrayLength), _ => (ushort)random.Next(100));
        intArray = Generator.GenerateArray(random.Next(MinArrayLength, MaxArrayLength), _ => random.Next(-1_000, 1_000));
        uintArray = Generator.GenerateArray(random.Next(MinArrayLength, MaxArrayLength), _ => (uint)random.Next(2_000));
        longArray = Generator.GenerateArray(random.Next(MinArrayLength, MaxArrayLength), _ => random.NextInt64(-10_000, 10_000));
        ulongArray = Generator.GenerateArray(random.Next(MinArrayLength, MaxArrayLength), _ => (ulong)random.NextInt64(20_000));
        i128Array = Generator.GenerateArray(random.Next(MinArrayLength, MaxArrayLength), _ => (Int128)random.NextInt64(-50_000, 50_000));
        u128Array = Generator.GenerateArray(random.Next(MinArrayLength, MaxArrayLength), _ => (UInt128)random.Next(100_000));
        floatArray = Generator.GenerateArray(random.Next(MinArrayLength, MaxArrayLength), _ => random.NextSingle() * 2_000F);
        doubleArray = Generator.GenerateArray(random.Next(MinArrayLength, MaxArrayLength), _ => random.NextDouble() * 20_000D);
        decimalArray = Generator.GenerateArray(random.Next(MinArrayLength, MaxArrayLength), _ => (decimal)(random.NextDouble() * 30_000D));
    }

    [Test]
    public void VectorEnumerator()
    {
        var arr = Enumerable.Range(0, 255).Select(n => (byte)n).ToArray();
        var iter = new VectorIterator<byte>(arr);

        var blockSize = Vector<byte>.Count;
        for (var i = 0; iter.MoveNext(); i += blockSize)
        {
            Assert.AreEqual(iter.Current, new Vector<byte>(Enumerable.Range(i, blockSize).Select(n => (byte)n).ToArray()));
        }

        iter = new();
        for (var i = 0; i < 10; i++)
            Assert.IsFalse(iter.MoveNext());
    }

    [Test]
    public void VectorSum()
    {
        Assert.AreEqual((byte)byteArray.Sum(b => b), byteArray.AsSpan().Sum());
        Assert.AreEqual(intArray.Sum(), intArray.AsSpan().Sum());
        Assert.AreEqual(longArray.Sum(), longArray.AsSpan().Sum());
    }

    [Test]
    public void VectorCount()
    {
        var random = new Random();
        var byt = (byte)random.Next();
        Assert.AreEqual(byteArray.Count(b => b == byt), byteArray.AsSpan().Count(byt));
        var n = random.Next(SignedMinArrayValues, SignedMaxArrayValues);
        Assert.AreEqual(intArray.Count(x => x == n), intArray.AsSpan().Count(n));
    }
}