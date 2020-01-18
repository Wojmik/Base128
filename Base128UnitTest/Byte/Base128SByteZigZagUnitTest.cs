using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WojciechMikołajewicz.Base128UnitTest.Short;
using WojciechMikołajewicz.Base128UnitTest.Long;
using WojciechMikołajewicz.Base128UnitTest.Model;

namespace WojciechMikołajewicz.Base128UnitTest.Byte
{
	[TestClass]
	public class Base128SByteZigZagUnitTest : Base128UnitTestBase<sbyte>
	{
		internal static OverflowTestSample[] OverflowTestData = new OverflowTestSample[]
			{
				new OverflowTestSample(new byte[] { 0x80, 0x02, }),
				new OverflowTestSample(new byte[] { 0x81, 0x02, }),
				new OverflowTestSample(new byte[] { 0xFE, 0x03, }),
				new OverflowTestSample(new byte[] { 0xFF, 0x03, }),
			}
			.Concat(Base128ShortZigZagUnitTest.OverflowTestData)
			.ToArray();

		public static IEnumerable<object[]> GetTestData()
		{
			return Base128LongZigZagUnitTest.TestData
				.Where(sample => sbyte.MinValue<=sample.Value && sample.Value<=sbyte.MaxValue)
				.Select(test => new object[] { (sbyte)test.Value, test.Serialized, });
		}

		public static IEnumerable<object[]> GetOverflowTestData()
		{
			return OverflowTestData
				.Concat(Base128LongZigZagUnitTest.TestData.Where(sample => sample.Value<sbyte.MinValue || sbyte.MaxValue<sample.Value).Select(sample => new OverflowTestSample(sample.Serialized)))
				.Select(test => new object[] { test.Serialized, });
		}

		protected override int MaxMinBytesToWrite { get => 5; }

		protected override bool TryWrite(Span<byte> destination, sbyte value, out int written)
		{
			return Base128.TryWriteInt32ZigZag(destination: destination, value: value, written: out written);
		}

		protected override bool TryWrite(Span<byte> destination, sbyte value, int minBytesToWrite, out int written)
		{
			return Base128.TryWriteInt32ZigZag(destination: destination, value: value, minBytesToWrite: minBytesToWrite, written: out written);
		}

		protected override bool TryRead(Span<byte> source, out sbyte value, out int read)
		{
			return Base128.TryReadInt8ZigZag(source: source, value: out value, read: out read);
		}

		protected override int GetRequiredBytes(sbyte value)
		{
			return Base128.GetRequiredBytesInt32(value);
		}

		#region TryWrite
		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryWriteInt8TestMethod(sbyte value, byte[] serialized)
		{
			TryWriteTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryWriteInt8LongerBufTestMethod(sbyte value, byte[] serialized)
		{
			TryWriteLongerBufTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryWriteInt8EndOfBufTestMethod(sbyte value, byte[] serialized)
		{
			TryWriteEndOfBufTestMethod(value, serialized);
		}
		#endregion

		#region TryRead
		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryReadInt16TestMethod(sbyte value, byte[] serialized)
		{
			TryReadTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryReadInt8LongerBufTestMethod(sbyte value, byte[] serialized)
		{
			TryReadLongerBufTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryReadInt8EndOfBufTestMethod(sbyte value, byte[] serialized)
		{
			TryReadEndOfBufTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetOverflowTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryReadInt8OverflowTestMethod(byte[] serialized)
		{
			TryReadOverflowTestMethod(serialized);
		}
		#endregion

		#region GetRequiredBytes
		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void GetRequiredBytesInt8TestMethod(sbyte value, byte[] serialized)
		{
			GetRequiredBytesTestMethod(value, serialized);
		}
		#endregion

		#region TryWriteWithMinimum
		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryWriteInt8WithMinimum0TestMethod(sbyte value, byte[] serialized)
		{
			TryWriteWithMinimum0TestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryWriteInt8WithMinimum1TestMethod(sbyte value, byte[] serialized)
		{
			TryWriteWithMinimum1TestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryWriteInt8WithMinimumMinValueTestMethod(sbyte value, byte[] serialized)
		{
			TryWriteWithMinimumMinValueTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryWriteInt8WithMinimumMinValuePlusOneTestMethod(sbyte value, byte[] serialized)
		{
			TryWriteWithMinimumMinValuePlusOneTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryWriteInt8WithMinimumOneOverTestMethod(sbyte value, byte[] serialized)
		{
			TryWriteUnsignedWithMinimumOneOverTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryWriteInt8WithMinimumMaxOverTestMethod(sbyte value, byte[] serialized)
		{
			TryWriteUnsignedWithMinimumMaxOverTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryWriteInt8WithMinimumMaxOverPlusOneTestMethod(sbyte value, byte[] serialized)
		{
			TryWriteWithMinimumMaxOverPlusOneTestMethod(value, serialized);
		}
		#endregion

		#region TryReadWithMinimum
		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryReadInt8WithMinimumOneOverTestMethod(sbyte value, byte[] serialized)
		{
			TryReadUnsignedWithMinimumOneOverTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryReadInt8WithMinimumMaxOverTestMethod(sbyte value, byte[] serialized)
		{
			TryReadUnsignedWithMinimumMaxOverTestMethod(value, serialized);
		}
		#endregion
	}
}