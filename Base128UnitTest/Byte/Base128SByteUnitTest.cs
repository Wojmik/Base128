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
	public class Base128SByteUnitTest : Base128UnitTestBase<sbyte>
	{
		internal static OverflowTestSample[] OverflowTestData = new OverflowTestSample[]
			{
				new OverflowTestSample(new byte[] { 0x80, 0x01, }),
				new OverflowTestSample(new byte[] { 0xFF,0x7E, }),
				new OverflowTestSample(new byte[] { 0xFF, 0x01, }),
				new OverflowTestSample(new byte[] { 0x80, 0x7E, }),
				new OverflowTestSample(new byte[] { 0x80, 0x80, 0x01, }),
				new OverflowTestSample(new byte[] { 0xFF, 0xFF, 0x7E, }),
			}
			.Concat(Base128ShortUnitTest.OverflowTestData)
			.ToArray();

		public static IEnumerable<object[]> GetTestData()
		{
			return Base128LongUnitTest.TestData
				.Where(sample => sbyte.MinValue<=sample.Value && sample.Value<=sbyte.MaxValue)
				.Select(test => new object[] { (sbyte)test.Value, test.Serialized, });
		}

		public static IEnumerable<object[]> GetOverflowTestData()
		{
			return OverflowTestData
				.Concat(Base128LongUnitTest.TestData.Where(sample => sample.Value<sbyte.MinValue || sbyte.MaxValue<sample.Value).Select(sample => new OverflowTestSample(sample.Serialized)))
				.Select(test => new object[] { test.Serialized, });
		}

		protected override bool TryWrite(Span<byte> destination, sbyte value, out int written)
		{
			return Base128.TryWriteInt32(destination: destination, value: value, written: out written);
		}

		protected override bool TryRead(Span<byte> source, out sbyte value, out int read)
		{
			return Base128.TryReadInt8(source: source, value: out value, read: out read);
		}

		protected override int GetRequiredBytes(sbyte value)
		{
			return Base128.GetRequiredBytesInt32(value);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryWriteInt8TestMethod(sbyte value, byte[] serialized)
		{
			TryWriteTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public virtual void TryWriteInt8LongerBufTestMethod(sbyte value, byte[] serialized)
		{
			TryWriteLongerBufTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public virtual void TryWriteInt8EndOfBufTestMethod(sbyte value, byte[] serialized)
		{
			TryWriteEndOfBufTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryReadInt8TestMethod(sbyte value, byte[] serialized)
		{
			TryReadTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public virtual void TryReadInt8LongerBufTestMethod(sbyte value, byte[] serialized)
		{
			TryReadLongerBufTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public virtual void TryReadInt8EndOfBufTestMethod(sbyte value, byte[] serialized)
		{
			TryReadEndOfBufTestMethod(value, serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetOverflowTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryReadInt8OverflowTestMethod(byte[] serialized)
		{
			TryReadOverflowTestMethod(serialized);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void GetRequiredBytesInt8TestMethod(sbyte value, byte[] serialized)
		{
			GetRequiredBytesTestMethod(value, serialized);
		}
	}
}