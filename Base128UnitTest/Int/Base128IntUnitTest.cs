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
	public class Base128IntUnitTest : Base128UnitTestBase<int>
	{
		internal static OverflowTestSample[] OverflowTestData = new OverflowTestSample[]
			{
				new OverflowTestSample(new byte[] { 0x80, 0x80, 0x80, 0x80, 0x08, }),
				new OverflowTestSample(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0x77, }),
				new OverflowTestSample(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0x0F, }),
				new OverflowTestSample(new byte[] { 0x80, 0x80, 0x80, 0x80, 0x70, }),
				new OverflowTestSample(new byte[] { 0x80, 0x80, 0x80, 0x80, 0x08, 0x01, }),
				new OverflowTestSample(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7E, }),
			}
			.Concat(Base128LongUnitTest.OverflowTestData)
			.ToArray();

		public static IEnumerable<object[]> GetTestData()
		{
			return Base128LongUnitTest.TestData
				.Where(sample => int.MinValue<=sample.Value && sample.Value<=int.MaxValue)
				.Select(test => new object[] { (int)test.Value, test.Serialized, });
		}

		public static IEnumerable<object[]> GetOverflowTestData()
		{
			return OverflowTestData
				.Concat(Base128LongUnitTest.TestData.Where(sample => sample.Value<int.MinValue || int.MaxValue<sample.Value).Select(sample => new OverflowTestSample(sample.Serialized)))
				.Select(test => new object[] { test.Serialized, });
		}

		protected override bool TryWrite(Span<byte> destination, int value, out int written)
		{
			return Base128.TryWriteInt32(destination: destination, value: value, written: out written);
		}

		protected override bool TryRead(Span<byte> source, out int value, out int read)
		{
			return Base128.TryReadInt32(source: source, value: out value, read: out read);
		}

		protected override int GetRequiredBytes(int value)
		{
			return Base128.GetRequiredBytesInt32(value);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryWriteInt32TestMethod(int value, byte[] serialized)
		{
			TryWriteTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public virtual void TryWriteInt32LongerBufTestMethod(int value, byte[] serialized)
		{
			TryWriteLongerBufTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public virtual void TryWriteInt32EndOfBufTestMethod(int value, byte[] serialized)
		{
			TryWriteEndOfBufTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryReadInt32TestMethod(int value, byte[] serialized)
		{
			TryReadTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public virtual void TryReadInt32LongerBufTestMethod(int value, byte[] serialized)
		{
			TryReadLongerBufTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public virtual void TryReadInt32EndOfBufTestMethod(int value, byte[] serialized)
		{
			TryReadEndOfBufTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetOverflowTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryReadInt32OverflowTestMethod(byte[] serialized)
		{
			TryReadOverflowTestMethod(serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void GetRequiredBytesInt32TestMethod(int value, byte[] serialized)
		{
			GetRequiredBytesTestMethod(value, serialized);
		}
	}
}