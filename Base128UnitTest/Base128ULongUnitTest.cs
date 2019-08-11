using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static WojciechMiko³ajewicz.Base128;

namespace WojciechMiko³ajewicz.Base128UnitTest
{
	[TestClass]
	public class Base128ULongUnitTest
	{
		public static IEnumerable<object[]> GetTestData()
		{
			return new TestSample<ulong>[]
				{
					new TestSample<ulong>(0x0, new byte[] { 0x00, }),//2^0-1
					new TestSample<ulong>(0x1, new byte[] { 0x01, }),//2^0
					new TestSample<ulong>(0x7F, new byte[] { 0x7F, }),//2^7-1
					new TestSample<ulong>(0x80, new byte[] { 0x80, 0x01, }),//2^7
					new TestSample<ulong>(0x3FFF, new byte[] { 0xFF, 0x7F, }),//2^14-1
					new TestSample<ulong>(0x4000, new byte[] { 0x80, 0x80, 0x01, }),//2^14
					new TestSample<ulong>(0x1FFFFF, new byte[] { 0xFF, 0xFF, 0x7F, }),//2^21-1
					new TestSample<ulong>(0x200000, new byte[] { 0x80, 0x80, 0x80, 0x01, }),//2^21
					new TestSample<ulong>(0xFFFFFFF, new byte[] { 0xFF, 0xFF, 0xFF, 0x7F, }),//2^28-1
					new TestSample<ulong>(0x10000000, new byte[] { 0x80, 0x80, 0x80, 0x80, 0x01, }),//2^28
					new TestSample<ulong>(0x7FFFFFFFF, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0x7F, }),//2^35-1
					new TestSample<ulong>(0x800000000, new byte[] { 0x80, 0x80, 0x80, 0x80, 0x80, 0x01, }),//2^35
					new TestSample<ulong>(0x3FFFFFFFFFF, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F, }),//2^42-1
					new TestSample<ulong>(0x40000000000, new byte[] { 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x01, }),//2^42
					new TestSample<ulong>(0x1FFFFFFFFFFFF, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F, }),//2^49-1
					new TestSample<ulong>(0x2000000000000, new byte[] { 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x01, }),//2^49
					new TestSample<ulong>(0xFFFFFFFFFFFFFF, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F, }),//2^56-1
					new TestSample<ulong>(0x100000000000000, new byte[] { 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x01, }),//2^56
					new TestSample<ulong>(0x7FFFFFFFFFFFFFFF, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F, }),//2^63-1
					new TestSample<ulong>(0x8000000000000000, new byte[] { 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x01, }),//2^63
					new TestSample<ulong>((ulong)sbyte.MaxValue, new byte[] { 0x7F, }),
					new TestSample<ulong>(byte.MaxValue, new byte[] { 0xFF, 0x01, }),
					new TestSample<ulong>((ulong)short.MaxValue, new byte[] { 0xFF, 0xFF, 0x01, }),
					new TestSample<ulong>(ushort.MaxValue, new byte[] { 0xFF, 0xFF, 0x03, }),
					new TestSample<ulong>(int.MaxValue, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0x07, }),
					new TestSample<ulong>(uint.MaxValue, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0x0F, }),
					new TestSample<ulong>(long.MaxValue, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F }),
					new TestSample<ulong>(ulong.MaxValue, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x01, }),
					new TestSample<ulong>(63, new byte[] { 0x3F, }),
					new TestSample<ulong>(64, new byte[] { 0x40, }),
					new TestSample<ulong>(300, new byte[] { 0xAC, 0x02, }),
				}
				.Select(test => new object[] { test.Value, test.Serialized, });
		}

		public static IEnumerable<object[]> GetOverflowTestData()
		{
			return new OverflowTest[]
				{
					new OverflowTest(new byte[] { 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x02, }),
					new OverflowTest(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x03, }),
				}
				.Select(test => new object[] { test.Serialized, });
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryWriteUInt64TestMethod(ulong value, byte[] serialized)
		{
			byte[] buf = new byte[serialized.Length];
			int written;
			bool success;

			success=TryWriteUInt64(destination: buf, value: value, written: out written);
			Assert.IsTrue(success);
			Assert.AreEqual(expected: serialized.Length, actual: written);
			Assert.IsTrue(Enumerable.SequenceEqual(serialized, buf));
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryReadUInt64TestMethod(ulong value, byte[] serialized)
		{
			ulong readValue;
			int read;
			bool success;

			success=TryReadUInt64(source: serialized, value: out readValue, read: out read);
			Assert.IsTrue(success);
			Assert.AreEqual(expected: serialized.Length, actual: read);
			Assert.AreEqual(expected: value, actual: readValue);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetOverflowTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryReadUInt64OverflowTestMethod(byte[] serialized)
		{
			ulong readValue;
			int read;

			Assert.ThrowsException<OverflowException>(() => TryReadUInt64(source: serialized, value: out readValue, read: out read));
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void GetRequiredBytesUInt64TestMethod(ulong value, byte[] serialized)
		{
			int requiredBytes;

			requiredBytes=GetRequiredBytesUInt64(value);

			Assert.AreEqual(expected: serialized.Length, actual: requiredBytes);
		}
	}
}