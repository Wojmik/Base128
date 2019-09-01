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
		/// <exception cref="ArgumentNullException">encoding is null.</exception>
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
		/// <exception cref="ArgumentNullException">encoding or input is null.</exception>
		public BinaryReaderBase128(Stream input, Encoding encoding, bool leaveOpen)
			: base(input, encoding, leaveOpen)
		{ }

		/// <summary>
		/// Reads an 8-byte unsigned integer from the current stream as variable length integer and advances the position of the stream by eight bytes.
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
					val=this.BaseStream.ReadByte();
					read+=7;
					value|=(ulong)val<<57;//Shift val to the most significant byte and one bit more to cut overflow bit

					if(0<=(sbyte)val)
						return value>>(64-read);
					else if(val==-1)
						throw new EndOfStreamException();
				}

				//If we are here it is the last (tenth) byte. There could be only one bit (64-9*7)
				val=this.BaseStream.ReadByte();
				if(0!=(val&-2))//-2 = 0xFFFFFFFE
					if(val==-1)
						throw new EndOfStreamException();
					else
						throw new OverflowException($"Value in stream is too big or too small");
				return (value>>1)|(ulong)val<<63;
			}
		}
	}
}