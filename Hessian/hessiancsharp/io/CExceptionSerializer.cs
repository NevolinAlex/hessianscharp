using System;
using System.Collections; using System.Collections.Generic;
using System.Reflection;

namespace hessiancsharp.io
{
	public class CExceptionSerializer : CObjectSerializer
	{
		private List<Object> m_serPropertys = new List<Object>();
		public CExceptionSerializer():base(typeof(Exception))
		{
			m_serPropertys = GetSerializablePropertys();
		}

		public static List<Object> GetSerializablePropertys()
		{
			Type type = typeof(Exception);
			List<Object> serPropertys = new List<Object>();
			PropertyInfo [] propertys = type.GetProperties(BindingFlags.Public|
												BindingFlags.Instance|
												BindingFlags.NonPublic|
												BindingFlags.GetProperty |
												BindingFlags.DeclaredOnly);
			if (propertys!=null) 
			{
				for (int i = 0; i<propertys.Length; i++)
				{
					if ((!serPropertys.Contains(propertys[i]))   && (propertys[i].PropertyType != typeof(System.IntPtr)))
					{
						serPropertys.Add(propertys[i]);
					}
				}
			}
			return serPropertys;
		}

		public override List<Object> GetSerializablePropertyList()
		{
			return m_serPropertys;
		}

	}
}
