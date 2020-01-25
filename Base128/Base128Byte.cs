using System;
using System.Collections.Generic;
using System.Text;

namespace WojciechMikołajewicz
{
	partial class Base128
	{
		#region TryRead
		/// <summary>
		/// Method tries to read 8-bit unsigned integer (<see cref="byte"/>) from byte array
		/// </summary>
		/// <param name="source">Byte array from which read <paramref name="value"/></param>
		/// <param name="value">Read value</param>
		/// <param name="read">Number of bytes read</param>
		/// <returns>True if success or false if not - which means end of <paramref name="source"/> was reached before whole <paramref name="value"/> was read</returns>
		/// <exception cref="OverflowException">Read value is too big for <see cref="byte"/></exception>
		/// <remarks>
		/// If method return false, <paramref name="value"/> and <paramref name="read"/> will be set to zero
		/// </remarks>
		public static bool TryReadUInt8(ReadOnlySpan<byte> source, out byte value, out int read)
		{
			uint val;
			bool bReturn;

			bReturn=TryReadUInt32(source: source, value: out val, read: out read);
			checked
			{
				value=(byte)val;
			}
			return bReturn;
		}

		/// <summary>
		/// Method tries to read 8-bit signed integer (<see cref="sbyte"/>) from byte array
		/// </summary>
		/// <param name="source">Byte array from which read <paramref name="value"/></param>
		/// <param name="value">Read value</param>
		/// <param name="read">Number of bytes read</param>
		/// <returns>True if success or false if not - which means end of <paramref name="source"/> was reached before whole <paramref name="value"/> was read</returns>
		/// <exception cref="OverflowException">Read value is too big or too small for <see cref="sbyte"/></exception>
		/// <remarks>
		/// If method return false, <paramref name="value"/> and <paramref name="read"/> will be set to zero
		/// </remarks>
		public static bool TryReadInt8(ReadOnlySpan<byte> source, out sbyte value, out int read)
		{
			int val;
			bool bReturn;

			bReturn=TryReadInt32(source: source, value: out val, read: out read);
			checked
			{
				value=(sbyte)val;
			}
			return bReturn;
		}

		/// <summary>
		/// Method tries to read 8-bit signed integer (<see cref="sbyte"/>) from byte array ZigZag encoded (sign bit as the least significant bit)
		/// </summary>
		/// <param name="source">Byte array from which read <paramref name="value"/></param>
		/// <param name="value">Read value</param>
		/// <param name="read">Number of bytes read</param>
		/// <returns>True if success or false if not - which means end of <paramref name="source"/> was reached before whole <paramref name="value"/> was read</returns>
		/// <exception cref="OverflowException">Read value is too big or too small for <see cref="sbyte"/></exception>
		/// <remarks>
		/// If method return false, <paramref name="value"/> and <paramref name="read"/> will be set to zero
		/// </remarks>
		public static bool TryReadInt8ZigZag(ReadOnlySpan<byte> source, out sbyte value, out int read)
		{
			int val;
			bool bReturn;

			bReturn=TryReadInt32ZigZag(source: source, value: out val, read: out read);
			checked
			{
				value=(sbyte)val;
			}
			return bReturn;
		}
		#endregion
		#region Read
		/// <summary>
		/// Method reads 8-bit unsigned integer (<see cref="byte"/>) from byte array
		/// </summary>
		/// <param name="source">Byte array from which read value</param>
		/// <param name="read">Number of bytes read</param>
		/// <returns>Read value</returns>
		/// <exception cref="ArgumentOutOfRangeException">End of <paramref name="source"/> was reached before whole value was read</exception>
		/// <exception cref="OverflowException">Read value is too big for <see cref="byte"/></exception>
		public static byte ReadUInt8(ReadOnlySpan<byte> source, out int read)
		{
			uint value;

			if(!TryReadUInt32(source: source, value: out value, read: out read))
				throw new ArgumentOutOfRangeException(nameof(source), $"{nameof(source)} is too small to contain an UInt32");
			checked
			{
				return (byte)value;
			}
		}

		/// <summary>
		/// Method reads 8-bit signed integer (<see cref="sbyte"/>) from byte array
		/// </summary>
		/// <param name="source">Byte array from which read value</param>
		/// <param name="read">Number of bytes read</param>
		/// <returns>Read value</returns>
		/// <exception cref="ArgumentOutOfRangeException">End of <paramref name="source"/> was reached before whole value was read</exception>
		/// <exception cref="OverflowException">Read value is too big for <see cref="sbyte"/></exception>
		public static sbyte ReadInt8(ReadOnlySpan<byte> source, out int read)
		{
			int value;

			if(!TryReadInt32(source: source, value: out value, read: out read))
				throw new ArgumentOutOfRangeException(nameof(source), $"{nameof(source)} is too small to contain an Int32");
			checked
			{
				return (sbyte)value;
			}
		}

		/// <summary>
		/// Method reads 8-bit signed integer (<see cref="sbyte"/>) from byte array ZigZag encoded (sign bit as the least significant bit)
		/// </summary>
		/// <param name="source">Byte array from which read value</param>
		/// <param name="read">Number of bytes read</param>
		/// <returns>Read value</returns>
		/// <exception cref="ArgumentOutOfRangeException">End of <paramref name="source"/> was reached before whole value was read</exception>
		/// <exception cref="OverflowException">Read value is too big for <see cref="sbyte"/></exception>
		public static sbyte ReadInt8ZigZag(ReadOnlySpan<byte> source, out int read)
		{
			int value;

			if(!TryReadInt32ZigZag(source: source, value: out value, read: out read))
				throw new ArgumentOutOfRangeException(nameof(source), $"{nameof(source)} is too small to contain an Int32");
			checked
			{
				return (sbyte)value;
			}
		}
		#endregion
	}
}