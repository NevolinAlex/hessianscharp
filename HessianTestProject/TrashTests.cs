using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hessiancsharp.io;
using HessianTestProject.TestClasses;
using NTransit.Business.Java.Service.dto;
using NUnit.Framework;
using TestEnum = NTransit.Business.Java.Service.dto.TestEnum;

namespace HessianTestProject
{
	public class TrashTests
	{
		private ITestConnectionController _hessianController;
		[Test]
		public void TestEnumSerializing()
		{
			MemoryStream stream = new MemoryStream();
			CHessianOutput out1 = new CHessianOutput(stream);

			TestDto obj = new TestDto();

			out1.StartReply();
			out1.WriteObject(obj);
			out1.CompleteReply();
			byte[] data = stream.ToArray();

			File.WriteAllBytes(@"C:\Users\disap\Desktop\Projects\hessianscharp\HessianTestProject\test.txt", data);
			CHessianInput input = new CHessianInput(new MemoryStream(data));
			var result = input.ReadReply(typeof(TestDto));
			Assert.IsTrue(true);
		}
		[SetUp]
		public void Init()
		{
			_hessianController = new HessianConnection()._tester;
		}
		[Test]
		public void TestGettingObject()
		{
			var dto = new TestDto();
			dto.TestEnum = TestEnum.yes;
			dto.TestDouble = 1;

			object obj = _hessianController.getObjectCopy(dto);
		}
	}
}
