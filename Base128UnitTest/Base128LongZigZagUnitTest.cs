using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static WojciechMikołajewicz.Base128;

namespace WojciechMikołajewicz.Base128UnitTest
{
	[TestClass]
	public class Base128LongZigZagUnitTest
	{
		public static IEnumerable<object[]> GetTestData()
		{
			return new TestSample<long>[]
			{
				new TestSample<long>(0, new byte[] { 0x00, }),
				new TestSample<long>(1, new byte[] { 0x02, }),
				new TestSample<long>(-1, new byte[] { 0x01, }),
				new TestSample<long>(63, new byte[] { 0x7E, }),
				new TestSample<long>(-64, new byte[] { 0x7F, }),
				new TestSample<long>(64, new byte[] { 0x80, 0x01, }),
				new TestSample<long>(-65, new byte[] { 0x81, 0x01, }),
				new TestSample<long>(8191, new byte[] { 0xFE, 0x7F, }),
				new TestSample<long>(-8192, new byte[] { 0xFF, 0x7F, }),
				new TestSample<long>(8192, new byte[] { 0x80, 0x80, 0x01, }),
				new TestSample<long>(-8193, new byte[] { 0x81, 0x80, 0x01, }),
				new TestSample<long>(300, new byte[] { 0xD8, 0x04, }),
				new TestSample<long>(int.MaxValue, new byte[] { 0xFE, 0xFF, 0xFF, 0xFF, 0x0F, }),
				new TestSample<long>(int.MinValue, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0x0F, }),
				new TestSample<long>(long.MaxValue, new byte[] { 0xFE, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x01, }),
				new TestSample<long>(long.MinValue, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x01, }),
			}
			.Select(test => new object[] { test.Value, test.Serialized, });
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryWriteUInt64TestMethod(long value, byte[] serialized)
		{
			byte[] buf = new byte[serialized.Length];
			int written;
			bool success;

			success=TryWriteInt64ZigZag(destination: buf, value: value, written: out written);
			Assert.IsTrue(success);
			Assert.AreEqual(expected: serialized.Length, actual: written);
			Assert.IsTrue(Enumerable.SequenceEqual(serialized, buf));
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryReadUInt64TestMethod(long value, byte[] serialized)
		{
			long readValue;
			int read;
			bool success;

			success=TryReadInt64ZigZag(source: serialized, value: out readValue, read: out read);
			Assert.IsTrue(success);
			Assert.AreEqual(expected: serialized.Length, actual: read);
			Assert.AreEqual(expected: value, actual: readValue);
		}
	}
}