using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WojciechMikołajewicz.Base128UnitTest
{
	public abstract class Base128StreamUnitTestBase<T>
	{
		protected abstract void WriteStream(BinaryWriterBase128 binaryWriter, T value);

		protected abstract T ReadStream(BinaryReaderBase128 binaryReader);

		public virtual void WriteStreamTestMethod(T value, byte[] serialized)
		{
			using(var ms = new System.IO.MemoryStream(new byte[serialized.Length]))
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
			using(var ms = new System.IO.MemoryStream(new byte[serialized.Length+1]))
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
			using(var ms = new System.IO.MemoryStream(new byte[serialized.Length-1]))
			using(var binaryWriter = new BinaryWriterBase128(output: ms))
			{
				Assert.ThrowsException<NotSupportedException>(() => WriteStream(binaryWriter: binaryWriter, value: value));
			}
		}
	}
}