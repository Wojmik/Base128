using System;

namespace WojciechMikołajewicz
{
	/// <summary>
	/// Class for Base128 integer conversions
	/// </summary>
	public static partial class Base128
	{
		/// <summary>
		/// Method tries to skip Base128 variable integer value
		/// </summary>
		/// <param name="source">Byte array</param>
		/// <param name="read">Number of bytes read</param>
		/// <returns>True if success or false if not - which means end of array was reached before whole value was skipped</returns>
		public static bool TrySkip(ReadOnlySpan<byte> source, out int read)
		{
			byte val;

			read=0;

			while(read<source.Length)
			{
				val=source[read];
				read++;
				if(0<=(sbyte)source[read])
					return true;
			}
			return false;
		}

		/// <summary>
		/// Method skips Base128 variable integer value
		/// </summary>
		/// <param name="source">Byte array</param>
		/// <param name="read">Number of bytes read</param>
		/// <exception cref="ArgumentOutOfRangeException">End of <paramref name="source"/> was reached before whole value was skipped</exception>
		public static void Skip(ReadOnlySpan<byte> source, out int read)
		{
			if(!TrySkip(source: source, read: out read))
				throw new ArgumentOutOfRangeException(nameof(source), $"End of {nameof(source)} was reached before whole value was skipped");
		}
	}
}