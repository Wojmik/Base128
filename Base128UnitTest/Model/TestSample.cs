using System;
using System.Collections.Generic;
using System.Text;

namespace WojciechMikołajewicz.Base128UnitTest.Model
{
	class TestSample<T>
	{
		public T Value { get; }

		public byte[] Serialized { get; }

		public TestSample(T value, byte[] serialized)
		{
			this.Value=value;
			this.Serialized=serialized;
		}
	}
}