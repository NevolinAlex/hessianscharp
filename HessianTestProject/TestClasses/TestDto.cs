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
		private object result;
		private decimal? testDouble;

		public object Result
		{
			get => result;
			set => this.result = value;
		}

		public decimal? TestDouble
		{
			get => testDouble;
			set => this.testDouble = value;
		}

		private TestEnum testEnum;

		public TestEnum TestEnum
		{
			get => testEnum;
			set => this.testEnum = value;
		}

		public TestDto()
		{
			testEnum = TestEnum.enumerating;
		}
	}

	public enum TestEnum
	{
		enumerating,
		testing,
		cases
	}
}