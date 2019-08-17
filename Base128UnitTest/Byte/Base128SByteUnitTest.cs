using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WojciechMikołajewicz.Base128UnitTest.Short;
using WojciechMikołajewicz.Base128UnitTest.Long;
using WojciechMikołajewicz.Base128UnitTest.Model;
using static WojciechMikołajewicz.Base128;

namespace WojciechMikołajewicz.Base128UnitTest.Byte
{
	[TestClass]
	public class Base128SByteUnitTest
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

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryWriteInt8TestMethod(sbyte value, byte[] serialized)
		{
			byte[] buf = new byte[serialized.Length];
			int written;
			bool success;

			success=TryWriteInt32(destination: buf, value: value, written: out written);
			Assert.IsTrue(success);
			Assert.AreEqual(expected: serialized.Length, actual: written);
			Assert.IsTrue(Enumerable.SequenceEqual(serialized, buf));
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryReadInt8TestMethod(sbyte value, byte[] serialized)
		{
			sbyte readValue;
			int read;
			bool success;

			success=TryReadInt8(source: serialized, value: out readValue, read: out read);
			Assert.IsTrue(success);
			Assert.AreEqual(expected: serialized.Length, actual: read);
			Assert.AreEqual(expected: value, actual: readValue);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetOverflowTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void TryReadInt8OverflowTestMethod(byte[] serialized)
		{
			sbyte readValue;
			int read;

			Assert.ThrowsException<OverflowException>(() => TryReadInt8(source: serialized, value: out readValue, read: out read));
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public void GetRequiredBytesInt8TestMethod(sbyte value, byte[] serialized)
		{
			int requiredBytes;

			requiredBytes=GetRequiredBytesInt32(value);

			Assert.AreEqual(expected: serialized.Length, actual: requiredBytes);
		}
	}
}