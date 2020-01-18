using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WojciechMikołajewicz.Base128UnitTest.Int;
using WojciechMikołajewicz.Base128UnitTest.Long;
using WojciechMikołajewicz.Base128UnitTest.Model;

namespace WojciechMikołajewicz.Base128UnitTest.Short
{
	[TestClass]
	public class Base128ShortUnitTest : Base128UnitTestBase<short>
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

		protected override int MaxMinBytesToWrite { get => 5; }

		protected override bool TryWrite(Span<byte> destination, short value, out int written)
		{
			return Base128.TryWriteInt32(destination: destination, value: value, written: out written);
		}

		protected override bool TryWrite(Span<byte> destination, short value, int minBytesToWrite, out int written)
		{
			return Base128.TryWriteInt32(destination: destination, value: value, minBytesToWrite: minBytesToWrite, written: out written);
		}

		protected override bool TryRead(Span<byte> source, out short value, out int read)
		{
			return Base128.TryReadInt16(source: source, value: out value, read: out read);
		}

		protected override int GetRequiredBytes(short value)
		{
			return Base128.GetRequiredBytesInt32(value);
		}

		#region TryWrite
		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryWriteInt16TestMethod(short value, byte[] serialized)
		{
			TryWriteTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryWriteInt16LongerBufTestMethod(short value, byte[] serialized)
		{
			TryWriteLongerBufTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryWriteInt16EndOfBufTestMethod(short value, byte[] serialized)
		{
			TryWriteEndOfBufTestMethod(value, serialized);
		}
		#endregion

		#region TryRead
		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryReadInt16TestMethod(short value, byte[] serialized)
		{
			TryReadTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryReadInt16LongerBufTestMethod(short value, byte[] serialized)
		{
			TryReadLongerBufTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryReadInt16EndOfBufTestMethod(short value, byte[] serialized)
		{
			TryReadEndOfBufTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetOverflowTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryReadInt16OverflowTestMethod(byte[] serialized)
		{
			TryReadOverflowTestMethod(serialized);
		}
		#endregion

		#region GetRequiredBytes
		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void GetRequiredBytesInt32TestMethod(short value, byte[] serialized)
		{
			GetRequiredBytesTestMethod(value, serialized);
		}
		#endregion

		#region TryWriteWithMinimum
		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryWriteInt16WithMinimum0TestMethod(short value, byte[] serialized)
		{
			TryWriteWithMinimum0TestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryWriteInt16WithMinimum1TestMethod(short value, byte[] serialized)
		{
			TryWriteWithMinimum1TestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryWriteInt16WithMinimumMinValueTestMethod(short value, byte[] serialized)
		{
			TryWriteWithMinimumMinValueTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryWriteInt16WithMinimumMinValuePlusOneTestMethod(short value, byte[] serialized)
		{
			TryWriteWithMinimumMinValuePlusOneTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryWriteInt16WithMinimumOneOverTestMethod(short value, byte[] serialized)
		{
			TryWriteSignedWithMinimumOneOverTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryWriteInt16WithMinimumMaxOverTestMethod(short value, byte[] serialized)
		{
			TryWriteSignedWithMinimumMaxOverTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryWriteInt16WithMinimumMaxOverPlusOneTestMethod(short value, byte[] serialized)
		{
			TryWriteWithMinimumMaxOverPlusOneTestMethod(value, serialized);
		}
		#endregion

		#region TryReadWithMinimum
		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryReadInt16WithMinimumOneOverTestMethod(short value, byte[] serialized)
		{
			TryReadSignedWithMinimumOneOverTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryReadInt16WithMinimumMaxOverTestMethod(short value, byte[] serialized)
		{
			TryReadSignedWithMinimumMaxOverTestMethod(value, serialized);
		}
		#endregion
	}
}