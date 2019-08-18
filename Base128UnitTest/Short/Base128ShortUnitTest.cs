using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WojciechMikołajewicz.Base128UnitTest.Int;
using WojciechMikołajewicz.Base128UnitTest.Long;
using WojciechMikołajewicz.Base128UnitTest.Model;
using static WojciechMikołajewicz.Base128;

namespace WojciechMikołajewicz.Base128UnitTest.Short
{
	[TestClass]
	public class Base128ShortUnitTest
	{
		internal static OverflowTestSample[] OverflowTestData = new OverflowTestSample[]
			{
				new OverflowTestSample(new byte[] { 0x80, 0x80, 0x02, }),
				new OverflowTestSample(new byte[] { 0xFF, 0xFF, 0x7D, }),
				new OverflowTestSample(new byte[] { 0xFF, 0xFF, 0x03, }),
				new OverflowTestSample(new byte[] { 0x80, 0x80, 0x7C, }),
				new OverflowTestSample(new byte[] { 0x80, 0x80, 0x80, 0x01, }),
				new OverflowTestSample(new byte[] { 0xFF, 0xFF, 0xFF, 0x7E, }),
			}
			.Concat(Base128IntUnitTest.OverflowTestData)
			.ToArray();

		public static IEnumerable<object[]> GetTestData()
		{
			return Base128LongUnitTest.TestData
				.Where(sample => short.MinValue<=sample.Value && sample.Value<=short.MaxValue)
				.Select(test => new object[] { (short)test.Value, test.Serialized, });
		}

		public static IEnumerable<object[]> GetOverflowTestData()
		{
			return OverflowTestData
				.Concat(Base128LongUnitTest.TestData.Where(sample => sample.Value<short.MinValue || short.MaxValue<sample.Value).Select(sample => new OverflowTestSample(sample.Serialized)))
				.Select(test => new object[] { test.Serialized, });
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryWriteInt16TestMethod(short value, byte[] serialized)
		{
			byte[] buf = new byte[serialized.Length];
			int written;
			bool success;

			success=TryWriteInt32(destination: buf, value: value, written: out written);
			Assert.IsTrue(success);
			Assert.AreEqual(expected: serialized.Length, actual: written);
			Assert.IsTrue(Enumerable.SequenceEqual(serialized, buf));
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryReadInt16TestMethod(short value, byte[] serialized)
		{
			short readValue;
			int read;
			bool success;

			success=TryReadInt16(source: serialized, value: out readValue, read: out read);
			Assert.IsTrue(success);
			Assert.AreEqual(expected: serialized.Length, actual: read);
			Assert.AreEqual(expected: value, actual: readValue);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetOverflowTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryReadInt16OverflowTestMethod(byte[] serialized)
		{
			short readValue;
			int read;

			Assert.ThrowsException<OverflowException>(() => TryReadInt16(source: serialized, value: out readValue, read: out read));
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void GetRequiredBytesInt32TestMethod(short value, byte[] serialized)
		{
			int requiredBytes;

			requiredBytes=GetRequiredBytesInt32(value);

			Assert.AreEqual(expected: serialized.Length, actual: requiredBytes);
		}
	}
}