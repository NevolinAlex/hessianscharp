using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTransit.Business.Java.Service.dto
{
	[Serializable]
	public class TestDto
	{
		private decimal? testDouble;

		public decimal? TestDouble { get; set; }

		private TestEnum testEnum;

		public TestEnum TestEnum { get; set; }

		public TestDto()
		{
			//testEnum = TestEnum.nope;
			//TestEnum = TestEnum.yes;
			//testDouble = 0;
			//TestDouble = 1;
		}

		public void SetTestEnum(TestEnum enumValue)
		{
			testEnum = enumValue;
		}
		public void SetTestDouble(TestEnum enumValue)
		{
			testEnum = enumValue;
		}
	}

	public enum TestEnum
	{
		enumerating,
		testing,
		cases,
		nope,
		yes
	}
}