using System;
using System.Collections.Generic;
using System.Text;

namespace WojciechMikołajewicz
{
	partial class Base128
	{
		/// <summary>
		/// Method tries write 64-bit unsigned integer (<see cref="ulong"/>) to byte array
		/// </summary>
		/// <param name="destination">Byte array to write <paramref name="value"/></param>
		/// <param name="value">Value to serialize</param>
		/// <param name="written">Number of bytes written</param>
		/// <returns>True if success or false if not - which means there was not sufficient space in byte array to write <paramref name="value"/></returns>
		public static bool TryWriteUInt64(Span<byte> destination, ulong value, out int written)
		{
			byte val;

			written=0;
			
			unchecked
			{
				while(written<destination.Length)
				{
					val=(byte)(value&0x7F);
					if(0!=(value&0xFFFFFFFFFFFFFF80UL))
						val|=0x80;
					destination[written]=val;
					written++;
					if(0<=(sbyte)val)
						return true;
					value>>=7;
				}
				return false;
			}
		}

		/// <summary>
		/// Method tries write 64-bit signed integer (<see cref="long"/>) to byte array
		/// </summary>
		/// <param name="destination">Byte array to write <paramref name="value"/></param>
		/// <param name="value">Value to serialize</param>
		/// <param name="written">Number of bytes written</param>
		/// <returns>True if success or false if not - which means there was not sufficient space in byte array to write <paramref name="value"/></returns>
		public static bool TryWriteInt64(Span<byte> destination, long value, out int written)
		{
			long insignificantValue;
			byte val;

			written=0;
			//Zero all bits except most significant bit (sign bit) and copy this bit 57 times to right
			insignificantValue=(value&long.MinValue)>>57;//value>=0 ? 0 : -64 (-64 = 0xFFFFFFFFFFFFFFC0)

			unchecked
			{
				while(written<destination.Length)
				{
					val=(byte)(value&0x7F);
					if(insignificantValue!=(value&-64L))//-64 = 0xFFFFFFFFFFFFFFC0
						val|=0x80;
					destination[written]=val;
					written++;
					if(0<=(sbyte)val)
						return true;
					value>>=7;
				}
				return false;
			}
		}

		/// <summary>
		/// Method tries write 64-bit signed integer (<see cref="long"/>) to byte array with ZigZag coding (sign bit as the least significant bit)
		/// </summary>
		/// <param name="destination">Byte array to write <paramref name="value"/></param>
		/// <param name="value">Value to serialize</param>
		/// <param name="written">Number of bytes written</param>
		/// <returns>True if success or false if not - which means there was not sufficient space in byte array to write <paramref name="value"/></returns>
		public static bool TryWriteInt64ZigZag(Span<byte> destination, long value, out int written)
		{
			return TryWriteUInt64(destination: destination, value: (ulong)((value<<1)^(value>>63)), written: out written);
		}

		/// <summary>
		/// Method tries to read 64-bit unsigned integer (<see cref="ulong"/>) from byte array
		/// </summary>
		/// <param name="source">Byte array from which read <paramref name="value"/></param>
		/// <param name="value">Read value</param>
		/// <param name="read">Number of bytes read</param>
		/// <returns>True if success or false if not - which means end of array was reached before whole <paramref name="value"/> was read</returns>
		/// <exception cref="OverflowException">Read value is too big for <see cref="ulong"/></exception>
		public static bool TryReadUInt64(ReadOnlySpan<byte> source, out ulong value, out int read)
		{
			byte val;
			int maxLoop;

			unchecked
			{
				//maxLoop=Min(source.Length, 9)
				maxLoop=source.Length<9 ? source.Length : 9;

				for(read=0, value=0; read<maxLoop;)//Eventually last (tenth) byte treat special
				{
					value>>=7;
					val=source[read];
					read++;
					value|=(ulong)val<<57;//Shift val to the most significant byte and one bit more to cut overflow bit

					if(0<=(sbyte)val)
					{
						value>>=64-read*7;
						return true;
					}
				}

				//If we are here it could be end of source or it is the last (tenth) byte
				if(read<source.Length)
				{
					//It is the last (tenth) byte. There could be only one bit (64-7*9)
					val=source[read];
					read++;
					if(0!=(val&0xFE))
						throw new OverflowException($"Value in {nameof(source)} is too big or too small");
					value=(value>>1)|(ulong)val<<63;
					return true;
				}
				return false;
			}
		}

