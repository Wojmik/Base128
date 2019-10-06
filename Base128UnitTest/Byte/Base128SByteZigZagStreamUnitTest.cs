using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WojciechMikołajewicz.Base128UnitTest.Byte
{
	[TestClass]
	public class Base128SByteZigZagStreamUnitTest : Base128StreamUnitTestBase<sbyte>
	{
		static IEnumerable<object[]> GetTestData()
		{
			return Base128SByteZigZagUnitTest.GetTestData();
		}
		static IEnumerable<object[]> GetOverflowTestData()
		{
			return Base128SByteZigZagUnitTest.GetOverflowTestData();
		}

		protected override sbyte ReadStream(BinaryReaderBase128 binaryReader)
		{
			return binaryReader.ReadInt8Base128ZigZag();
		}

		protected override void WriteStream(BinaryWriterBase128 binaryWriter, sbyte value)
		{
			binaryWriter.WriteBase128ZigZag(value: value);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void WriteInt8ZigZagStreamTestMethod(sbyte value, byte[] serialized)
		{
			WriteStreamTestMethod(value: value, serialized: serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void WriteInt8ZigZagStreamLongerBufTestMethod(sbyte value, byte[] serialized)
		{
			WriteStreamLongerBufTestMethod(value: value, serialized: serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void WriteInt8ZigZagStreamEndOfStreamTestMethod(sbyte value, byte[] serialized)
		{
			WriteStreamEndOfStreamTestMethod(value: value, serialized: serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void ReadInt8ZigZagStreamTestMethod(sbyte value, byte[] serialized)
		{
			ReadStreamTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void ReadInt8ZigZagStreamLongerBufTestMethod(sbyte value, byte[] serialized)
		{
			ReadStreamLongerBufTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void ReadInt8ZigZagStreamEndOfStreamTestMethod(sbyte value, byte[] serialized)
		{
			ReadStreamEndOfStreamTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetOverflowTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void ReadInt8ZigZagStreamOverflowTestMethod(byte[] serialized)
		{
			ReadStreamOverflowTestMethod(serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetOverflowTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void ReadInt8ZigZagStreamLongerBufOverflowTestMethod(byte[] serialized)
		{
			ReadStreamLongerBufOverflowTestMethod(serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetOverflowTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void ReadInt8ZigZagStreamEndOfStreamOverflowTestMethod(byte[] serialized)
		{
			ReadStreamEndOfStreamOverflowTestMethod(serialized);
		}
	}
}