using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HessianTestProject.TestClasses
{
	class ObjectWithEnum
	{
		private TestEnum testEnum;

		public ObjectWithEnum()
		{
			testEnum = TestEnum.testing;
		}

		public override bool Equals(object obj)
		{
			var @enum = obj as ObjectWithEnum;
			return @enum != null &&
				   testEnum == @enum.testEnum;
		}

		public override int GetHashCode()
		{
			return 337929216 + testEnum.GetHashCode();
		}
	}
}
