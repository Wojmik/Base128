using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WojciechMikołajewicz
{
	/// <summary>
	/// Reads primitive data types as binary values in a specific encoding.
	/// </summary>
	public class BinaryReaderBase128 : BinaryReader
	{
		/// <summary>
		/// Initializes a new instance of the System.IO.BinaryReader class based on the specified stream and using UTF-8 encoding.
		/// </summary>
		/// <param name="input">The input stream.</param>
		/// <exception cref="ArgumentException">The stream does not support reading, is null, or is already closed.</exception>
		public BinaryReaderBase128(Stream input)
			: base(input)
		{ }

		/// <summary>
		/// Initializes a new instance of the System.IO.BinaryReader class based on the specified stream and character encoding.
		/// </summary>
		/// <param name="input">The input stream.</param>
		/// <param name="encoding">The character encoding to use.</param>
		/// <exception cref="ArgumentException">The stream does not support reading, is null, or is already closed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="encoding"/> is null.</exception>
		public BinaryReaderBase128(Stream input, Encoding encoding)
			: base(input, encoding)
		{ }

		/// <summary>
		/// Initializes a new instance of the System.IO.BinaryReader class based on the specified stream and character encoding, and optionally leaves the stream open.
		/// </summary>
		/// <param name="input">The input stream.</param>
		/// <param name="encoding">The character encoding to use.</param>
		/// <param name="leaveOpen">true to leave the stream open after the System.IO.BinaryReader object is disposed; otherwise, false.</param>
		/// <exception cref="ArgumentException">The stream does not support reading, is null, or is already closed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="encoding"/> or <paramref name="input"/> is null.</exception>
		public BinaryReaderBase128(Stream input, Encoding encoding, bool leaveOpen)
			: base(input, encoding, leaveOpen)
		{ }

		/// <summary>
		/// Reads an 8-byte unsigned integer from the current stream as variable length integer and advances the position of the stream.
		/// </summary>
		/// <returns>An 8-byte unsigned integer read from this stream.</returns>
		/// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		/// <exception cref="ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="OverflowException">Read value is too big for <see cref="ulong"/></exception>
		public virtual ulong ReadUInt64Base128()
		{
			int val, read = 0;
			ulong value = 0;
			
			unchecked
			{
				while(read<9*7)//Eventually last (tenth) byte treat special
				{
					value>>=7;
					val=this.ReadByte();
					read+=7;
					value|=(ulong)val<<57;//Shift val to the most significant byte and one bit more to cut overflow bit

					if(0<=(sbyte)val)
						return value>>(64-read);
					else if(val<0)
						throw new EndOfStreamException();
				}

				//If we are here it is the last (tenth) byte. There could be only one bit (64-9*7)
				val=this.ReadByte();
				if(0!=(val&-2))//-2 = 0xFFFFFFFE
				{
					//End of stream or overflow detected
					while(128<=val)//Read the rest of the overflowed value - to set Position after whole Base128 value
					{
						val=this.ReadByte();
					}
					if(val<0)//End of stream
						throw new EndOfStreamException();
					throw new OverflowException($"Value in stream is too big or too small");
				}
				return (value>>1)|(ulong)val<<63;
			}
		}

		/// <summary>
		/// Reads an 8-byte signed integer from the current stream as variable length integer and advances the position of the stream.
		/// </summary>
		/// <returns>An 8-byte signed integer read from this stream.</returns>
		/// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		/// <exception cref="ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="OverflowException">Read value is too big for <see cref="long"/></exception>
		public virtual long ReadInt64Base128()
		{
			int val, read = 0;
			long value = 0;

			unchecked
			{
				while(read<9*7)//Eventually last (tenth) byte treat special
				{
					value=(long)((ulong)value>>7);//Rotate unsigned - the most significant 7 bits have to be zeros. Next operation will be OR on those bits
					val=this.ReadByte();
					read+=7;
					value|=(long)val<<57;//Shift val to the most significant byte and one bit more to cut overflow bit

					if(0<=(sbyte)val)
						return value>>(64-read);
					else if(val<0)
						throw new EndOfStreamException();
				}

				//If we are here it is the last (tenth) byte. There could be only one bit (64-9*7)
				val=this.ReadByte();
				if((uint)(val<<31>>6)>>25!=(uint)val)//val can be only 0b0000_0000 or 0b0111_1111
				{
					//End of stream or overflow detected
					while(128<=val)//Read the rest of the overflowed value - to set Position after whole Base128 value
					{
						val=this.ReadByte();
					}
					if(val<0)//End of stream
						throw new EndOfStreamException();
					throw new OverflowException($"Value in stream is too big or too small");
				}
				return (long)(((ulong)value>>1)|(ulong)val<<63);//Rotate unsigned - the most significant bit has to be zero. Next operation is OR on this bit
			}
		}

		/// <summary>
		/// Reads an 8-byte signed integer from the current stream ZigZag encoded (sign bit as the least significant bit) as variable length integer and advances the position of the stream.
		/// </summary>
		/// <returns>An 8-byte signed integer read from this stream.</returns>
		/// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		/// <exception cref="ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="OverflowException">Read value is too big for <see cref="long"/></exception>
		public virtual long ReadInt64Base128ZigZag()
		{
			ulong uValue;

			uValue=this.ReadUInt64Base128();
			return (long)(uValue>>1)^-(long)(uValue&1);
		}

		/// <summary>
		/// Reads an 4-byte unsigned integer from the current stream as variable length integer and advances the position of the stream.
		/// </summary>
		/// <returns>An 4-byte unsigned integer read from this stream.</returns>
		/// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		/// <exception cref="ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="OverflowException">Read value is too big for <see cref="uint"/></exception>
		public virtual uint ReadUInt32Base128()
		{
			int val, read = 0;
			uint value = 0;

			unchecked
			{
				while(read<4*7)//Eventually last (fifth) byte treat special
				{
					value>>=7;
					val=this.ReadByte();
					read+=7;
					value|=(uint)val<<25;//Shift val to the most significant byte and one bit more to cut overflow bit

					if(0<=(sbyte)val)
						return value>>(32-read);
					else if(val<0)
						throw new EndOfStreamException();
				}

				//If we are here it is the last (fifth) byte. There could be only four bits (32-4*7)
				val=this.ReadByte();
				if(0!=(val&-16))//-16 = 0xFFFFFFF0
				{
					//End of stream or overflow detected
					while(128<=val)//Read the rest of the overflowed value - to set Position after whole Base128 value
					{
						val=this.ReadByte();
					}
					if(val<0)//End of stream
						throw new EndOfStreamException();
					throw new OverflowException($"Value in stream is too big or too small");
				}
				return (value>>4)|(uint)val<<28;
			}
		}

		/// <summary>
		/// Reads an 4-byte signed integer from the current stream as variable length integer and advances the position of the stream.
		/// </summary>
		/// <returns>An 4-byte signed integer read from this stream.</returns>
		/// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		/// <exception cref="ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="OverflowException">Read value is too big for <see cref="int"/></exception>
		public virtual int ReadInt32Base128()
		{
			int val, read = 0, value = 0;

			unchecked
			{
				while(read<4*7)//Eventually last (fifth) byte treat special
				{
					value=(int)((uint)value>>7);//Rotate unsigned - the most significant 7 bits have to be zeros. Next operation will be OR on those bits
					val=this.ReadByte();
					read+=7;
					value|=val<<25;//Shift val to the most significant byte and one bit more to cut overflow bit

					if(0<=(sbyte)val)
						return value>>(32-read);
					else if(val<0)
						throw new EndOfStreamException();
				}

				//If we are here it is the last (fifth) byte. There could be only four significant bits (32-4*7)
				val=this.ReadByte();
				if((uint)(val<<28>>3)>>25!=(uint)val)//val can be only 0b0000_0xxx or 0b0111_1xxx
				{
					//End of stream or overflow detected
					while(128<=val)//Read the rest of the overflowed value - to set Position after whole Base128 value
					{
						val=this.ReadByte();
					}
					if(val<0)//End of stream
						throw new EndOfStreamException();
					throw new OverflowException($"Value in stream is too big or too small");
				}
				return (int)(((uint)value>>4)|(uint)val<<28);//Rotate unsigned - the most significant bits have to be zeros. Next operation is OR on those bits
			}
		}

		/// <summary>
		/// Reads an 4-byte signed integer from the current stream ZigZag encoded (sign bit as the least significant bit) as variable length integer and advances the position of the stream.
		/// </summary>
		/// <returns>An 4-byte signed integer read from this stream.</returns>
		/// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		/// <exception cref="ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="OverflowException">Read value is too big for <see cref="int"/></exception>
		public virtual int ReadInt32Base128ZigZag()
		{
			uint uValue;

			uValue=this.ReadUInt32Base128();
			return (int)(uValue>>1)^-(int)(uValue&1);
		}

		/// <summary>
		/// Reads an 2-byte unsigned integer from the current stream as variable length integer and advances the position of the stream.
		/// </summary>
		/// <returns>An 2-byte unsigned integer read from this stream.</returns>
		/// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		/// <exception cref="ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="OverflowException">Read value is too big for <see cref="ushort"/></exception>
		public virtual ushort ReadUInt16Base128()
		{
			uint value;

			value=this.ReadUInt32Base128();
			checked
			{
				return (ushort)value;
			}
		}

		/// <summary>
		/// Reads an 2-byte signed integer from the current stream as variable length integer and advances the position of the stream.
		/// </summary>
		/// <returns>An 2-byte signed integer read from this stream.</returns>
		/// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		/// <exception cref="ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="OverflowException">Read value is too big for <see cref="short"/></exception>
		public virtual short ReadInt16Base128()
		{
			int value;

			value=this.ReadInt32Base128();
			checked
			{
				return (short)value;
			}
		}

		/// <summary>
		/// Reads an 2-byte signed integer from the current stream ZigZag encoded (sign bit as the least significant bit) as variable length integer and advances the position of the stream.
		/// </summary>
		/// <returns>An 2-byte signed integer read from this stream.</returns>
		/// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		/// <exception cref="ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="OverflowException">Read value is too big for <see cref="short"/></exception>
		public virtual short ReadInt16Base128ZigZag()
		{
			int value;

			value=this.ReadInt32Base128ZigZag();
			checked
			{
				return (short)value;
			}
		}

		/// <summary>
		/// Reads an 1-byte unsigned integer from the current stream as variable length integer and advances the position of the stream.
		/// </summary>
		/// <returns>An 1-byte unsigned integer read from this stream.</returns>
		/// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		/// <exception cref="ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="OverflowException">Read value is too big for <see cref="byte"/></exception>
		public virtual byte ReadUInt8Base128()
		{
			uint value;

			value=this.ReadUInt32Base128();
			checked
			{
				return (byte)value;
			}
		}

		/// <summary>
		/// Reads an 1-byte signed integer from the current stream as variable length integer and advances the position of the stream.
		/// </summary>
		/// <returns>An 1-byte signed integer read from this stream.</returns>
		/// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		/// <exception cref="ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="OverflowException">Read value is too big for <see cref="sbyte"/></exception>
		public virtual sbyte ReadInt8Base128()
		{
			int value;

			value=this.ReadInt32Base128();
			checked
			{
				return (sbyte)value;
			}
		}

		/// <summary>
		/// Reads an 1-byte signed integer from the current stream ZigZag encoded (sign bit as the least significant bit) as variable length integer and advances the position of the stream.
		/// </summary>
		/// <returns>An 1-byte signed integer read from this stream.</returns>
		/// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		/// <exception cref="ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="OverflowException">Read value is too big for <see cref="sbyte"/></exception>
		public virtual sbyte ReadInt8Base128ZigZag()
		{
			int value;

			value=this.ReadInt32Base128ZigZag();
			checked
			{
				return (sbyte)value;
			}
		}

		/// <summary>
		/// Skips Base128 variable integer value on the current stream
		/// </summary>
		/// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		/// <exception cref="ObjectDisposedException">The stream is closed.</exception>
		public virtual void SkipBase128Value()
		{
			int val;

			do
			{
				val=this.ReadByte();
			}
			while(128<=val);//This avoids endless loop if val is -1 (end of stream)
			if(val<0)//End of stream
				throw new EndOfStreamException();
		}
	}
}