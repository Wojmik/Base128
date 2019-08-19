using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WojciechMikołajewicz.Base128UnitTest.Long;
using WojciechMikołajewicz.Base128UnitTest.Model;

namespace WojciechMikołajewicz.Base128UnitTest.Int
{
	[TestClass]
	public class Base128UIntUnitTest : Base128UnitTestBase<uint>
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

		protected override bool TryWrite(Span<byte> destination, uint value, out int written)
		{
			return Base128.TryWriteUInt32(destination: destination, value: value, written: out written);
		}

		protected override bool TryRead(Span<byte> source, out uint value, out int read)
		{
			return Base128.TryReadUInt32(source: source, value: out value, read: out read);
		}

		protected override int GetRequiredBytes(uint value)
		{
			return Base128.GetRequiredBytesUInt32(value);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryWriteUInt32TestMethod(uint value, byte[] serialized)
		{
			TryWriteTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public virtual void TryWriteUInt32LongerBufTestMethod(uint value, byte[] serialized)
		{
			TryWriteLongerBufTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public virtual void TryWriteUInt32EndOfBufTestMethod(uint value, byte[] serialized)
		{
			TryWriteEndOfBufTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryReadUInt32TestMethod(uint value, byte[] serialized)
		{
			TryReadTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public virtual void TryReadUInt32LongerBufTestMethod(uint value, byte[] serialized)
		{
			TryReadLongerBufTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public virtual void TryReadUInt32EndOfBufTestMethod(uint value, byte[] serialized)
		{
			TryReadEndOfBufTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetOverflowTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryReadUInt32OverflowTestMethod(byte[] serialized)
		{
			TryReadOverflowTestMethod(serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void GetRequiredBytesUInt32TestMethod(uint value, byte[] serialized)
		{
			GetRequiredBytesTestMethod(value, serialized);
		}
	}
}