		/// <summary>
		/// Method tries to read 64-bit signed integer (<see cref="long"/>) from byte array
		/// </summary>
		/// <param name="source">Byte array from which read <paramref name="value"/></param>
		/// <param name="value">Read value</param>
		/// <param name="read">Number of bytes read</param>
		/// <returns>True if success or false if not - which means end of array was reached before whole <paramref name="value"/> was read</returns>
		/// <exception cref="OverflowException">Read value is too big or too small for <see cref="long"/></exception>
		public static bool TryReadInt64(ReadOnlySpan<byte> source, out long value, out int read)
		{
			byte val;
			int maxLoop;

			unchecked
			{
				//maxLoop=Min(source.Length, 9)
				maxLoop=source.Length<9 ? source.Length : 9;

				for(read=0, value=0; read<maxLoop;)//Eventually last (tenth) byte treat special
				{
					value=(long)((ulong)value>>7);//Rotate unsigned - the most significant 7 bits have to be zeros. Next operation will be OR on those bits
					val=source[read];
					read++;
					value|=(long)val<<57;//Shift val to the most significant byte and one bit more to cut overflow bit

					if(0<=(sbyte)val)
					{
						value>>=64-read*7;
						return true;
					}
				}

				//If we are here it could be end of source or it is the last (tenth) byte
				if(read<source.Length)
				{
					//It is the last (tenth) byte. There could be only one significant bit (64-7*9)
					val=source[read];
					read++;
					if((uint)((int)val<<31>>6)>>25!=(uint)val)//val can be only 0b0000_0000 or 0b0111_1111
						throw new OverflowException($"Value in {nameof(source)} is too big or too small");
					value=(long)(((ulong)value>>1)|(ulong)val<<63);//Rotate unsigned - the most significant bit has to be zero. Next operation is OR on this bit
					return true;
				}
				return false;
			}
		}

		/// <summary>
		/// Method tries to read 64-bit signed integer (<see cref="long"/>) from byte array ZigZag encoded (sign bit as the least significant bit)
		/// </summary>
		/// <param name="source">Byte array from which read <paramref name="value"/></param>
		/// <param name="value">Read value</param>
		/// <param name="read">Number of bytes read</param>
		/// <returns>True if success or false if not - which means end of array was reached before whole <paramref name="value"/> was read</returns>
		/// <exception cref="OverflowException">Read value is too big or too small for <see cref="long"/></exception>
		public static bool TryReadInt64ZigZag(ReadOnlySpan<byte> source, out long value, out int read)
		{
			ulong uValue;
			bool success;

			success=TryReadUInt64(source: source, value: out uValue, read: out read);
			value=(long)(uValue>>1)^-(long)(uValue&1);
			return success;
		}

		/// <summary>
		/// Method returns number of bytes required to store <see cref="ulong"/> <paramref name="value"/>
		/// </summary>
		/// <param name="value">Value to check</param>
		/// <returns>Number of bytes required to store <see cref="ulong"/> <paramref name="value"/></returns>
		public static int GetRequiredBytesUInt64(ulong value)
		{
			int required = 1;

			//Base 2 logarithm cannot be used because double can't store ulong with full precision so (int)Math.Log(0xFFFFFFFFFFFFFF, 2) should be 55 and is 56
			//.Net Core 3.0 Math.ILogB method probably will not work too - for the same reason
			//(double)0xFFFFFFFFFFFFFF is indistinguishable from (double)0x100000000000000

			while(0!=(value>>=7))
			{
				required++;
			}

			return required;
		}

		/// <summary>
		/// Method returns number of bytes required to store <see cref="long"/> <paramref name="value"/>. It works also for ZigZag.
		/// </summary>
		/// <param name="value">Value to check</param>
		/// <returns>Number of bytes required to store <see cref="long"/> <paramref name="value"/></returns>
		public static int GetRequiredBytesInt64(long value)
		{
			long insignificantValue;
			int required = 1;

			//Copy most significant bit (sign bit) bit 63 times to right
			insignificantValue=value>>63;//value>=0 ? 0 : -1 (-1 = 0xFFFFFFFFFFFFFFFF)

			//First rotation is 6 bit
			if(insignificantValue!=(value>>=6))
			{
				required++;

				while(insignificantValue!=(value>>=7))
				{
					required++;
				}
			}

			return required;
		}

