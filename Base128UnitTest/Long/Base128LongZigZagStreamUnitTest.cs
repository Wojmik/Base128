using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WojciechMikołajewicz.Base128UnitTest.Long
{
	[TestClass]
	public class Base128LongZigZagStreamUnitTest : Base128StreamUnitTestBase<long>
	{
		static IEnumerable<object[]> GetTestData()
		{
			return Base128LongZigZagUnitTest.GetTestData();
		}
		static IEnumerable<object[]> GetOverflowTestData()
		{
			return Base128LongZigZagUnitTest.GetOverflowTestData();
		}

		protected override long ReadStream(BinaryReaderBase128 binaryReader)
		{
			return binaryReader.ReadInt64Base128ZigZag();
		}

		protected override void WriteStream(BinaryWriterBase128 binaryWriter, long value)
		{
			binaryWriter.WriteBase128ZigZag(value: value);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void WriteInt64ZigZagStreamTestMethod(long value, byte[] serialized)
		{
			WriteStreamTestMethod(value: value, serialized: serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void WriteInt64ZigZagStreamLongerBufTestMethod(long value, byte[] serialized)
		{
			WriteStreamLongerBufTestMethod(value: value, serialized: serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void WriteInt64ZigZagStreamEndOfStreamTestMethod(long value, byte[] serialized)
		{
			WriteStreamEndOfStreamTestMethod(value: value, serialized: serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void ReadInt64ZigZagStreamTestMethod(long value, byte[] serialized)
		{
			ReadStreamTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void ReadInt64ZigZagStreamLongerBufTestMethod(long value, byte[] serialized)
		{
			ReadStreamLongerBufTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void ReadInt64ZigZagStreamEndOfStreamTestMethod(long value, byte[] serialized)
		{
			ReadStreamEndOfStreamTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetOverflowTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void ReadInt64ZigZagStreamOverflowTestMethod(byte[] serialized)
		{
			ReadStreamOverflowTestMethod(serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetOverflowTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void ReadInt64ZigZagStreamLongerBufOverflowTestMethod(byte[] serialized)
		{
			ReadStreamLongerBufOverflowTestMethod(serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetOverflowTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void ReadInt64ZigZagStreamEndOfStreamOverflowTestMethod(byte[] serialized)
		{
			ReadStreamEndOfStreamOverflowTestMethod(serialized);
		}
	}
}