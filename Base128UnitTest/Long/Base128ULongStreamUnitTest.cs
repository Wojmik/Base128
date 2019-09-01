using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WojciechMikołajewicz.Base128UnitTest.Long
{
	[TestClass]
	public class Base128ULongStreamUnitTest : Base128StreamUnitTestBase<ulong>
	{
		static IEnumerable<object[]> GetTestData()
		{
			return Base128ULongUnitTest.GetTestData();
		}

		protected override ulong ReadStream(BinaryReaderBase128 binaryReader)
		{
			return binaryReader.ReadUInt64Base128();
		}

		protected override void WriteStream(BinaryWriterBase128 binaryWriter, ulong value)
		{
			binaryWriter.WriteBase128(value: value);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void WriteUInt64StreamTestMethod(ulong value, byte[] serialized)
		{
			WriteStreamTestMethod(value: value, serialized: serialized);
		}
	}
}