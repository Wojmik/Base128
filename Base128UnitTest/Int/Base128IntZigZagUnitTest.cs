using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WojciechMikołajewicz.Base128UnitTest.Long;
using WojciechMikołajewicz.Base128UnitTest.Model;

namespace WojciechMikołajewicz.Base128UnitTest.Int
{
	[TestClass]
	public class Base128IntZigZagUnitTest : Base128UnitTestBase<int>
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

		protected override int MaxMinBytesToWrite { get => 5; }

		protected override bool TryWrite(Span<byte> destination, int value, out int written)
		{
			return Base128.TryWriteInt32ZigZag(destination: destination, value: value, written: out written);
		}

		protected override bool TryWrite(Span<byte> destination, int value, int minBytesToWrite, out int written)
		{
			return Base128.TryWriteInt32ZigZag(destination: destination, value: value, minBytesToWrite: minBytesToWrite, written: out written);
		}

		protected override bool TryRead(Span<byte> source, out int value, out int read)
		{
			return Base128.TryReadInt32ZigZag(source: source, value: out value, read: out read);
		}

		protected override int GetRequiredBytes(int value)
		{
			return Base128.GetRequiredBytesInt32(value);
		}

		#region TryWrite
		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryWriteInt32TestMethod(int value, byte[] serialized)
		{
			TryWriteTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryWriteInt32LongerBufTestMethod(int value, byte[] serialized)
		{
			TryWriteLongerBufTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryWriteInt32EndOfBufTestMethod(int value, byte[] serialized)
		{
			TryWriteEndOfBufTestMethod(value, serialized);
		}
		#endregion

		#region TryRead
		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryReadInt32TestMethod(int value, byte[] serialized)
		{
			TryReadTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryReadInt32LongerBufTestMethod(int value, byte[] serialized)
		{
			TryReadLongerBufTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryReadInt32EndOfBufTestMethod(int value, byte[] serialized)
		{
			TryReadEndOfBufTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetOverflowTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryReadInt32OverflowTestMethod(byte[] serialized)
		{
			TryReadOverflowTestMethod(serialized);
		}
		#endregion

		#region GetRequiredBytes
		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void GetRequiredBytesInt32TestMethod(int value, byte[] serialized)
		{
			GetRequiredBytesTestMethod(value, serialized);
		}
		#endregion

		#region TryWriteWithMinimum
		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryWriteInt32WithMinimum0TestMethod(int value, byte[] serialized)
		{
			TryWriteWithMinimum0TestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryWriteInt32WithMinimum1TestMethod(int value, byte[] serialized)
		{
			TryWriteWithMinimum1TestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryWriteInt32WithMinimumMinValueTestMethod(int value, byte[] serialized)
		{
			TryWriteWithMinimumMinValueTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryWriteInt32WithMinimumMinValuePlusOneTestMethod(int value, byte[] serialized)
		{
			TryWriteWithMinimumMinValuePlusOneTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryWriteInt32WithMinimumOneOverTestMethod(int value, byte[] serialized)
		{
			TryWriteUnsignedWithMinimumOneOverTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryWriteInt32WithMinimumMaxOverTestMethod(int value, byte[] serialized)
		{
			TryWriteUnsignedWithMinimumMaxOverTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryWriteInt32WithMinimumMaxOverPlusOneTestMethod(int value, byte[] serialized)
		{
			TryWriteWithMinimumMaxOverPlusOneTestMethod(value, serialized);
		}
		#endregion

		#region TryReadWithMinimum
		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryReadInt32WithMinimumOneOverTestMethod(int value, byte[] serialized)
		{
			TryReadUnsignedWithMinimumOneOverTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryReadInt32WithMinimumMaxOverTestMethod(int value, byte[] serialized)
		{
			TryReadUnsignedWithMinimumMaxOverTestMethod(value, serialized);
		}
		#endregion
	}
}