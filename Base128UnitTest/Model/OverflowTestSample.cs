using System;
using System.Collections.Generic;
using System.Text;

namespace WojciechMikołajewicz.Base128UnitTest.Model
{
	class OverflowTestSample
	{
		public byte[] Serialized { get; }

		public OverflowTestSample(byte[] serialized)
		{
			this.Serialized=serialized;
		}
	}
}