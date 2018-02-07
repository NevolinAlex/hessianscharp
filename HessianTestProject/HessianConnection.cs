using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hessiancsharp.client;
using NTransit.Business.Java.Service.dto;

namespace HessianTestProject
{
	public class HessianConnection
	{
		public ITestConnectionController _tester;
		public HessianConnection()
		{
				CHessianProxyFactory factory = new CHessianProxyFactory();
				Type testConnType = typeof(ITestConnectionController);
				_tester = (ITestConnectionController)factory.Create(testConnType, InitUrlOfService(testConnType));
		}
		public static string InitUrlOfService(Type type)
		{
			string url = @"http://localhost:8090/";
			url += url[url.Length - 1] == '/' ? "" : "/";
			return url + GetHessianAttribute(type).Url;
		}

		public static HessianAttribute GetHessianAttribute(Type type)
		{
			return (HessianAttribute)Attribute.GetCustomAttribute(type, typeof(HessianAttribute));
		}
	}
	public class HessianAttribute : Attribute
	{
		public string Url { get; set; }

		public HessianAttribute(string url)
		{
			this.Url = url;
		}
	}

	[Hessian(@"testConnection")]
	public interface ITestConnectionController
	{
		bool testConnection();

		TestDto getTestDto();

		object getObjectCopy(object document);

	}

}
