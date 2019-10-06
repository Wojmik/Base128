using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WojciechMikołajewicz.Base128UnitTest.Byte
{
	[TestClass]
	public class Base128ByteStreamUnitTest : Base128StreamUnitTestBase<byte>
	{
		static IEnumerable<object[]> GetTestData()
		{
			return Base128ByteUnitTest.GetTestData();
		}
		static IEnumerable<object[]> GetOverflowTestData()
		{
			return Base128ByteUnitTest.GetOverflowTestData();
		}

		protected override byte ReadStream(BinaryReaderBase128 binaryReader)
		{
			return binaryReader.ReadUInt8Base128();
		}

		protected override void WriteStream(BinaryWriterBase128 binaryWriter, byte value)
		{
			binaryWriter.WriteBase128(value: value);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void WriteUInt8StreamTestMethod(byte value, byte[] serialized)
		{
			WriteStreamTestMethod(value: value, serialized: serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void WriteUInt8StreamLongerBufTestMethod(byte value, byte[] serialized)
		{
			WriteStreamLongerBufTestMethod(value: value, serialized: serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void WriteUInt8StreamEndOfStreamTestMethod(byte value, byte[] serialized)
		{
			WriteStreamEndOfStreamTestMethod(value: value, serialized: serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void ReadUInt8StreamTestMethod(byte value, byte[] serialized)
		{
			ReadStreamTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void ReadUInt8StreamLongerBufTestMethod(byte value, byte[] serialized)
		{
			ReadStreamLongerBufTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void ReadUInt8StreamEndOfStreamTestMethod(byte value, byte[] serialized)
		{
			ReadStreamEndOfStreamTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetOverflowTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void ReadUInt8StreamOverflowTestMethod(byte[] serialized)
		{
			ReadStreamOverflowTestMethod(serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetOverflowTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void ReadUInt8StreamLongerBufOverflowTestMethod(byte[] serialized)
		{
			ReadStreamLongerBufOverflowTestMethod(serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetOverflowTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void ReadUInt8StreamEndOfStreamOverflowTestMethod(byte[] serialized)
		{
			ReadStreamEndOfStreamOverflowTestMethod(serialized);
		}
	}
}