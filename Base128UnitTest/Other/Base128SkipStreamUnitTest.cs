using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WojciechMikołajewicz.Base128UnitTest.Other
{
	[TestClass]
	public class Base128SkipStreamUnitTest
	{
		public static IEnumerable<object[]> GetTestData()
		{
			return Base128SkipUnitTest.GetTestData();
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public virtual void SkipStreamTestMethod(byte[] serialized)
		{
			using(var ms = new MemoryStream(serialized))
			using(var binaryReader = new BinaryReaderBase128(input: ms))
			{
				binaryReader.SkipBase128Value();

				Assert.AreEqual(expected: serialized.Length, actual: binaryReader.BaseStream.Position);
			}
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public virtual void SkipStreamLongerStreamTestMethod(byte[] serialized)
		{
			byte[] buf = new byte[serialized.Length+1];

			serialized.AsSpan().CopyTo(buf);

			using(var ms = new MemoryStream(buf))
			using(var binaryReader = new BinaryReaderBase128(input: ms))
			{
				binaryReader.SkipBase128Value();

				Assert.AreEqual(expected: serialized.Length, actual: binaryReader.BaseStream.Position);
			}
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public virtual void SkipStreamEndOfStreamTestMethod(byte[] serialized)
		{
			byte[] buf = new byte[serialized.Length-1];

			serialized.AsSpan(0, buf.Length).CopyTo(buf);

			using(var ms = new MemoryStream(buf))
			using(var binaryReader = new BinaryReaderBase128(input: ms))
			{
				Assert.ThrowsException<EndOfStreamException>(() => binaryReader.SkipBase128Value());

				Assert.AreEqual(expected: buf.Length, actual: binaryReader.BaseStream.Position);
			}
		}
	}
}