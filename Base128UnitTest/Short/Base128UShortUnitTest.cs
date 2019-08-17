using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WojciechMiko³ajewicz.Base128UnitTest.Int;
using WojciechMiko³ajewicz.Base128UnitTest.Long;
using WojciechMiko³ajewicz.Base128UnitTest.Model;
using static WojciechMiko³ajewicz.Base128;

namespace WojciechMiko³ajewicz.Base128UnitTest.Short
{
	[TestClass]
	public class Base128UShortUnitTest
	{
		internal static OverflowTestSample[] OverflowTestData = new OverflowTestSample[]
			{
				new OverflowTestSample(new byte[] { 0x80, 0x80, 0x04, }),
				new OverflowTestSample(new byte[] { 0xFF, 0xFF, 0x07, }),
				new OverflowTestSample(new byte[] { 0x80, 0x80, 0x80, 0x01, }),
			}
			.Concat(Base128UIntUnitTest.OverflowTestData)
			.ToArray();

		public static IEnumerable<object[]> GetTestData()
		{
			return Base128ULongUnitTest.TestData
				.Where(sample => ushort.MinValue<=sample.Value && sample.Value<=ushort.MaxValue)
				.Select(test => new object[] { (ushort)test.Value, test.Serialized, });
		}

		public static IEnumerable<object[]> GetOverflowTestData()
		{
			return OverflowTestData
				.Concat(Base128ULongUnitTest.TestData.Where(sample => sample.Value<ushort.MinValue || ushort.MaxValue<sample.Value).Select(sample => new OverflowTestSample(sample.Serialized)))
				.Select(test => new object[] { test.Serialized, });
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryWriteUInt16TestMethod(ushort value, byte[] serialized)
		{
			byte[] buf = new byte[serialized.Length];
			int written;
			bool success;

			success=TryWriteUInt32(destination: buf, value: value, written: out written);
			Assert.IsTrue(success);
			Assert.AreEqual(expected: serialized.Length, actual: written);
			Assert.IsTrue(Enumerable.SequenceEqual(serialized, buf));
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryReadUInt16TestMethod(ushort value, byte[] serialized)
		{
			ushort readValue;
			int read;
			bool success;

			success=TryReadUInt16(source: serialized, value: out readValue, read: out read);
			Assert.IsTrue(success);
			Assert.AreEqual(expected: serialized.Length, actual: read);
			Assert.AreEqual(expected: value, actual: readValue);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetOverflowTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryReadUInt16OverflowTestMethod(byte[] serialized)
		{
			ushort readValue;
			int read;

			Assert.ThrowsException<OverflowException>(() => TryReadUInt16(source: serialized, value: out readValue, read: out read));
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void GetRequiredBytesUInt16TestMethod(ushort value, byte[] serialized)
		{
			int requiredBytes;

			requiredBytes=GetRequiredBytesUInt32(value);

			Assert.AreEqual(expected: serialized.Length, actual: requiredBytes);
		}
	}
}