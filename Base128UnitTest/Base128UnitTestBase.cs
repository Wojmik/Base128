using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace WojciechMikołajewicz.Base128UnitTest
{
	public abstract class Base128UnitTestBase<T>
		where T : struct
	{
		protected abstract bool TryWrite(Span<byte> destination, T value, out int written);

		protected abstract bool TryRead(Span<byte> source, out T value, out int read);

		protected abstract int GetRequiredBytes(T value);

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
		}

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
		}

		public virtual void TryReadOverflowTestMethod(byte[] serialized)
		{
			T readValue;
			int read;

			Assert.ThrowsException<OverflowException>(() => TryRead(source: serialized, value: out readValue, read: out read));
		}

		public virtual void GetRequiredBytesTestMethod(T value, byte[] serialized)
		{
			int requiredBytes;

			requiredBytes=GetRequiredBytes(value);

			Assert.AreEqual(expected: serialized.Length, actual: requiredBytes);
		}
	}
}