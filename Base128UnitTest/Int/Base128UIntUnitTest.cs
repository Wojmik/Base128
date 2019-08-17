using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WojciechMiko³ajewicz.Base128UnitTest.Long;
using WojciechMiko³ajewicz.Base128UnitTest.Model;
using static WojciechMiko³ajewicz.Base128;

namespace WojciechMiko³ajewicz.Base128UnitTest.Int
{
	[TestClass]
	public class Base128UIntUnitTest
	{
		internal static OverflowTestSample[] OverflowTestData = new OverflowTestSample[]
			{
				new OverflowTestSample(new byte[] { 0x80, 0x80, 0x80, 0x80, 0x10, }),
				new OverflowTestSample(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0x1F, }),
				new OverflowTestSample(new byte[] { 0x80, 0x80, 0x80, 0x80, 0x80, 0x01, }),
			}
			.Concat(Base128ULongUnitTest.OverflowTestData)
			.ToArray();

		public static IEnumerable<object[]> GetTestData()
		{
			return Base128ULongUnitTest.TestData
				.Where(sample => uint.MinValue<=sample.Value && sample.Value<=uint.MaxValue)
				.Select(test => new object[] { (uint)test.Value, test.Serialized, });
		}

		public static IEnumerable<object[]> GetOverflowTestData()
		{
			return OverflowTestData
				.Concat(Base128ULongUnitTest.TestData.Where(sample => sample.Value<uint.MinValue || uint.MaxValue<sample.Value).Select(sample => new OverflowTestSample(sample.Serialized)))
				.Select(test => new object[] { test.Serialized, });
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryWriteUInt32TestMethod(uint value, byte[] serialized)
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
		public void TryReadUInt32TestMethod(uint value, byte[] serialized)
		{
			uint readValue;
			int read;
			bool success;

			success=TryReadUInt32(source: serialized, value: out readValue, read: out read);
			Assert.IsTrue(success);
			Assert.AreEqual(expected: serialized.Length, actual: read);
			Assert.AreEqual(expected: value, actual: readValue);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetOverflowTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryReadUInt32OverflowTestMethod(byte[] serialized)
		{
			uint readValue;
			int read;

			Assert.ThrowsException<OverflowException>(() => TryReadUInt32(source: serialized, value: out readValue, read: out read));
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void GetRequiredBytesUInt32TestMethod(uint value, byte[] serialized)
		{
			int requiredBytes;

			requiredBytes=GetRequiredBytesUInt32(value);

			Assert.AreEqual(expected: serialized.Length, actual: requiredBytes);
		}
	}
}