using System;
using System.Collections.Generic;
using System.Text;

namespace WojciechMikołajewicz
{
	partial class Base128
	{
		#region TryWrite
		/// <summary>
		/// Method tries write 64-bit unsigned integer (<see cref="ulong"/>) to byte array
		/// </summary>
		/// <param name="destination">Byte array to write <paramref name="value"/></param>
		/// <param name="value">Value to serialize</param>
		/// <param name="written">Number of bytes written</param>
		/// <returns>True if success or false if not - which means there was not sufficient space in <paramref name="destination"/> to write <paramref name="value"/></returns>
		/// <remarks>
		/// If method return false, <paramref name="written"/> will be set to zero but whole <paramref name="destination"/> will be polluted
		/// </remarks>
		public static bool TryWriteUInt64(Span<byte> destination, ulong value, out int written)
		{
			written=0;
			
			unchecked
			{
				while(written<destination.Length)
				{
					if(0==(value&0xFFFFFFFFFFFFFF80UL))//The last byte
					{
						destination[written]=(byte)(value&0x7F);
						written++;
						return true;
					}
					destination[written]=(byte)(value&0x7F|0x80);
					written++;
					value>>=7;
				}
				written=0;
				return false;
			}
		}

		/// <summary>
		/// Method tries write 64-bit unsigned integer (<see cref="ulong"/>) to byte array
		/// </summary>
		/// <param name="destination">Byte array to write <paramref name="value"/></param>
		/// <param name="value">Value to serialize</param>
		/// <param name="minBytesToWrite">Minimum number of bytes to write to <paramref name="destination"/>. It has to be less or equal to 10</param>
		/// <param name="written">Number of bytes written</param>
		/// <returns>True if success or false if not - which means there was not sufficient space in <paramref name="destination"/> to write <paramref name="value"/></returns>
		/// <exception cref="ArgumentException"><paramref name="minBytesToWrite"/> is too big</exception>
		/// <remarks>
		/// If method return false, <paramref name="written"/> will be set to zero but whole <paramref name="destination"/> will be polluted
		/// </remarks>
		public static bool TryWriteUInt64(Span<byte> destination, ulong value, int minBytesToWrite, out int written)
		{
			const int maxMinBytesToWrite = 10;

			written=0;

			unchecked
			{
				//Subtract one here to not to subtract in every loop's iteration
				minBytesToWrite--;
				if(maxMinBytesToWrite<=minBytesToWrite)
				{
					if(minBytesToWrite!=int.MaxValue)//If it was int.MinValue, subtract one is int.MaxValue, but it was ok, so do not throw exception
						throw new ArgumentException($"{nameof(minBytesToWrite)} cannot be greater than {maxMinBytesToWrite}", nameof(minBytesToWrite));
					minBytesToWrite=0;//Correct int.MaxValue to zero (which effectively means one)
				}

				while(written<destination.Length)
				{
					if(0==(value&0xFFFFFFFFFFFFFF80UL) && minBytesToWrite<=written)//The last byte
					{
						destination[written]=(byte)(value&0x7F);
						written++;
						return true;
					}
					destination[written]=(byte)(value&0x7F|0x80);
					written++;
					value>>=7;
				}
				written=0;
				return false;
			}
		}

