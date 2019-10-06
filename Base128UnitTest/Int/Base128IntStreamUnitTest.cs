using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WojciechMikołajewicz.Base128UnitTest.Int
{
	[TestClass]
	public class Base128IntStreamUnitTest : Base128StreamUnitTestBase<int>
	{
		static IEnumerable<object[]> GetTestData()
		{
			return Base128IntUnitTest.GetTestData();
		}
		static IEnumerable<object[]> GetOverflowTestData()
		{
			return Base128IntUnitTest.GetOverflowTestData();
		}

		protected override int ReadStream(BinaryReaderBase128 binaryReader)
		{
			return binaryReader.ReadInt32Base128();
		}

		protected override void WriteStream(BinaryWriterBase128 binaryWriter, int value)
		{
			binaryWriter.WriteBase128(value: value);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void WriteInt32StreamTestMethod(int value, byte[] serialized)
		{
			WriteStreamTestMethod(value: value, serialized: serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void WriteInt32StreamLongerBufTestMethod(int value, byte[] serialized)
		{
			WriteStreamLongerBufTestMethod(value: value, serialized: serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void WriteInt32StreamEndOfStreamTestMethod(int value, byte[] serialized)
		{
			WriteStreamEndOfStreamTestMethod(value: value, serialized: serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void ReadInt32StreamTestMethod(int value, byte[] serialized)
		{
			ReadStreamTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void ReadInt32StreamLongerBufTestMethod(int value, byte[] serialized)
		{
			ReadStreamLongerBufTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void ReadInt32StreamEndOfStreamTestMethod(int value, byte[] serialized)
		{
			ReadStreamEndOfStreamTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetOverflowTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void ReadInt32StreamOverflowTestMethod(byte[] serialized)
		{
			ReadStreamOverflowTestMethod(serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetOverflowTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void ReadInt32StreamLongerBufOverflowTestMethod(byte[] serialized)
		{
			ReadStreamLongerBufOverflowTestMethod(serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetOverflowTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void ReadInt32StreamEndOfStreamOverflowTestMethod(byte[] serialized)
		{
			ReadStreamEndOfStreamOverflowTestMethod(serialized);
		}
	}
}