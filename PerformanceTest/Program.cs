using System;

namespace PerformanceTest
{
	class Program
	{
		static void Main(string[] args)
		{
			int i;
			uint val1, val2;

			Console.WriteLine($"Lzcnt supported: {System.Runtime.Intrinsics.X86.Lzcnt.IsSupported}");

			for(i=0; i<=32; i++)
			{
				val2=(uint)Math.Pow(2, i);
				val1=val2-1;

				Console.WriteLine($"{val1,8:X}: {Lzcnt(val1),2} {System.Runtime.Intrinsics.X86.Lzcnt.LeadingZeroCount(val1),2} {HowManyBitsUsed(val1),2}");

				Console.WriteLine($"{val2,8:X}: {Lzcnt(val2),2} {System.Runtime.Intrinsics.X86.Lzcnt.LeadingZeroCount(val2),2} {HowManyBitsUsed(val2),2}");
			}
		}

		static int Lzcnt(uint value)
		{
			int i=(int)Math.Log(value, 2);
			return 31-(i&int.MaxValue)-(i>>31);
		}

		static int HowManyBitsUsed(uint value)
		{
			return 1+((int)Math.Log(value, 2)&int.MaxValue);
		}
	}
}