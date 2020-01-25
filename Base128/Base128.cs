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
		/// <returns>True if success or false if not - which means end of <paramref name="source"/> was reached before whole value was skipped</returns>
		/// <remarks>
		/// If method return false, <paramref name="read"/> will be set to zero
		/// </remarks>
		public static bool TrySkip(ReadOnlySpan<byte> source, out int read)
		{
			byte val;

			read=0;

			while(read<source.Length)
			{
				val=source[read];
				read++;
				if(0<=(sbyte)val)
					return true;
			}
			read=0;
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