		/// <summary>
		/// Method writes 64-bit unsigned integer (<see cref="ulong"/>) to byte array
		/// </summary>
		/// <param name="destination">Byte array to write <paramref name="value"/></param>
		/// <param name="value">Value to serialize</param>
		/// <param name="written">Number of bytes written</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain an <see cref="ulong"/></exception>
		public static void WriteUInt64(Span<byte> destination, ulong value, out int written)
		{
			if(!TryWriteUInt64(destination: destination, value: value, written: out written))
				throw new ArgumentOutOfRangeException(nameof(destination), $"{nameof(destination)} is too small to contain an UInt64");
		}

		/// <summary>
		/// Method writes 64-bit signed integer (<see cref="long"/>) to byte array
		/// </summary>
		/// <param name="destination">Byte array to write <paramref name="value"/></param>
		/// <param name="value">Value to serialize</param>
		/// <param name="written">Number of bytes written</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain an <see cref="long"/></exception>
		public static void WriteInt64(Span<byte> destination, long value, out int written)
		{
			if(!TryWriteInt64(destination: destination, value: value, written: out written))
				throw new ArgumentOutOfRangeException(nameof(destination), $"{nameof(destination)} is too small to contain an Int64");
		}

		/// <summary>
		/// Method writes 64-bit signed integer (<see cref="long"/>) to byte array with ZigZag coding (sign bit as the least significant bit)
		/// </summary>
		/// <param name="destination">Byte array to write <paramref name="value"/></param>
		/// <param name="value">Value to serialize</param>
		/// <param name="written">Number of bytes written</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain an <see cref="long"/></exception>
		public static void WriteInt64ZigZag(Span<byte> destination, long value, out int written)
		{
			if(!TryWriteInt64ZigZag(destination: destination, value: value, written: out written))
				throw new ArgumentOutOfRangeException(nameof(destination), $"{nameof(destination)} is too small to contain an Int64");
		}

		/// <summary>
		/// Method reads 64-bit unsigned integer (<see cref="ulong"/>) from byte array
		/// </summary>
		/// <param name="source">Byte array from which read value</param>
		/// <param name="read">Number of bytes read</param>
		/// <returns>Read value</returns>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="source"/> is too small to contain an <see cref="ulong"/></exception>
		/// <exception cref="OverflowException">Read value is too big for <see cref="ulong"/></exception>
		public static ulong ReadUInt64(ReadOnlySpan<byte> source, out int read)
		{
			ulong value;

			if(!TryReadUInt64(source: source, value: out value, read: out read))
				throw new ArgumentOutOfRangeException(nameof(source), $"{nameof(source)} is too small to contain an UInt64");
			return value;
		}

		/// <summary>
		/// Method reads 64-bit signed integer (<see cref="long"/>) from byte array
		/// </summary>
		/// <param name="source">Byte array from which read value</param>
		/// <param name="read">Number of bytes read</param>
		/// <returns>Read value</returns>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="source"/> is too small to contain an <see cref="long"/></exception>
		/// <exception cref="OverflowException">Read value is too big for <see cref="long"/></exception>
		public static long ReadInt64(ReadOnlySpan<byte> source, out int read)
		{
			long value;

			if(!TryReadInt64(source: source, value: out value, read: out read))
				throw new ArgumentOutOfRangeException(nameof(source), $"{nameof(source)} is too small to contain an Int64");
			return value;
		}

		/// <summary>
		/// Method reads 64-bit signed integer (<see cref="long"/>) from byte array ZigZag encoded (sign bit as the least significant bit)
		/// </summary>
		/// <param name="source">Byte array from which read value</param>
		/// <param name="read">Number of bytes read</param>
		/// <returns>Read value</returns>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="source"/> is too small to contain an <see cref="long"/></exception>
		/// <exception cref="OverflowException">Read value is too big for <see cref="long"/></exception>
		public static long ReadInt64ZigZag(ReadOnlySpan<byte> source, out int read)
		{
			long value;

			if(!TryReadInt64ZigZag(source: source, value: out value, read: out read))
				throw new ArgumentOutOfRangeException(nameof(source), $"{nameof(source)} is too small to contain an Int64");
			return value;
		}
	}
}