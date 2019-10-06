using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WojciechMikołajewicz
{
	/// <summary>
	/// Writes primitive types in binary to a stream and supports writing strings in a specific encoding.
	/// </summary>
	public class BinaryWriterBase128 : BinaryWriter
	{
		/// <summary>
		/// Internal buffer for serializing
		/// </summary>
		private readonly byte[] Buf = new byte[10];

		/// <summary>
		/// Initializes a new instance of the System.IO.BinaryWriter class based on the specified stream and using UTF-8 encoding.
		/// </summary>
		/// <param name="output">The output stream.</param>
		/// <exception cref="ArgumentException">The stream does not support writing or is already closed.</exception>
		/// <exception cref="ArgumentNullException">output is null.</exception>
		public BinaryWriterBase128(Stream output)
			: base(output)
		{ }

		/// <summary>
		/// Initializes a new instance of the System.IO.BinaryWriter class based on the specified stream and character encoding.
		/// </summary>
		/// <param name="output">The output stream.</param>
		/// <param name="encoding">The character encoding to use.</param>
		/// <exception cref="ArgumentException">The stream does not support writing or is already closed.</exception>
		/// <exception cref="ArgumentNullException">output or encoding is null.</exception>
		public BinaryWriterBase128(Stream output, Encoding encoding)
			: base(output, encoding)
		{ }

		/// <summary>
		/// Initializes a new instance of the System.IO.BinaryWriter class based on the specified stream and character encoding, and optionally leaves the stream open.
		/// </summary>
		/// <param name="output">The output stream.</param>
		/// <param name="encoding">The character encoding to use.</param>
		/// <param name="leaveOpen">true to leave the stream open after the System.IO.BinaryWriter object is disposed; otherwise, false.</param>
		/// <exception cref="ArgumentException">The stream does not support writing or is already closed.</exception>
		/// <exception cref="ArgumentNullException">output or encoding is null.</exception>
		public BinaryWriterBase128(Stream output, Encoding encoding, bool leaveOpen)
			: base(output, encoding, leaveOpen)
		{ }

		/// <summary>
		/// Writes an eight-byte unsigned integer to the current stream as variable length integer and advances the stream position by appropriate number of bytes.
		/// </summary>
		/// <param name="value">The eight-byte unsigned integer to write.</param>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		/// <exception cref="ObjectDisposedException">The stream is closed.</exception>
		public virtual void WriteBase128(ulong value)
		{
			int written;
			
			Base128.WriteUInt64(destination: this.Buf, value: value, written: out written);
			this.OutStream.Write(Buf, 0, written);
		}

		/// <summary>
		/// Writes an eight-byte signed integer to the current stream as variable length integer and advances the stream position by appropriate number of bytes.
		/// </summary>
		/// <param name="value">The eight-byte signed integer to write.</param>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		/// <exception cref="ObjectDisposedException">The stream is closed.</exception>
		public virtual void WriteBase128(long value)
		{
			int written;

			Base128.WriteInt64(destination: this.Buf, value: value, written: out written);
			this.OutStream.Write(Buf, 0, written);
		}

		/// <summary>
		/// Writes an eight-byte signed integer to the current stream as variable length integer ZgiZag encoded and advances the stream position by appropriate number of bytes.
		/// </summary>
		/// <param name="value">The eight-byte signed integer to write.</param>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		/// <exception cref="ObjectDisposedException">The stream is closed.</exception>
		public virtual void WriteBase128ZigZag(long value)
		{
			int written;

			Base128.WriteInt64ZigZag(destination: this.Buf, value: value, written: out written);
			this.OutStream.Write(Buf, 0, written);
		}

		/// <summary>
		/// Writes a four-byte unsigned integer to the current stream as variable length integer and advances the stream position by appropriate number of bytes.
		/// </summary>
		/// <param name="value">The four-byte unsigned integer to write.</param>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		/// <exception cref="ObjectDisposedException">The stream is closed.</exception>
		public virtual void WriteBase128(uint value)
		{
			int written;
			
			Base128.WriteUInt32(destination: this.Buf, value: value, written: out written);
			this.OutStream.Write(Buf, 0, written);
		}

		/// <summary>
		/// Writes a four-byte signed integer to the current stream as variable length integer and advances the stream position by appropriate number of bytes.
		/// </summary>
		/// <param name="value">The four-byte signed integer to write.</param>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		/// <exception cref="ObjectDisposedException">The stream is closed.</exception>
		public virtual void WriteBase128(int value)
		{
			int written;

			Base128.WriteInt32(destination: this.Buf, value: value, written: out written);
			this.OutStream.Write(Buf, 0, written);
		}

		/// <summary>
		/// Writes a four-byte signed integer to the current stream as variable length integer ZgiZag encoded and advances the stream position by appropriate number of bytes.
		/// </summary>
		/// <param name="value">The four-byte signed integer to write.</param>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		/// <exception cref="ObjectDisposedException">The stream is closed.</exception>
		public virtual void WriteBase128ZigZag(int value)
		{
			int written;

			Base128.WriteInt32ZigZag(destination: this.Buf, value: value, written: out written);
			this.OutStream.Write(Buf, 0, written);
		}

		/// <summary>
		/// Writes a two-byte unsigned integer to the current stream as variable length integer and advances the stream position by appropriate number of bytes.
		/// </summary>
		/// <param name="value">The two-byte unsigned integer to write.</param>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		/// <exception cref="ObjectDisposedException">The stream is closed.</exception>
		public virtual void WriteBase128(ushort value)
		{
			int written;

			Base128.WriteUInt32(destination: this.Buf, value: value, written: out written);
			this.OutStream.Write(Buf, 0, written);
		}

		/// <summary>
		/// Writes a two-byte signed integer to the current stream as variable length integer and advances the stream position by appropriate number of bytes.
		/// </summary>
		/// <param name="value">The two-byte signed integer to write.</param>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		/// <exception cref="ObjectDisposedException">The stream is closed.</exception>
		public virtual void WriteBase128(short value)
		{
			int written;

			Base128.WriteInt32(destination: this.Buf, value: value, written: out written);
			this.OutStream.Write(Buf, 0, written);
		}

		/// <summary>
		/// Writes a two-byte signed integer to the current stream as variable length integer ZgiZag encoded and advances the stream position by appropriate number of bytes.
		/// </summary>
		/// <param name="value">The two-byte signed integer to write.</param>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		/// <exception cref="ObjectDisposedException">The stream is closed.</exception>
		public virtual void WriteBase128ZigZag(short value)
		{
			int written;

			Base128.WriteInt32ZigZag(destination: this.Buf, value: value, written: out written);
			this.OutStream.Write(Buf, 0, written);
		}

		/// <summary>
		/// Writes an unsigned byte to the current stream as variable length integer and advances the stream position by appropriate number of bytes.
		/// </summary>
		/// <param name="value">The unsigned byte to write.</param>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		/// <exception cref="ObjectDisposedException">The stream is closed.</exception>
		public virtual void WriteBase128(byte value)
		{
			int written;
			
			Base128.WriteUInt32(destination: this.Buf, value: value, written: out written);
			this.OutStream.Write(Buf, 0, written);
		}

		/// <summary>
		/// Writes a signed byte to the current stream as variable length integer and advances the stream position by appropriate number of bytes.
		/// </summary>
		/// <param name="value">The signed byte to write.</param>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		/// <exception cref="ObjectDisposedException">The stream is closed.</exception>
		public virtual void WriteBase128(sbyte value)
		{
			int written;
			
			Base128.WriteInt32(destination: this.Buf, value: value, written: out written);
			this.OutStream.Write(Buf, 0, written);
		}

		/// <summary>
		/// Writes a signed byte to the current stream as variable length integer ZgiZag encoded and advances the stream position by appropriate number of bytes.
		/// </summary>
		/// <param name="value">The signed byte to write.</param>
		/// <exception cref="IOException">An I/O error occurs.</exception>
		/// <exception cref="ObjectDisposedException">The stream is closed.</exception>
		public virtual void WriteBase128ZigZag(sbyte value)
		{
			int written;

			Base128.WriteInt32ZigZag(destination: this.Buf, value: value, written: out written);
			this.OutStream.Write(Buf, 0, written);
		}
	}
}