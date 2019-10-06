using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WojciechMikołajewicz.Base128UnitTest.Short
{
	[TestClass]
	public class Base128ShortZigZagStreamUnitTest : Base128StreamUnitTestBase<short>
	{
		static IEnumerable<object[]> GetTestData()
		{
			return Base128ShortZigZagUnitTest.GetTestData();
		}
		static IEnumerable<object[]> GetOverflowTestData()
		{
			return Base128ShortZigZagUnitTest.GetOverflowTestData();
		}

		protected override short ReadStream(BinaryReaderBase128 binaryReader)
		{
			return binaryReader.ReadInt16Base128ZigZag();
		}

		protected override void WriteStream(BinaryWriterBase128 binaryWriter, short value)
		{
			binaryWriter.WriteBase128ZigZag(value: value);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void WriteInt16ZigZagStreamTestMethod(short value, byte[] serialized)
		{
			WriteStreamTestMethod(value: value, serialized: serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void WriteInt16ZigZagStreamLongerBufTestMethod(short value, byte[] serialized)
		{
			WriteStreamLongerBufTestMethod(value: value, serialized: serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void WriteInt16ZigZagStreamEndOfStreamTestMethod(short value, byte[] serialized)
		{
			WriteStreamEndOfStreamTestMethod(value: value, serialized: serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void ReadInt16ZigZagStreamTestMethod(short value, byte[] serialized)
		{
			ReadStreamTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void ReadInt16ZigZagStreamLongerBufTestMethod(short value, byte[] serialized)
		{
			ReadStreamLongerBufTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void ReadInt16ZigZagStreamEndOfStreamTestMethod(short value, byte[] serialized)
		{
			ReadStreamEndOfStreamTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetOverflowTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void ReadInt16ZigZagStreamOverflowTestMethod(byte[] serialized)
		{
			ReadStreamOverflowTestMethod(serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetOverflowTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void ReadInt16ZigZagStreamLongerBufOverflowTestMethod(byte[] serialized)
		{
			ReadStreamLongerBufOverflowTestMethod(serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetOverflowTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void ReadInt16ZigZagStreamEndOfStreamOverflowTestMethod(byte[] serialized)
		{
			ReadStreamEndOfStreamOverflowTestMethod(serialized);
		}
	}
}