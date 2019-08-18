using System;
using System.Collections.Generic;
using System.Text;

namespace WojciechMikołajewicz
{
	partial class Base128
	{
		/// <summary>
		/// Method tries to read 16-bit unsigned integer (<see cref="ushort"/>) from byte array
		/// </summary>
		/// <param name="source">Byte array from which read <paramref name="value"/></param>
		/// <param name="value">Read value</param>
		/// <param name="read">Number of bytes read</param>
		/// <returns>True if success or false if not - which means end of array was reached before whole <paramref name="value"/> was read</returns>
		/// <exception cref="OverflowException">Read value is too big for <see cref="ushort"/></exception>
		public static bool TryReadUInt16(ReadOnlySpan<byte> source, out ushort value, out int read)
		{
			uint val;
			bool bReturn;

			bReturn=TryReadUInt32(source: source, value: out val, read: out read);
			checked
			{
				value=(ushort)val;
			}
			return bReturn;
		}

		/// <summary>
		/// Method tries to read 16-bit signed integer (<see cref="short"/>) from byte array
		/// </summary>
		/// <param name="source">Byte array from which read <paramref name="value"/></param>
		/// <param name="value">Read value</param>
		/// <param name="read">Number of bytes read</param>
		/// <returns>True if success or false if not - which means end of array was reached before whole <paramref name="value"/> was read</returns>
		/// <exception cref="OverflowException">Read value is too big or too small for <see cref="short"/></exception>
		public static bool TryReadInt16(ReadOnlySpan<byte> source, out short value, out int read)
		{
			int val;
			bool bReturn;

			bReturn=TryReadInt32(source: source, value: out val, read: out read);
			checked
			{
				value=(short)val;
			}
			return bReturn;
		}

		/// <summary>
		/// Method tries to read 16-bit signed integer (<see cref="short"/>) from byte array ZigZag encoded (sign bit as the least significant bit)
		/// </summary>
		/// <param name="source">Byte array from which read <paramref name="value"/></param>
		/// <param name="value">Read value</param>
		/// <param name="read">Number of bytes read</param>
		/// <returns>True if success or false if not - which means end of array was reached before whole <paramref name="value"/> was read</returns>
		/// <exception cref="OverflowException">Read value is too big or too small for <see cref="short"/></exception>
		public static bool TryReadInt16ZigZag(ReadOnlySpan<byte> source, out short value, out int read)
		{
			int val;
			bool bReturn;

			bReturn=TryReadInt32ZigZag(source: source, value: out val, read: out read);
			checked
			{
				value=(short)val;
			}
			return bReturn;
		}

		/// <summary>
		/// Method reads 16-bit unsigned integer (<see cref="ushort"/>) from byte array
		/// </summary>
		/// <param name="source">Byte array from which read value</param>
		/// <param name="read">Number of bytes read</param>
		/// <returns>Read value</returns>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="source"/> is too small to contain an <see cref="ushort"/></exception>
		/// <exception cref="OverflowException">Read value is too big for <see cref="ushort"/></exception>
		public static ushort ReadUInt16(ReadOnlySpan<byte> source, out int read)
		{
			uint value;

			if(!TryReadUInt32(source: source, value: out value, read: out read))
				throw new ArgumentOutOfRangeException(nameof(source), $"{nameof(source)} is too small to contain an UInt32");
			checked
			{
				return (ushort)value;
			}
		}

		/// <summary>
		/// Method reads 16-bit signed integer (<see cref="short"/>) from byte array
		/// </summary>
		/// <param name="source">Byte array from which read value</param>
		/// <param name="read">Number of bytes read</param>
		/// <returns>Read value</returns>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="source"/> is too small to contain an <see cref="short"/></exception>
		/// <exception cref="OverflowException">Read value is too big for <see cref="short"/></exception>
		public static short ReadInt16(ReadOnlySpan<byte> source, out int read)
		{
			int value;

			if(!TryReadInt32(source: source, value: out value, read: out read))
				throw new ArgumentOutOfRangeException(nameof(source), $"{nameof(source)} is too small to contain an Int32");
			checked
			{
				return (short)value;
			}
		}

		/// <summary>
		/// Method reads 16-bit signed integer (<see cref="short"/>) from byte array ZigZag encoded (sign bit as the least significant bit)
		/// </summary>
		/// <param name="source">Byte array from which read value</param>
		/// <param name="read">Number of bytes read</param>
		/// <returns>Read value</returns>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="source"/> is too small to contain an <see cref="short"/></exception>
		/// <exception cref="OverflowException">Read value is too big for <see cref="short"/></exception>
		public static short ReadInt16ZigZag(ReadOnlySpan<byte> source, out int read)
		{
			int value;

			if(!TryReadInt32ZigZag(source: source, value: out value, read: out read))
				throw new ArgumentOutOfRangeException(nameof(source), $"{nameof(source)} is too small to contain an Int32");
			checked
			{
				return (short)value;
			}
		}
	}
}