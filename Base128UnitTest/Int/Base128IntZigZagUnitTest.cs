using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WojciechMikołajewicz.Base128UnitTest.Long;
using WojciechMikołajewicz.Base128UnitTest.Model;
using static WojciechMikołajewicz.Base128;

namespace WojciechMikołajewicz.Base128UnitTest.Int
{
	[TestClass]
	public class Base128IntZigZagUnitTest
	{
		internal static OverflowTestSample[] OverflowTestData = new OverflowTestSample[]
			{
				new OverflowTestSample(new byte[] { 0x80, 0x80, 0x80, 0x80, 0x10, }),
				new OverflowTestSample(new byte[] { 0x81, 0x80, 0x80, 0x80, 0x10, }),
				new OverflowTestSample(new byte[] { 0xFE, 0xFF, 0xFF, 0xFF, 0x1F, }),
				new OverflowTestSample(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0x1F, }),
			}
			.Concat(Base128LongZigZagUnitTest.OverflowTestData)
			.ToArray();

		public static IEnumerable<object[]> GetTestData()
		{
			return Base128LongZigZagUnitTest.TestData
				.Where(sample => int.MinValue<=sample.Value && sample.Value<=int.MaxValue)
				.Select(test => new object[] { (int)test.Value, test.Serialized, });
		}

		public static IEnumerable<object[]> GetOverflowTestData()
		{
			return OverflowTestData
				.Concat(Base128LongZigZagUnitTest.TestData.Where(sample => sample.Value<int.MinValue || int.MaxValue<sample.Value).Select(sample => new OverflowTestSample(sample.Serialized)))
				.Select(test => new object[] { test.Serialized, });
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryWriteInt32TestMethod(int value, byte[] serialized)
		{
			byte[] buf = new byte[serialized.Length];
			int written;
			bool success;

			success=TryWriteInt32ZigZag(destination: buf, value: value, written: out written);
			Assert.IsTrue(success);
			Assert.AreEqual(expected: serialized.Length, actual: written);
			Assert.IsTrue(Enumerable.SequenceEqual(serialized, buf));
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryReadInt32TestMethod(int value, byte[] serialized)
		{
			int readValue;
			int read;
			bool success;

			success=TryReadInt32ZigZag(source: serialized, value: out readValue, read: out read);
			Assert.IsTrue(success);
			Assert.AreEqual(expected: serialized.Length, actual: read);
			Assert.AreEqual(expected: value, actual: readValue);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetOverflowTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryReadInt32OverflowTestMethod(byte[] serialized)
		{
			int readValue;
			int read;

			Assert.ThrowsException<OverflowException>(() => TryReadInt32ZigZag(source: serialized, value: out readValue, read: out read));
		}
	}
}