		/// <summary>
		/// Method tries write 64-bit signed integer (<see cref="long"/>) to byte array
		/// </summary>
		/// <param name="destination">Byte array to write <paramref name="value"/></param>
		/// <param name="value">Value to serialize</param>
		/// <param name="written">Number of bytes written</param>
		/// <returns>True if success or false if not - which means there was not sufficient space in <paramref name="destination"/> to write <paramref name="value"/></returns>
		/// <remarks>
		/// If method return false, <paramref name="written"/> will be set to zero but whole <paramref name="destination"/> will be polluted
		/// </remarks>
		public static bool TryWriteInt64(Span<byte> destination, long value, out int written)
		{
			long insignificantValue;

			written=0;

			unchecked
			{
				//Zero all bits except most significant bit (sign bit) and copy this bit 57 times to right
				insignificantValue=(value&long.MinValue)>>57;//value>=0 ? 0 : -64 (-64 = 0xFFFFFFFFFFFFFFC0)

				while(written<destination.Length)
				{
					if(insignificantValue==(value&-64L))//-64 = 0xFFFFFFFFFFFFFFC0. The last byte
					{
						destination[written]=(byte)(value&0x7F);
						written++;
						return true;
					}
					destination[written]=(byte)(value&0x7F|0x80);
					written++;
					value>>=7;
				}
				written=0;
				return false;
			}
		}

		/// <summary>
		/// Method tries write 64-bit signed integer (<see cref="long"/>) to byte array
		/// </summary>
		/// <param name="destination">Byte array to write <paramref name="value"/></param>
		/// <param name="value">Value to serialize</param>
		/// <param name="minBytesToWrite">Minimum number of bytes to write to <paramref name="destination"/>. It has to be less or equal to 10</param>
		/// <param name="written">Number of bytes written</param>
		/// <returns>True if success or false if not - which means there was not sufficient space in <paramref name="destination"/> to write <paramref name="value"/></returns>
		/// <exception cref="ArgumentException"><paramref name="minBytesToWrite"/> is too big</exception>
		/// <remarks>
		/// If method return false, <paramref name="written"/> will be set to zero but whole <paramref name="destination"/> will be polluted
		/// </remarks>
		public static bool TryWriteInt64(Span<byte> destination, long value, int minBytesToWrite, out int written)
		{
			const int maxMinBytesToWrite = 10;
			long insignificantValue;

			written=0;

			unchecked
			{
				//Subtract one here to not to subtract in every loop's iteration
				minBytesToWrite--;
				if(maxMinBytesToWrite<=minBytesToWrite)
				{
					if(minBytesToWrite!=int.MaxValue)//If it was int.MinValue, subtract one is int.MaxValue, but it was ok, so do not throw exception
						throw new ArgumentException($"{nameof(minBytesToWrite)} cannot be greater than {maxMinBytesToWrite}", nameof(minBytesToWrite));
					minBytesToWrite=0;//Correct int.MaxValue to zero (which effectively means one)
				}

				//Zero all bits except most significant bit (sign bit) and copy this bit 57 times to right
				insignificantValue=(value&long.MinValue)>>57;//value>=0 ? 0 : -64 (-64 = 0xFFFFFFFFFFFFFFC0)

				while(written<destination.Length)
				{
					if(insignificantValue==(value&-64L) && minBytesToWrite<=written)//-64 = 0xFFFFFFFFFFFFFFC0. The last byte
					{
						destination[written]=(byte)(value&0x7F);
						written++;
						return true;
					}
					destination[written]=(byte)(value&0x7F|0x80);
					written++;
					value>>=7;
				}
				written=0;
				return false;
			}
		}

		/// <summary>
		/// Method tries write 64-bit signed integer (<see cref="long"/>) to byte array with ZigZag coding (sign bit as the least significant bit)
		/// </summary>
		/// <param name="destination">Byte array to write <paramref name="value"/></param>
		/// <param name="value">Value to serialize</param>
		/// <param name="written">Number of bytes written</param>
		/// <returns>True if success or false if not - which means there was not sufficient space in <paramref name="destination"/> to write <paramref name="value"/></returns>
		/// <remarks>
		/// If method return false, <paramref name="written"/> will be set to zero but whole <paramref name="destination"/> will be polluted
		/// </remarks>
		public static bool TryWriteInt64ZigZag(Span<byte> destination, long value, out int written)
		{
			return TryWriteUInt64(destination: destination, value: (ulong)((value<<1)^(value>>63)), written: out written);
		}

