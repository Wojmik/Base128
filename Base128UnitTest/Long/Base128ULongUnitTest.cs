using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WojciechMikołajewicz.Base128UnitTest.Model;

namespace WojciechMikołajewicz.Base128UnitTest.Long
{
	[TestClass]
	public class Base128ULongUnitTest : Base128UnitTestBase<ulong>
	{
		internal static readonly TestSample<ulong>[] TestData = new TestSample<ulong>[]
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
			};

		internal static OverflowTestSample[] OverflowTestData = new OverflowTestSample[]
			{
				new OverflowTestSample(new byte[] { 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x02, }),
				new OverflowTestSample(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x03, }),
				new OverflowTestSample(new byte[] { 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x01, }),
			};

		static IEnumerable<object[]> GetTestData()
		{
			return TestData
				.Select(test => new object[] { test.Value, test.Serialized, });
		}

		static IEnumerable<object[]> GetOverflowTestData()
		{
			return OverflowTestData
				.Select(test => new object[] { test.Serialized, });
		}

		protected override bool TryWrite(Span<byte> destination, ulong value, out int written)
		{
			return Base128.TryWriteUInt64(destination: destination, value: value, written: out written);
		}

		protected override bool TryRead(Span<byte> source, out ulong value, out int read)
		{
			return Base128.TryReadUInt64(source: source, value: out value, read: out read);
		}

		protected override int GetRequiredBytes(ulong value)
		{
			return Base128.GetRequiredBytesUInt64(value);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryWriteUInt64TestMethod(ulong value, byte[] serialized)
		{
			TryWriteTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public virtual void TryWriteUInt64LongerBufTestMethod(ulong value, byte[] serialized)
		{
			TryWriteLongerBufTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public virtual void TryWriteUInt64EndOfBufTestMethod(ulong value, byte[] serialized)
		{
			TryWriteEndOfBufTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryReadUInt64TestMethod(ulong value, byte[] serialized)
		{
			TryReadTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public virtual void TryReadUInt64LongerBufTestMethod(ulong value, byte[] serialized)
		{
			TryReadLongerBufTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public virtual void TryReadUInt64EndOfBufTestMethod(ulong value, byte[] serialized)
		{
			TryReadEndOfBufTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetOverflowTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryReadUInt64OverflowTestMethod(byte[] serialized)
		{
			TryReadOverflowTestMethod(serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void GetRequiredBytesUInt64TestMethod(ulong value, byte[] serialized)
		{
			GetRequiredBytesTestMethod(value, serialized);
		}
	}
}