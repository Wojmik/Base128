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
			using(var ms = new System.IO.MemoryStream(serialized.Length))
			using(var binaryWriter = new BinaryWriterBase128(output: ms))
			{
				WriteStream(binaryWriter: binaryWriter, value: value);

				Assert.AreEqual(expected: serialized.Length, actual: binaryWriter.BaseStream.Position);

				binaryWriter.Flush();

				Assert.IsTrue(ms.ToArray().SequenceEqual(serialized));
			}
		}
	}
}