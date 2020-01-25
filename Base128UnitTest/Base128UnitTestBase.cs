using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace WojciechMikołajewicz.Base128UnitTest
{
	public abstract class Base128UnitTestBase<T>
		where T : struct
	{
		protected abstract int MaxMinBytesToWrite { get; }

		protected abstract bool TryWrite(Span<byte> destination, T value, out int written);

		protected abstract bool TryWrite(Span<byte> destination, T value, int minBytesToWrite, out int written);

		protected abstract bool TryRead(Span<byte> source, out T value, out int read);

		protected abstract int GetRequiredBytes(T value);

		#region Write
		public virtual void TryWriteTestMethod(T value, byte[] serialized)
		{
			Span<byte> buf = stackalloc byte[serialized.Length];
			int written;
			bool success;

			success=TryWrite(destination: buf, value: value, written: out written);
			Assert.IsTrue(success);
			Assert.AreEqual(expected: serialized.Length, actual: written);
			Assert.IsTrue(buf.SequenceEqual(serialized));
		}

		public virtual void TryWriteLongerBufTestMethod(T value, byte[] serialized)
		{
			Span<byte> buf = stackalloc byte[serialized.Length+1];
			int written;
			bool success;

			success=TryWrite(destination: buf, value: value, written: out written);
			Assert.IsTrue(success);
			Assert.AreEqual(expected: serialized.Length, actual: written);
			Assert.IsTrue(buf.StartsWith(serialized));
		}

		public virtual void TryWriteEndOfBufTestMethod(T value, byte[] serialized)
		{
			Span<byte> buf = stackalloc byte[serialized.Length-1];
			int written;
			bool success;

			success=TryWrite(destination: buf, value: value, written: out written);
			Assert.IsFalse(success);
			Assert.AreEqual(expected: 0, actual: written);
		}
		#endregion

		#region Read
		public virtual void TryReadTestMethod(T value, byte[] serialized)
		{
			T readValue;
			int read;
			bool success;

			success=TryRead(source: serialized, value: out readValue, read: out read);
			Assert.IsTrue(success);
			Assert.AreEqual(expected: serialized.Length, actual: read);
			Assert.AreEqual(expected: value, actual: readValue);
		}

		public virtual void TryReadLongerBufTestMethod(T value, byte[] serialized)
		{
			Span<byte> buf = stackalloc byte[serialized.Length+1];
			T readValue;
			int read;
			bool success;

			serialized.AsSpan().CopyTo(buf);
			success=TryRead(source: buf, value: out readValue, read: out read);
			Assert.IsTrue(success);
			Assert.AreEqual(expected: serialized.Length, actual: read);
			Assert.AreEqual(expected: value, actual: readValue);
		}

		public virtual void TryReadEndOfBufTestMethod(T value, byte[] serialized)
		{
			T readValue;
			int read;
			bool success;

			success=TryRead(source: serialized.AsSpan(0, serialized.Length-1), value: out readValue, read: out read);
			Assert.IsFalse(success);
			Assert.AreEqual(default, readValue);
			Assert.AreEqual(expected: 0, actual: read);
		}

		public virtual void TryReadOverflowTestMethod(byte[] serialized)
		{
			T readValue;
			int read;

			Assert.ThrowsException<OverflowException>(() => TryRead(source: serialized, value: out readValue, read: out read));
		}
		#endregion

		#region GetRequiredBytes
		public virtual void GetRequiredBytesTestMethod(T value, byte[] serialized)
		{
			int requiredBytes;

			requiredBytes=GetRequiredBytes(value);

			Assert.AreEqual(expected: serialized.Length, actual: requiredBytes);
		}
		#endregion

		#region WriteWithMinimum
		public virtual void TryWriteWithMinimum0TestMethod(T value, byte[] serialized)
		{
			Span<byte> buf = stackalloc byte[serialized.Length];
			int written;
			bool success;

			success=TryWrite(destination: buf, value: value, minBytesToWrite: 0, written: out written);
			Assert.IsTrue(success);
			Assert.AreEqual(expected: serialized.Length, actual: written);
			Assert.IsTrue(buf.SequenceEqual(serialized));
		}

		public virtual void TryWriteWithMinimum1TestMethod(T value, byte[] serialized)
		{
			Span<byte> buf = stackalloc byte[serialized.Length];
			int written;
			bool success;

			success=TryWrite(destination: buf, value: value, minBytesToWrite: 1, written: out written);
			Assert.IsTrue(success);
			Assert.AreEqual(expected: serialized.Length, actual: written);
			Assert.IsTrue(buf.SequenceEqual(serialized));
		}

		public virtual void TryWriteWithMinimumMinValueTestMethod(T value, byte[] serialized)
		{
			Span<byte> buf = stackalloc byte[serialized.Length];
			int written;
			bool success;

			success=TryWrite(destination: buf, value: value, minBytesToWrite: int.MinValue, written: out written);
			Assert.IsTrue(success);
			Assert.AreEqual(expected: serialized.Length, actual: written);
			Assert.IsTrue(buf.SequenceEqual(serialized));
		}

		public virtual void TryWriteWithMinimumMinValuePlusOneTestMethod(T value, byte[] serialized)
		{
			Span<byte> buf = stackalloc byte[serialized.Length];
			int written;
			bool success;

			success=TryWrite(destination: buf, value: value, minBytesToWrite: int.MinValue+1, written: out written);
			Assert.IsTrue(success);
			Assert.AreEqual(expected: serialized.Length, actual: written);
			Assert.IsTrue(buf.SequenceEqual(serialized));
		}

		public virtual void TryWriteUnsignedWithMinimumOneOverTestMethod(T value, byte[] serialized)
		{
			int written, minToWrite=Math.Min(serialized.Length+1, MaxMinBytesToWrite);
			Span<byte> buf = stackalloc byte[minToWrite], expected = stackalloc byte[minToWrite];
			bool success;

			GetUnsignedWithMinimumExpected(sample: serialized, expected: expected);
			success=TryWrite(destination: buf, value: value, minBytesToWrite: minToWrite, written: out written);
			Assert.IsTrue(success);
			Assert.AreEqual(expected: expected.Length, actual: written);
			Assert.IsTrue(buf.SequenceEqual(expected));
		}

		public virtual void TryWriteSignedWithMinimumOneOverTestMethod(T value, byte[] serialized)
		{
			int written, minToWrite=Math.Min(serialized.Length+1, MaxMinBytesToWrite);
			Span<byte> buf = stackalloc byte[minToWrite], expected = stackalloc byte[minToWrite];
			bool success;

			GetSignedWithMinimumExpected(sample: serialized, expected: expected);
			success=TryWrite(destination: buf, value: value, minBytesToWrite: minToWrite, written: out written);
			Assert.IsTrue(success);
			Assert.AreEqual(expected: expected.Length, actual: written);
			Assert.IsTrue(buf.SequenceEqual(expected));
		}

		public virtual void TryWriteUnsignedWithMinimumMaxOverTestMethod(T value, byte[] serialized)
		{
			int written;
			Span<byte> buf = stackalloc byte[MaxMinBytesToWrite], expected = stackalloc byte[MaxMinBytesToWrite];
			bool success;

			GetUnsignedWithMinimumExpected(sample: serialized, expected: expected);
			success=TryWrite(destination: buf, value: value, minBytesToWrite: MaxMinBytesToWrite, written: out written);
			Assert.IsTrue(success);
			Assert.AreEqual(expected: expected.Length, actual: written);
			Assert.IsTrue(buf.SequenceEqual(expected));
		}

		public virtual void TryWriteSignedWithMinimumMaxOverTestMethod(T value, byte[] serialized)
		{
			int written;
			Span<byte> buf = stackalloc byte[MaxMinBytesToWrite], expected = stackalloc byte[MaxMinBytesToWrite];
			bool success;

			GetSignedWithMinimumExpected(sample: serialized, expected: expected);
			success=TryWrite(destination: buf, value: value, minBytesToWrite: MaxMinBytesToWrite, written: out written);
			Assert.IsTrue(success);
			Assert.AreEqual(expected: expected.Length, actual: written);
			Assert.IsTrue(buf.SequenceEqual(expected));
		}

		public virtual void TryWriteWithMinimumMaxOverPlusOneTestMethod(T value, byte[] serialized)
		{
			int written;

			Assert.ThrowsException<ArgumentException>(() =>
			{
				Span<byte> buf = stackalloc byte[MaxMinBytesToWrite+1];

				TryWrite(destination: buf, value: value, minBytesToWrite: MaxMinBytesToWrite+1, written: out written);
			});
		}

		private void GetUnsignedWithMinimumExpected(Span<byte> sample, Span<byte> expected)
		{
			int i;

			sample.CopyTo(expected);
			expected.Slice(sample.Length).Fill(0);

			for(i=sample.Length; i<expected.Length; i++)
			{
				expected[i-1]|=0x80;

			}
		}
		
		private void GetSignedWithMinimumExpected(Span<byte> sample, Span<byte> expected)
		{
			int i;

			sample.CopyTo(expected);
			expected.Slice(sample.Length).Fill((byte)((uint)(sample[^1]<<25>>6)>>25));

			for(i=sample.Length; i<expected.Length; i++)
			{
				expected[i-1]|=0x80;

			}
		}
		#endregion

		#region ReadWithMinimum
		public virtual void TryReadUnsignedWithMinimumOneOverTestMethod(T value, byte[] serialized)
		{
			T readValue;
			int read, minToWrite = Math.Min(serialized.Length+1, MaxMinBytesToWrite);
			Span<byte> buf = stackalloc byte[minToWrite];
			bool success;

			GetUnsignedWithMinimumExpected(sample: serialized, expected: buf);
			success=TryRead(source: buf, value: out readValue, read: out read);
			Assert.IsTrue(success);
			Assert.AreEqual(expected: buf.Length, actual: read);
			Assert.AreEqual(expected: value, actual: readValue);
		}

		public virtual void TryReadSignedWithMinimumOneOverTestMethod(T value, byte[] serialized)
		{
			T readValue;
			int read, minToWrite = Math.Min(serialized.Length+1, MaxMinBytesToWrite);
			Span<byte> buf = stackalloc byte[minToWrite];
			bool success;

			GetSignedWithMinimumExpected(sample: serialized, expected: buf);
			success=TryRead(source: buf, value: out readValue, read: out read);
			Assert.IsTrue(success);
			Assert.AreEqual(expected: buf.Length, actual: read);
			Assert.AreEqual(expected: value, actual: readValue);
		}

		public virtual void TryReadUnsignedWithMinimumMaxOverTestMethod(T value, byte[] serialized)
		{
			T readValue;
			int read;
			Span<byte> buf = stackalloc byte[MaxMinBytesToWrite];
			bool success;

			GetUnsignedWithMinimumExpected(sample: serialized, expected: buf);
			success=TryRead(source: buf, value: out readValue, read: out read);
			Assert.IsTrue(success);
			Assert.AreEqual(expected: buf.Length, actual: read);
			Assert.AreEqual(expected: value, actual: readValue);
		}

		public virtual void TryReadSignedWithMinimumMaxOverTestMethod(T value, byte[] serialized)
		{
			T readValue;
			int read;
			Span<byte> buf = stackalloc byte[MaxMinBytesToWrite];
			bool success;

			GetSignedWithMinimumExpected(sample: serialized, expected: buf);
			success=TryRead(source: buf, value: out readValue, read: out read);
			Assert.IsTrue(success);
			Assert.AreEqual(expected: buf.Length, actual: read);
			Assert.AreEqual(expected: value, actual: readValue);
		}
		#endregion
	}
}