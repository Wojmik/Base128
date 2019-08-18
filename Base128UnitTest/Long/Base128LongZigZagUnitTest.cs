using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WojciechMikołajewicz.Base128UnitTest.Model;
using static WojciechMikołajewicz.Base128;

namespace WojciechMikołajewicz.Base128UnitTest.Long
{
	[TestClass]
	public class Base128LongZigZagUnitTest
	{
		internal static readonly TestSample<long>[] TestData = new TestSample<long>[]
			{
				new TestSample<long>(0, new byte[] { 0x00, }),
				new TestSample<long>(1, new byte[] { 0x02, }),
				new TestSample<long>(-1, new byte[] { 0x01, }),
				new TestSample<long>(63, new byte[] { 0x7E, }),//2^6-1
				new TestSample<long>(-64, new byte[] { 0x7F, }),//-2^6
				new TestSample<long>(64, new byte[] { 0x80, 0x01, }),//2^6
				new TestSample<long>(-65, new byte[] { 0x81, 0x01, }),//-2^6-1
				new TestSample<long>(8191, new byte[] { 0xFE, 0x7F, }),//2^13-1
				new TestSample<long>(-8192, new byte[] { 0xFF, 0x7F, }),//-2^13
				new TestSample<long>(8192, new byte[] { 0x80, 0x80, 0x01, }),//2^13
				new TestSample<long>(-8193, new byte[] { 0x81, 0x80, 0x01, }),//-2^13-1
				new TestSample<long>(1048575, new byte[] { 0xFE, 0xFF, 0x7F, }),//2^20-1
				new TestSample<long>(-1048576, new byte[] { 0xFF, 0xFF, 0x7F, }),//-2^20
				new TestSample<long>(1048576, new byte[] { 0x80, 0x80, 0x80, 0x01, }),//2^20
				new TestSample<long>(-1048577, new byte[] { 0x81, 0x80, 0x80, 0x01, }),//-2^20-1
				new TestSample<long>(134217727, new byte[] { 0xFE, 0xFF, 0xFF, 0x7F, }),//2^27-1
				new TestSample<long>(-134217728, new byte[] { 0xFF, 0xFF, 0xFF, 0x7F, }),//-2^27
				new TestSample<long>(134217728, new byte[] { 0x80, 0x80, 0x80, 0x80, 0x01, }),//2^27
				new TestSample<long>(-134217729, new byte[] { 0x81, 0x80, 0x80, 0x80, 0x01, }),//-2^27-1
				new TestSample<long>(17179869183, new byte[] { 0xFE, 0xFF, 0xFF, 0xFF, 0x7F, }),//2^34-1
				new TestSample<long>(-17179869184, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0x7F, }),//-2^34
				new TestSample<long>(17179869184, new byte[] { 0x80, 0x80, 0x80, 0x80, 0x80, 0x01, }),//2^34
				new TestSample<long>(-17179869185, new byte[] { 0x81, 0x80, 0x80, 0x80, 0x80, 0x01, }),//-2^34-1
				new TestSample<long>(2199023255551, new byte[] { 0xFE, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F, }),//2^41-1
				new TestSample<long>(-2199023255552, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F, }),//-2^41
				new TestSample<long>(2199023255552, new byte[] { 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x01, }),//2^41
				new TestSample<long>(-2199023255553, new byte[] { 0x81, 0x80, 0x80, 0x80, 0x80, 0x80, 0x01, }),//-2^41-1
				new TestSample<long>(281474976710655, new byte[] { 0xFE, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F, }),//2^48-1
				new TestSample<long>(-281474976710656, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F, }),//-2^48
				new TestSample<long>(281474976710656, new byte[] { 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x01, }),//2^48
				new TestSample<long>(-281474976710657, new byte[] { 0x81, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x01, }),//-2^48-1
				new TestSample<long>(36028797018963967, new byte[] { 0xFE, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F, }),//2^55-1
				new TestSample<long>(-36028797018963968, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F, }),//-2^55
				new TestSample<long>(36028797018963968, new byte[] { 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x01, }),//2^55
				new TestSample<long>(-36028797018963969, new byte[] { 0x81, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x01, }),//-2^55-1
				new TestSample<long>(4611686018427387903, new byte[] { 0xFE, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F, }),//2^62-1
				new TestSample<long>(-4611686018427387904, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F, }),//-2^62
				new TestSample<long>(4611686018427387904, new byte[] { 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x01, }),//2^62
				new TestSample<long>(-4611686018427387905, new byte[] { 0x81, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x01, }),//-2^62-1
				new TestSample<long>(sbyte.MaxValue, new byte[] { 0xFE, 0x01, }),
				new TestSample<long>(sbyte.MinValue, new byte[] { 0xFF, 0x01, }),
				new TestSample<long>(byte.MaxValue, new byte[] { 0xFE, 0x03, }),
				new TestSample<long>(short.MaxValue, new byte[] { 0xFE, 0xFF, 0x03, }),
				new TestSample<long>(short.MinValue, new byte[] { 0xFF, 0xFF, 0x03, }),
				new TestSample<long>(ushort.MaxValue, new byte[] { 0xFE, 0xFF, 0x07, }),
				new TestSample<long>(int.MaxValue, new byte[] { 0xFE, 0xFF, 0xFF, 0xFF, 0x0F, }),
				new TestSample<long>(int.MinValue, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0x0F, }),
				new TestSample<long>(uint.MaxValue, new byte[] { 0xFE, 0xFF, 0xFF, 0xFF, 0x1F, }),
				new TestSample<long>(long.MaxValue, new byte[] { 0xFE, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x01, }),
				new TestSample<long>(long.MinValue, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x01, }),
				new TestSample<long>(300, new byte[] { 0xD8, 0x04, }),
			};

		internal static readonly OverflowTestSample[] OverflowTestData = new OverflowTestSample[]
			{
				new OverflowTestSample(new byte[] { 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x02, }),
				new OverflowTestSample(new byte[] { 0x81, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x02, }),
				new OverflowTestSample(new byte[] { 0xFE, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x03, }),
				new OverflowTestSample(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x03, }),
			};

		public static IEnumerable<object[]> GetTestData()
		{
			return TestData
				.Select(test => new object[] { test.Value, test.Serialized, });
		}

		public static IEnumerable<object[]> GetOverflowTestData()
		{
			return OverflowTestData
				.Select(test => new object[] { test.Serialized, });
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryWriteInt64TestMethod(long value, byte[] serialized)
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
		public void TryReadInt64TestMethod(long value, byte[] serialized)
		{
			long readValue;
			int read;
			bool success;

			success=TryReadInt64ZigZag(source: serialized, value: out readValue, read: out read);
			Assert.IsTrue(success);
			Assert.AreEqual(expected: serialized.Length, actual: read);
			Assert.AreEqual(expected: value, actual: readValue);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetOverflowTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryReadInt64OverflowTestMethod(byte[] serialized)
		{
			long readValue;
			int read;

			Assert.ThrowsException<OverflowException>(() => TryReadInt64ZigZag(source: serialized, value: out readValue, read: out read));
		}
	}
}