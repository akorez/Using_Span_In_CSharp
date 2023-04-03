using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;


BenchmarkRunner.Run<BinaryDataOperations>();
Console.ReadLine();


[MemoryDiagnoser]
public class StringProcessing
{
	public static readonly string originalString = "Hello, World!";

	[Benchmark]
	public void ExtractNumbers()
	{
		string subString = originalString.Substring(7, 5);
	}

	[Benchmark]
	public void ExtractNumbersWithSpan()
	{
		ReadOnlySpan<char> subString = originalString.AsSpan().Slice(7, 5);

	}

}


[MemoryDiagnoser]
public class StringProcessing2
{
	public static readonly string someString = "01 02 03";

	[Benchmark]
	public (int pos1, int pos2, int pos3) ExtractNumbers()
	{
		var strNumber1 = someString.Substring(0, 2);
		var strNumber2 = someString.Substring(2, 4);
		var strNumber3 = someString.Substring(6);

		return (int.Parse(strNumber1), int.Parse(strNumber2), int.Parse(strNumber3));
	}

	[Benchmark]
	public (int pos1, int pos2, int pos3) ExtractNumbersWithSpan()
	{
		ReadOnlySpan<char> spanString = someString;
		var strNumber1 = spanString.Slice(0, 2);
		var strNumber2 = spanString.Slice(2, 4);
		var strNumber3 = spanString.Slice(6);

		return (int.Parse(strNumber1), int.Parse(strNumber2), int.Parse(strNumber3));
	}

}

[MemoryDiagnoser]
public class ArrayOperations
{
	public static readonly int[] array = new int[1000000];

	[Benchmark]
	public void FillArray()
	{
		for (int i = 0; i < array.Length; i++)
		{
			array[i] += 10;
		}
	}

	[Benchmark]
	public void FillArrayWitSpan()
	{
		Span<int> span = new Span<int>(array);
		span.Fill(10);
	}

}

[MemoryDiagnoser]
public class BinaryDataOperations
{
	byte[] data = new byte[1000000];
	byte[] pattern = { 0x12, 0x34, 0x56 };
	int index = 0;

	[Benchmark]
	public void SearchPattern()
	{
		for (int i = 0; i < data.Length - pattern.Length; i++)
		{
			if (data[i] == pattern[0] && data[i + 1] == pattern[1] && data[i + 2] == pattern[2])
			{
				index = i;
				break;
			}
		}
	}

	[Benchmark]
	public void SearchPatternWithSpan()
	{
		Span<byte> span = new Span<byte>(data);
		index = span.IndexOf(pattern);
	}

}