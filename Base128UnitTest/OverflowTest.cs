using System;
using System.Collections.Generic;
using System.Text;

namespace WojciechMikołajewicz.Base128UnitTest
{
	class OverflowTest
	{
		public byte[] Serialized { get; }

		public OverflowTest(byte[] serialized)
		{
			this.Serialized=serialized;
		}
	}
}