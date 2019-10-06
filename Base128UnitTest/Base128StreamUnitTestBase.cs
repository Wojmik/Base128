using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WojciechMikołajewicz.Base128UnitTest
{
	public abstract class Base128StreamUnitTestBase<T>
		where T : struct
	{
		protected abstract void WriteStream(BinaryWriterBase128 binaryWriter, T value);

		protected abstract T ReadStream(BinaryReaderBase128 binaryReader);

		public virtual void WriteStreamTestMethod(T value, byte[] serialized)
		{
			using(var ms = new MemoryStream(new byte[serialized.Length]))
			using(var binaryWriter = new BinaryWriterBase128(output: ms))
			{
				WriteStream(binaryWriter: binaryWriter, value: value);

				Assert.AreEqual(expected: serialized.Length, actual: binaryWriter.BaseStream.Position);

				binaryWriter.Flush();

				Assert.IsTrue(ms.ToArray().SequenceEqual(serialized));
			}
		}

		public virtual void WriteStreamLongerBufTestMethod(T value, byte[] serialized)
		{
			using(var ms = new MemoryStream(new byte[serialized.Length+1]))
			using(var binaryWriter = new BinaryWriterBase128(output: ms))
			{
				WriteStream(binaryWriter: binaryWriter, value: value);

				Assert.AreEqual(expected: serialized.Length, actual: binaryWriter.BaseStream.Position);

				binaryWriter.Flush();

				Assert.IsTrue(ms.ToArray().Take(serialized.Length).SequenceEqual(serialized));
			}
		}

		public virtual void WriteStreamEndOfStreamTestMethod(T value, byte[] serialized)
		{
			using(var ms = new MemoryStream(new byte[serialized.Length-1]))
			using(var binaryWriter = new BinaryWriterBase128(output: ms))
			{
				Assert.ThrowsException<NotSupportedException>(() => WriteStream(binaryWriter: binaryWriter, value: value));
			}
		}

		public virtual void ReadStreamTestMethod(T value, byte[] serialized)
		{
			T readValue;

			using(var ms = new MemoryStream(serialized))
			using(var binaryReader = new BinaryReaderBase128(input: ms))
			{
				readValue=ReadStream(binaryReader: binaryReader);

				Assert.AreEqual(expected: serialized.Length, actual: binaryReader.BaseStream.Position);
			}

			Assert.AreEqual(expected: value, actual: readValue);
		}

		public virtual void ReadStreamLongerBufTestMethod(T value, byte[] serialized)
		{
			byte[] buf = new byte[serialized.Length+1];
			T readValue;

			serialized.AsSpan().CopyTo(buf);

			using(var ms = new MemoryStream(buf))
			using(var binaryReader = new BinaryReaderBase128(input: ms))
			{
				readValue=ReadStream(binaryReader: binaryReader);

				Assert.AreEqual(expected: serialized.Length, actual: binaryReader.BaseStream.Position);
			}

			Assert.AreEqual(expected: value, actual: readValue);
		}

		public virtual void ReadStreamEndOfStreamTestMethod(T value, byte[] serialized)
		{
			byte[] buf = new byte[serialized.Length-1];

			serialized.AsSpan(0, buf.Length).CopyTo(buf);

			using(var ms = new MemoryStream(buf))
			using(var binaryReader = new BinaryReaderBase128(input: ms))
			{
				Assert.ThrowsException<EndOfStreamException>(() => ReadStream(binaryReader: binaryReader));

				Assert.AreEqual(expected: buf.Length, actual: binaryReader.BaseStream.Position);
			}
		}

		public virtual void ReadStreamOverflowTestMethod(byte[] serialized)
		{
			using(var ms = new MemoryStream(serialized))
			using(var binaryReader = new BinaryReaderBase128(input: ms))
			{
				Assert.ThrowsException<OverflowException>(() => ReadStream(binaryReader: binaryReader));

				//Should set Position after whole overflowed value
				Assert.AreEqual(expected: serialized.Length, actual: binaryReader.BaseStream.Position);
			}
		}

		public virtual void ReadStreamLongerBufOverflowTestMethod(byte[] serialized)
		{
			byte[] buf = new byte[serialized.Length+1];

			serialized.AsSpan().CopyTo(buf);

			using(var ms = new MemoryStream(buf))
			using(var binaryReader = new BinaryReaderBase128(input: ms))
			{
				Assert.ThrowsException<OverflowException>(() => ReadStream(binaryReader: binaryReader));

				//Should set Position after whole overflowed value
				Assert.AreEqual(expected: serialized.Length, actual: binaryReader.BaseStream.Position);
			}
		}

		public virtual void ReadStreamEndOfStreamOverflowTestMethod(byte[] serialized)
		{
			byte[] buf = new byte[serialized.Length-1];

			serialized.AsSpan(0, buf.Length).CopyTo(buf);

			using(var ms = new MemoryStream(buf))
			using(var binaryReader = new BinaryReaderBase128(input: ms))
			{
				Assert.ThrowsException<EndOfStreamException>(() => ReadStream(binaryReader: binaryReader));

				//Should set Position after whole overflowed value but reached end of stream
				Assert.AreEqual(expected: buf.Length, actual: binaryReader.BaseStream.Position);
			}
		}
	}
}