		/// <summary>
		/// Method tries write 64-bit signed integer (<see cref="long"/>) to byte array with ZigZag coding (sign bit as the least significant bit)
		/// </summary>
		/// <param name="destination">Byte array to write <paramref name="value"/></param>
		/// <param name="value">Value to serialize</param>
		/// <param name="minBytesToWrite">Minimum number of bytes to write to <paramref name="destination"/>. It has to be less or equal to 10</param>
		/// <param name="written">Number of bytes written</param>
		/// <returns>True if success or false if not - which means there was not sufficient space in <paramref name="destination"/> to write <paramref name="value"/></returns>
		/// <exception cref="ArgumentException"><paramref name="minBytesToWrite"/> is too big</exception>
		/// <remarks>
		/// If method return false, <paramref name="written"/> will be set to zero but whole <paramref name="destination"/> will be polluted
		/// </remarks>
		public static bool TryWriteInt64ZigZag(Span<byte> destination, long value, int minBytesToWrite, out int written)
		{
			return TryWriteUInt64(destination: destination, value: (ulong)((value<<1)^(value>>63)), minBytesToWrite: minBytesToWrite, written: out written);
		}
		#endregion
		#region TryRead
		/// <summary>
		/// Method tries to read 64-bit unsigned integer (<see cref="ulong"/>) from byte array
		/// </summary>
		/// <param name="source">Byte array from which read <paramref name="value"/></param>
		/// <param name="value">Read value</param>
		/// <param name="read">Number of bytes read</param>
		/// <returns>True if success or false if not - which means end of <paramref name="source"/> was reached before whole <paramref name="value"/> was read</returns>
		/// <exception cref="OverflowException">Read value is too big for <see cref="ulong"/></exception>
		/// <remarks>
		/// If method return false, <paramref name="value"/> and <paramref name="read"/> will be set to zero
		/// </remarks>
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
					//It is the last (tenth) byte. There could be only one bit (64-9*7)
					val=source[read];
					read++;
					if(0!=(val&0xFE))
						throw new OverflowException($"Value in {nameof(source)} is too big or too small");
					value=(value>>1)|(ulong)val<<63;
					return true;
				}
				value=0;
				read=0;
				return false;
			}
		}

		/// <summary>
		/// Method tries to read 64-bit signed integer (<see cref="long"/>) from byte array
		/// </summary>
		/// <param name="source">Byte array from which read <paramref name="value"/></param>
		/// <param name="value">Read value</param>
		/// <param name="read">Number of bytes read</param>
		/// <returns>True if success or false if not - which means end of <paramref name="source"/> was reached before whole <paramref name="value"/> was read</returns>
		/// <exception cref="OverflowException">Read value is too big or too small for <see cref="long"/></exception>
		/// <remarks>
		/// If method return false, <paramref name="value"/> and <paramref name="read"/> will be set to zero
		/// </remarks>
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
					//It is the last (tenth) byte. There could be only one significant bit (64-9*7)
					val=source[read];
					read++;
					if((uint)((int)val<<31>>6)>>25!=(uint)val)//val can be only 0b0000_0000 or 0b0111_1111
						throw new OverflowException($"Value in {nameof(source)} is too big or too small");
					value=(long)(((ulong)value>>1)|(ulong)val<<63);//Rotate unsigned - the most significant bit has to be zero. Next operation is OR on this bit
					return true;
				}
				value=0;
				read=0;
				return false;
			}
		}

		/// <summary>
		/// Method tries to read 64-bit signed integer (<see cref="long"/>) from byte array ZigZag encoded (sign bit as the least significant bit)
		/// </summary>
		/// <param name="source">Byte array from which read <paramref name="value"/></param>
		/// <param name="value">Read value</param>
		/// <param name="read">Number of bytes read</param>
		/// <returns>True if success or false if not - which means end of <paramref name="source"/> was reached before whole <paramref name="value"/> was read</returns>
		/// <exception cref="OverflowException">Read value is too big or too small for <see cref="long"/></exception>
		/// <remarks>
		/// If method return false, <paramref name="value"/> and <paramref name="read"/> will be set to zero
		/// </remarks>
		public static bool TryReadInt64ZigZag(ReadOnlySpan<byte> source, out long value, out int read)
		{
			ulong uValue;
			bool success;

			success=TryReadUInt64(source: source, value: out uValue, read: out read);
			value=(long)(uValue>>1)^-(long)(uValue&1);
			return success;
		}
		#endregion
		#region GetRequiredBytes
		/// <summary>
		/// Method returns number of bytes required to store <see cref="ulong"/> <paramref name="value"/>
		/// </summary>
		/// <param name="value">Value to check</param>
		/// <returns>Number of bytes required to store <see cref="ulong"/> <paramref name="value"/></returns>
		public static int GetRequiredBytesUInt64(ulong value)
		{
			int required;

			//Base 2 logarithm cannot be used because double can't store ulong with full precision so (int)Math.Log(0xFFFFFFFFFFFFFF, 2) should be 55 and is 56
			//.Net Core 3.0 Math.ILogB method probably will not work too - for the same reason
			//(double)0xFFFFFFFFFFFFFF is indistinguishable from (double)0x100000000000000
#if NETCOREAPP
			if(System.Runtime.Intrinsics.X86.Lzcnt.X64.IsSupported)
				required=(63-(int)System.Runtime.Intrinsics.X86.Lzcnt.X64.LeadingZeroCount(value))/7+1;
			else
#endif
			{
				required = 1;
				while(0!=(value>>=7))
				{
					required++;
				}
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
			//long and ZigZag take exactly the same bits, but ZigZag changes value to ulong and ulong can be calculated using Lzcnt, so do so
			return GetRequiredBytesUInt64((ulong)((value<<1)^(value>>63)));
		}
		#endregion
		#region Write
		/// <summary>
		/// Method writes 64-bit unsigned integer (<see cref="ulong"/>) to byte array
		/// </summary>
		/// <param name="destination">Byte array to write <paramref name="value"/></param>
		/// <param name="value">Value to serialize</param>
		/// <param name="written">Number of bytes written</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to hold <paramref name="value"/></exception>
		public static void WriteUInt64(Span<byte> destination, ulong value, out int written)
		{
			if(!TryWriteUInt64(destination: destination, value: value, written: out written))
				throw new ArgumentOutOfRangeException(nameof(destination), $"{nameof(destination)} is too small to contain an UInt64");
		}

		/// <summary>
		/// Method writes 64-bit unsigned integer (<see cref="ulong"/>) to byte array
		/// </summary>
		/// <param name="destination">Byte array to write <paramref name="value"/></param>
		/// <param name="value">Value to serialize</param>
		/// <param name="minBytesToWrite">Minimum number of bytes to write to <paramref name="destination"/>. It has to be less or equal to 10</param>
		/// <param name="written">Number of bytes written</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to hold <paramref name="value"/></exception>
		/// <exception cref="ArgumentException"><paramref name="minBytesToWrite"/> is too big</exception>
		public static void WriteUInt64(Span<byte> destination, ulong value, int minBytesToWrite, out int written)
		{
			if(!TryWriteUInt64(destination: destination, value: value, minBytesToWrite: minBytesToWrite, written: out written))
				throw new ArgumentOutOfRangeException(nameof(destination), $"{nameof(destination)} is too small to contain an UInt64");
		}

		/// <summary>
		/// Method writes 64-bit signed integer (<see cref="long"/>) to byte array
		/// </summary>
		/// <param name="destination">Byte array to write <paramref name="value"/></param>
		/// <param name="value">Value to serialize</param>
		/// <param name="written">Number of bytes written</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to hold <paramref name="value"/></exception>
		public static void WriteInt64(Span<byte> destination, long value, out int written)
		{
			if(!TryWriteInt64(destination: destination, value: value, written: out written))
				throw new ArgumentOutOfRangeException(nameof(destination), $"{nameof(destination)} is too small to contain an Int64");
		}

		/// <summary>
		/// Method writes 64-bit signed integer (<see cref="long"/>) to byte array
		/// </summary>
		/// <param name="destination">Byte array to write <paramref name="value"/></param>
		/// <param name="value">Value to serialize</param>
		/// <param name="minBytesToWrite">Minimum number of bytes to write to <paramref name="destination"/>. It has to be less or equal to 10</param>
		/// <param name="written">Number of bytes written</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to hold <paramref name="value"/></exception>
		/// <exception cref="ArgumentException"><paramref name="minBytesToWrite"/> is too big</exception>
		public static void WriteInt64(Span<byte> destination, long value, int minBytesToWrite, out int written)
		{
			if(!TryWriteInt64(destination: destination, value: value, minBytesToWrite: minBytesToWrite, written: out written))
				throw new ArgumentOutOfRangeException(nameof(destination), $"{nameof(destination)} is too small to contain an Int64");
		}

		/// <summary>
		/// Method writes 64-bit signed integer (<see cref="long"/>) to byte array with ZigZag coding (sign bit as the least significant bit)
		/// </summary>
		/// <param name="destination">Byte array to write <paramref name="value"/></param>
		/// <param name="value">Value to serialize</param>
		/// <param name="written">Number of bytes written</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to hold <paramref name="value"/></exception>
		public static void WriteInt64ZigZag(Span<byte> destination, long value, out int written)
		{
			if(!TryWriteInt64ZigZag(destination: destination, value: value, written: out written))
				throw new ArgumentOutOfRangeException(nameof(destination), $"{nameof(destination)} is too small to contain an Int64");
		}

		/// <summary>
		/// Method writes 64-bit signed integer (<see cref="long"/>) to byte array with ZigZag coding (sign bit as the least significant bit)
		/// </summary>
		/// <param name="destination">Byte array to write <paramref name="value"/></param>
		/// <param name="value">Value to serialize</param>
		/// <param name="minBytesToWrite">Minimum number of bytes to write to <paramref name="destination"/>. It has to be less or equal to 10</param>
		/// <param name="written">Number of bytes written</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to hold <paramref name="value"/></exception>
		/// <exception cref="ArgumentException"><paramref name="minBytesToWrite"/> is too big</exception>
		public static void WriteInt64ZigZag(Span<byte> destination, long value, int minBytesToWrite, out int written)
		{
			if(!TryWriteInt64ZigZag(destination: destination, value: value, minBytesToWrite: minBytesToWrite, written: out written))
				throw new ArgumentOutOfRangeException(nameof(destination), $"{nameof(destination)} is too small to contain an Int64");
		}
		#endregion
		#region Read
		/// <summary>
		/// Method reads 64-bit unsigned integer (<see cref="ulong"/>) from byte array
		/// </summary>
		/// <param name="source">Byte array from which read value</param>
		/// <param name="read">Number of bytes read</param>
		/// <returns>Read value</returns>
		/// <exception cref="ArgumentOutOfRangeException">End of <paramref name="source"/> was reached before whole value was read</exception>
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
		/// <exception cref="ArgumentOutOfRangeException">End of <paramref name="source"/> was reached before whole value was read</exception>
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
		/// <exception cref="ArgumentOutOfRangeException">End of <paramref name="source"/> was reached before whole value was read</exception>
		/// <exception cref="OverflowException">Read value is too big for <see cref="long"/></exception>
		public static long ReadInt64ZigZag(ReadOnlySpan<byte> source, out int read)
		{
			long value;

			if(!TryReadInt64ZigZag(source: source, value: out value, read: out read))
				throw new ArgumentOutOfRangeException(nameof(source), $"{nameof(source)} is too small to contain an Int64");
			return value;
		}
		#endregion
	}
}