using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WojciechMikołajewicz.Base128UnitTest.Other
{
	[TestClass]
	public class Base128SkipUnitTest
	{
		public static IEnumerable<object[]> GetTestData()
		{
			return Long.Base128ULongUnitTest.TestData
				.Select(test => new object[] { test.Serialized })
				.Concat(Long.Base128LongUnitTest.TestData
					.Concat(Long.Base128LongZigZagUnitTest.TestData)
					.Select(test => new object[] { test.Serialized })
				)
				.Concat(Byte.Base128ByteUnitTest.OverflowTestData
					.Concat(Byte.Base128SByteUnitTest.OverflowTestData)
					.Concat(Byte.Base128SByteZigZagUnitTest.OverflowTestData)
					.Select(test => new object[] { test.Serialized })
				);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public virtual void TrySkipTestMethod(byte[] serialized)
		{
			int read;
			bool success;

			success=Base128.TrySkip(source: serialized, read: out read);
			Assert.IsTrue(success);
			Assert.AreEqual(expected: serialized.Length, actual: read);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public virtual void TrySkipLongerBufTestMethod(byte[] serialized)
		{
			Span<byte> buf = stackalloc byte[serialized.Length+1];
			int read;
			bool success;

			serialized.AsSpan().CopyTo(buf);
			success=Base128.TrySkip(source: buf, read: out read);
			Assert.IsTrue(success);
			Assert.AreEqual(expected: serialized.Length, actual: read);
		}

		[DataTestMethod]
		[DynamicData(nameof(GetTestData), dynamicDataSourceType: DynamicDataSourceType.Method)]
		public virtual void TrySkipEndOfBufTestMethod(byte[] serialized)
		{
			int read;
			bool success;

			success=Base128.TrySkip(source: serialized.AsSpan(0, serialized.Length-1), read: out read);
			Assert.IsFalse(success);
		}
	}
}