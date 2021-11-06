using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace WojciechMikołajewicz
{
	partial class Base128
	{
		#region TryWrite
		/// <summary>
		/// Method tries write 32-bit unsigned integer (<see cref="uint"/>) to byte array
		/// </summary>
		/// <param name="destination">Byte array to write <paramref name="value"/></param>
		/// <param name="value">Value to serialize</param>
		/// <param name="written">Number of bytes written</param>
		/// <returns>True if success or false if not - which means there was not sufficient space in <paramref name="destination"/> to write <paramref name="value"/></returns>
		/// <remarks>
		/// If method return false, <paramref name="written"/> will be set to zero but whole <paramref name="destination"/> will be polluted
		/// </remarks>
		public static bool TryWriteUInt32(Span<byte> destination, uint value, out int written)
		{
			written=0;

			unchecked
			{
				while(written<destination.Length)
				{
					if(0==(value&0xFFFFFF80U))//The last byte
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
		/// Method tries write 32-bit unsigned integer (<see cref="uint"/>) to byte array
		/// </summary>
		/// <param name="destination">Byte array to write <paramref name="value"/></param>
		/// <param name="value">Value to serialize</param>
		/// <param name="minBytesToWrite">Minimum number of bytes to write to <paramref name="destination"/>. It has to be less or equal to 5</param>
		/// <param name="written">Number of bytes written</param>
		/// <returns>True if success or false if not - which means there was not sufficient space in <paramref name="destination"/> to write <paramref name="value"/></returns>
		/// <exception cref="ArgumentException"><paramref name="minBytesToWrite"/> is too big</exception>
		/// <remarks>
		/// If method return false, <paramref name="written"/> will be set to zero but whole <paramref name="destination"/> will be polluted
		/// </remarks>
		public static bool TryWriteUInt32(Span<byte> destination, uint value, int minBytesToWrite, out int written)
		{
			const int maxMinBytesToWrite = 5;

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
					if(0==(value&0xFFFFFF80U) && minBytesToWrite<=written)//The last byte
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
		/// Method tries write 32-bit signed integer (<see cref="int"/>) to byte array
		/// </summary>
		/// <param name="destination">Byte array to write <paramref name="value"/></param>
		/// <param name="value">Value to serialize</param>
		/// <param name="written">Number of bytes written</param>
		/// <returns>True if success or false if not - which means there was not sufficient space in <paramref name="destination"/> to write <paramref name="value"/></returns>
		/// <remarks>
		/// If method return false, <paramref name="written"/> will be set to zero but whole <paramref name="destination"/> will be polluted
		/// </remarks>
		public static bool TryWriteInt32(Span<byte> destination, int value, out int written)
		{
			int insignificantValue;

			written=0;

			unchecked
			{
				//Zero all bits except most significant bit (sign bit) and copy this bit 25 times to right
				insignificantValue=(value&int.MinValue)>>25;//value>=0 ? 0 : -64 (-64 = 0xFFFFFFC0)

				while(written<destination.Length)
				{
					if(insignificantValue==(value&-64))//-64 = 0xFFFFFFC0
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
		/// Method tries write 32-bit signed integer (<see cref="int"/>) to byte array
		/// </summary>
		/// <param name="destination">Byte array to write <paramref name="value"/></param>
		/// <param name="value">Value to serialize</param>
		/// <param name="minBytesToWrite">Minimum number of bytes to write to <paramref name="destination"/>. It has to be less or equal to 5</param>
		/// <param name="written">Number of bytes written</param>
		/// <returns>True if success or false if not - which means there was not sufficient space in <paramref name="destination"/> to write <paramref name="value"/></returns>
		/// <exception cref="ArgumentException"><paramref name="minBytesToWrite"/> is too big</exception>
		/// <remarks>
		/// If method return false, <paramref name="written"/> will be set to zero but whole <paramref name="destination"/> will be polluted
		/// </remarks>
		public static bool TryWriteInt32(Span<byte> destination, int value, int minBytesToWrite, out int written)
		{
			const int maxMinBytesToWrite = 5;
			int insignificantValue;

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

				//Zero all bits except most significant bit (sign bit) and copy this bit 25 times to right
				insignificantValue=(value&int.MinValue)>>25;//value>=0 ? 0 : -64 (-64 = 0xFFFFFFC0)

				while(written<destination.Length)
				{
					if(insignificantValue==(value&-64) && minBytesToWrite<=written)//-64 = 0xFFFFFFC0
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
		/// Method tries write 32-bit signed integer (<see cref="int"/>) to byte array with ZigZag coding (sign bit as the least significant bit)
		/// </summary>
		/// <param name="destination">Byte array to write <paramref name="value"/></param>
		/// <param name="value">Value to serialize</param>
		/// <param name="written">Number of bytes written</param>
		/// <returns>True if success or false if not - which means there was not sufficient space in <paramref name="destination"/> to write <paramref name="value"/></returns>
		/// <remarks>
		/// If method return false, <paramref name="written"/> will be set to zero but whole <paramref name="destination"/> will be polluted
		/// </remarks>
		public static bool TryWriteInt32ZigZag(Span<byte> destination, int value, out int written)
		{
			return TryWriteUInt32(destination: destination, value: (uint)((value<<1)^(value>>31)), written: out written);
		}

		/// <summary>
		/// Method tries write 32-bit signed integer (<see cref="int"/>) to byte array with ZigZag coding (sign bit as the least significant bit)
		/// </summary>
		/// <param name="destination">Byte array to write <paramref name="value"/></param>
		/// <param name="value">Value to serialize</param>
		/// <param name="minBytesToWrite">Minimum number of bytes to write to <paramref name="destination"/>. It has to be less or equal to 5</param>
		/// <param name="written">Number of bytes written</param>
		/// <returns>True if success or false if not - which means there was not sufficient space in <paramref name="destination"/> to write <paramref name="value"/></returns>
		/// <exception cref="ArgumentException"><paramref name="minBytesToWrite"/> is too big</exception>
		/// <remarks>
		/// If method return false, <paramref name="written"/> will be set to zero but whole <paramref name="destination"/> will be polluted
		/// </remarks>
		public static bool TryWriteInt32ZigZag(Span<byte> destination, int value, int minBytesToWrite, out int written)
		{
			return TryWriteUInt32(destination: destination, value: (uint)((value<<1)^(value>>31)), minBytesToWrite: minBytesToWrite, written: out written);
		}
		#endregion
		#region TryRead
		/// <summary>
		/// Method tries to read 32-bit unsigned integer (<see cref="uint"/>) from byte array
		/// </summary>
		/// <param name="source">Byte array from which read <paramref name="value"/></param>
		/// <param name="value">Read value</param>
		/// <param name="read">Number of bytes read</param>
		/// <returns>True if success or false if not - which means end of <paramref name="source"/> was reached before whole <paramref name="value"/> was read</returns>
		/// <exception cref="OverflowException">Read value is too big for <see cref="uint"/></exception>
		/// <remarks>
		/// If method return false, <paramref name="value"/> and <paramref name="read"/> will be set to zero
		/// </remarks>
		public static bool TryReadUInt32(ReadOnlySpan<byte> source, out uint value, out int read)
		{
			byte val;
			int maxLoop;

			unchecked
			{
				//maxLoop=Min(source.Length, 4)
				maxLoop=source.Length<4 ? source.Length : 4;

				for(read=0, value=0; read<maxLoop;)//Eventually last (fifth) byte treat special
				{
					value>>=7;
					val=source[read];
					read++;
					value|=(uint)val<<25;//Shift val to the most significant byte and one bit more to cut overflow bit

					if(0<=(sbyte)val)
					{
						value>>=32-read*7;
						return true;
					}
				}

				//If we are here it could be end of source or it is the last (fifth) byte
				if(read<source.Length)
				{
					//It is the last (fifth) byte. There could be only four bits (32-4*7)
					val=source[read];
					read++;
					if(0!=(val&0xF0))
						throw new OverflowException($"Value in {nameof(source)} is too big or too small");
					value=(value>>4)|(uint)val<<28;
					return true;
				}
				value=0;
				read=0;
				return false;
			}
		}

		/// <summary>
		/// Method tries to read 32-bit signed integer (<see cref="int"/>) from byte array
		/// </summary>
		/// <param name="source">Byte array from which read <paramref name="value"/></param>
		/// <param name="value">Read value</param>
		/// <param name="read">Number of bytes read</param>
		/// <returns>True if success or false if not - which means end of <paramref name="source"/> was reached before whole <paramref name="value"/> was read</returns>
		/// <exception cref="OverflowException">Read value is too big or too small for <see cref="int"/></exception>
		/// <remarks>
		/// If method return false, <paramref name="value"/> and <paramref name="read"/> will be set to zero
		/// </remarks>
		public static bool TryReadInt32(ReadOnlySpan<byte> source, out int value, out int read)
		{
			byte val;
			int maxLoop;

			unchecked
			{
				//maxLoop=Min(source.Length, 4)
				maxLoop=source.Length<4 ? source.Length : 4;

				for(read=0, value=0; read<maxLoop;)//Eventually last (fifth) byte treat special
				{
					value=(int)((uint)value>>7);//Rotate unsigned - the most significant 7 bits have to be zeros. Next operation will be OR on those bits
					val=source[read];
					read++;
					value|=(int)val<<25;//Shift val to the most significant byte and one bit more to cut overflow bit

					if(0<=(sbyte)val)
					{
						value>>=32-read*7;
						return true;
					}
				}

				//If we are here it could be end of source or it is the last (fifth) byte
				if(read<source.Length)
				{
					//It is the last (fifth) byte. There could be only four significant bits (32-4*7)
					val=source[read];
					read++;
					if((uint)((int)val<<28>>3)>>25!=(uint)val)//val can be only 0b0000_0xxx or 0b0111_1xxx
						throw new OverflowException($"Value in {nameof(source)} is too big or too small");
					value=(int)(((uint)value>>4)|(uint)val<<28);//Rotate unsigned - the most significant bits have to be zeros. Next operation is OR on those bits
					return true;
				}
				value=0;
				read=0;
				return false;
			}
		}

		/// <summary>
		/// Method tries to read 32-bit signed integer (<see cref="int"/>) from byte array ZigZag encoded (sign bit as the least significant bit)
		/// </summary>
		/// <param name="source">Byte array from which read <paramref name="value"/></param>
		/// <param name="value">Read value</param>
		/// <param name="read">Number of bytes read</param>
		/// <returns>True if success or false if not - which means end of <paramref name="source"/> was reached before whole <paramref name="value"/> was read</returns>
		/// <exception cref="OverflowException">Read value is too big or too small for <see cref="int"/></exception>
		/// <remarks>
		/// If method return false, <paramref name="value"/> and <paramref name="read"/> will be set to zero
		/// </remarks>
		public static bool TryReadInt32ZigZag(ReadOnlySpan<byte> source, out int value, out int read)
		{
			uint uValue;
			bool success;

			success=TryReadUInt32(source: source, value: out uValue, read: out read);
			value=(int)(uValue>>1)^-(int)(uValue&1);
			return success;
		}
		#endregion
		#region GetRequiredBytes
		/// <summary>
		/// Method returns number of bytes required to store <see cref="uint"/> <paramref name="value"/>
		/// </summary>
		/// <param name="value">Value to check</param>
		/// <returns>Number of bytes required to store <see cref="uint"/> <paramref name="value"/></returns>
		public static int GetRequiredBytesUInt32(uint value)
		{
			int required;

#if NETCOREAPP3_0_OR_GREATER
			required=(31-BitOperations.LeadingZeroCount(value))/7+1;
#else
			//Math.Log(0, 2) is -Infinity, cast to int is 0x80000000
			required=((int)Math.Log(value, 2)&int.MaxValue)/7+1;
#endif

			//required = 1;
			//while(0!=(value>>=7))
			//{
			//	required++;
			//}

			return required;
		}

		/// <summary>
		/// Method returns number of bytes required to store <see cref="int"/> <paramref name="value"/>. It works also for ZigZag.
		/// </summary>
		/// <param name="value">Value to check</param>
		/// <returns>Number of bytes required to store <see cref="int"/> <paramref name="value"/></returns>
		public static int GetRequiredBytesInt32(int value)
		{
			//int and ZigZag take exactly the same bits, but ZigZag changes value to uint and uint can be calculated using Lzcnt, so do so
			return GetRequiredBytesUInt32((uint)((value<<1)^(value>>31)));
		}
		#endregion
		#region Write
		/// <summary>
		/// Method writes 32-bit unsigned integer (<see cref="uint"/>) to byte array
		/// </summary>
		/// <param name="destination">Byte array to write <paramref name="value"/></param>
		/// <param name="value">Value to serialize</param>
		/// <param name="written">Number of bytes written</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to hold <paramref name="value"/></exception>
		public static void WriteUInt32(Span<byte> destination, uint value, out int written)
		{
			if(!TryWriteUInt32(destination: destination, value: value, written: out written))
				throw new ArgumentOutOfRangeException(nameof(destination), $"{nameof(destination)} is too small to contain an UInt32");
		}

		/// <summary>
		/// Method writes 32-bit unsigned integer (<see cref="uint"/>) to byte array
		/// </summary>
		/// <param name="destination">Byte array to write <paramref name="value"/></param>
		/// <param name="value">Value to serialize</param>
		/// <param name="minBytesToWrite">Minimum number of bytes to write to <paramref name="destination"/>. It has to be less or equal to 5</param>
		/// <param name="written">Number of bytes written</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to hold <paramref name="value"/></exception>
		/// <exception cref="ArgumentException"><paramref name="minBytesToWrite"/> is too big</exception>
		public static void WriteUInt32(Span<byte> destination, uint value, int minBytesToWrite, out int written)
		{
			if(!TryWriteUInt32(destination: destination, value: value, minBytesToWrite: minBytesToWrite, written: out written))
				throw new ArgumentOutOfRangeException(nameof(destination), $"{nameof(destination)} is too small to contain an UInt32");
		}

		/// <summary>
		/// Method writes 32-bit signed integer (<see cref="int"/>) to byte array
		/// </summary>
		/// <param name="destination">Byte array to write <paramref name="value"/></param>
		/// <param name="value">Value to serialize</param>
		/// <param name="written">Number of bytes written</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to hold <paramref name="value"/></exception>
		public static void WriteInt32(Span<byte> destination, int value, out int written)
		{
			if(!TryWriteInt32(destination: destination, value: value, written: out written))
				throw new ArgumentOutOfRangeException(nameof(destination), $"{nameof(destination)} is too small to contain an Int32");
		}

		/// <summary>
		/// Method writes 32-bit signed integer (<see cref="int"/>) to byte array
		/// </summary>
		/// <param name="destination">Byte array to write <paramref name="value"/></param>
		/// <param name="value">Value to serialize</param>
		/// <param name="minBytesToWrite">Minimum number of bytes to write to <paramref name="destination"/>. It has to be less or equal to 5</param>
		/// <param name="written">Number of bytes written</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to hold <paramref name="value"/></exception>
		/// <exception cref="ArgumentException"><paramref name="minBytesToWrite"/> is too big</exception>
		public static void WriteInt32(Span<byte> destination, int value, int minBytesToWrite, out int written)
		{
			if(!TryWriteInt32(destination: destination, value: value, minBytesToWrite: minBytesToWrite, written: out written))
				throw new ArgumentOutOfRangeException(nameof(destination), $"{nameof(destination)} is too small to contain an Int32");
		}

		/// <summary>
		/// Method writes 32-bit signed integer (<see cref="int"/>) to byte array with ZigZag coding (sign bit as the least significant bit)
		/// </summary>
		/// <param name="destination">Byte array to write <paramref name="value"/></param>
		/// <param name="value">Value to serialize</param>
		/// <param name="written">Number of bytes written</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to hold <paramref name="value"/></exception>
		public static void WriteInt32ZigZag(Span<byte> destination, int value, out int written)
		{
			if(!TryWriteInt32ZigZag(destination: destination, value: value, written: out written))
				throw new ArgumentOutOfRangeException(nameof(destination), $"{nameof(destination)} is too small to contain an Int32");
		}

		/// <summary>
		/// Method writes 32-bit signed integer (<see cref="int"/>) to byte array with ZigZag coding (sign bit as the least significant bit)
		/// </summary>
		/// <param name="destination">Byte array to write <paramref name="value"/></param>
		/// <param name="value">Value to serialize</param>
		/// <param name="minBytesToWrite">Minimum number of bytes to write to <paramref name="destination"/>. It has to be less or equal to 5</param>
		/// <param name="written">Number of bytes written</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to hold <paramref name="value"/></exception>
		/// <exception cref="ArgumentException"><paramref name="minBytesToWrite"/> is too big</exception>
		public static void WriteInt32ZigZag(Span<byte> destination, int value, int minBytesToWrite, out int written)
		{
			if(!TryWriteInt32ZigZag(destination: destination, value: value, minBytesToWrite: minBytesToWrite, written: out written))
				throw new ArgumentOutOfRangeException(nameof(destination), $"{nameof(destination)} is too small to contain an Int32");
		}
		#endregion
		#region Read
		/// <summary>
		/// Method reads 32-bit unsigned integer (<see cref="uint"/>) from byte array
		/// </summary>
		/// <param name="source">Byte array from which read value</param>
		/// <param name="read">Number of bytes read</param>
		/// <returns>Read value</returns>
		/// <exception cref="ArgumentOutOfRangeException">End of <paramref name="source"/> was reached before whole value was read</exception>
		/// <exception cref="OverflowException">Read value is too big for <see cref="uint"/></exception>
		public static uint ReadUInt32(ReadOnlySpan<byte> source, out int read)
		{
			uint value;

			if(!TryReadUInt32(source: source, value: out value, read: out read))
				throw new ArgumentOutOfRangeException(nameof(source), $"{nameof(source)} is too small to contain an UInt32");
			return value;
		}

		/// <summary>
		/// Method reads 32-bit signed integer (<see cref="int"/>) from byte array
		/// </summary>
		/// <param name="source">Byte array from which read value</param>
		/// <param name="read">Number of bytes read</param>
		/// <returns>Read value</returns>
		/// <exception cref="ArgumentOutOfRangeException">End of <paramref name="source"/> was reached before whole value was read</exception>
		/// <exception cref="OverflowException">Read value is too big for <see cref="int"/></exception>
		public static int ReadInt32(ReadOnlySpan<byte> source, out int read)
		{
			int value;

			if(!TryReadInt32(source: source, value: out value, read: out read))
				throw new ArgumentOutOfRangeException(nameof(source), $"{nameof(source)} is too small to contain an Int32");
			return value;
		}

		/// <summary>
		/// Method reads 32-bit signed integer (<see cref="int"/>) from byte array ZigZag encoded (sign bit as the least significant bit)
		/// </summary>
		/// <param name="source">Byte array from which read value</param>
		/// <param name="read">Number of bytes read</param>
		/// <returns>Read value</returns>
		/// <exception cref="ArgumentOutOfRangeException">End of <paramref name="source"/> was reached before whole value was read</exception>
		/// <exception cref="OverflowException">Read value is too big for <see cref="int"/></exception>
		public static int ReadInt32ZigZag(ReadOnlySpan<byte> source, out int read)
		{
			int value;

			if(!TryReadInt32ZigZag(source: source, value: out value, read: out read))
				throw new ArgumentOutOfRangeException(nameof(source), $"{nameof(source)} is too small to contain an Int32");
			return value;
		}
		#endregion
	}
}