using System;
using System.Collections; using System.Collections.Generic;
using System.Reflection;

namespace hessiancsharp.io
{
	/// <summary>
	/// Summary description for CExceptionDeserializer.
	/// </summary>
	public class CExceptionDeserializer : CObjectDeserializer
	{
		private IDictionary m_deserPropertys = new Dictionary<Object, Object>();
		private Type m_type = null;
		public CExceptionDeserializer(Type type):base(type)
		{
			List<Object> propertyList = CExceptionSerializer.GetSerializablePropertys();
			foreach (PropertyInfo propertyInfo in propertyList)
			{
				m_deserPropertys[propertyInfo.Name] = propertyInfo;
			}
			m_type = type;
		}

		public override IDictionary GetDeserializablePropertys()
		{
			return m_deserPropertys;
		}

		public override object ReadMap(AbstractHessianInput abstractHessianInput)
		{
			Dictionary<Object, Object> propertyValueMap = new Dictionary<Object, Object>();
			string _message = null;
			Exception _innerException = null;
			while (! abstractHessianInput.IsEnd()) 
			{
				object objKey = abstractHessianInput.ReadObject();
				if(objKey != null)
				{
					IDictionary deserPropertys = GetDeserializablePropertys();
                    PropertyInfo property = (PropertyInfo) deserPropertys[objKey];
                    // try to convert a Java Exception in a .NET exception
					if(objKey.ToString() == "_message" || objKey.ToString() == "detailMessage")
					{
                        if (property != null)
						    _message = abstractHessianInput.ReadObject(property.PropertyType) as string;
                        else
                            _message = abstractHessianInput.ReadObject().ToString();
					}
					else if(objKey.ToString() == "_innerException" || objKey.ToString() == "cause")
					{
                        try
                        {
                            if (property != null)
                                _innerException = abstractHessianInput.ReadObject(property.PropertyType) as Exception;
                            else
                                _innerException = abstractHessianInput.ReadObject(typeof(Exception)) as Exception;
                        }
                        catch (Exception)
                        {
                            // als Cause ist bei Java gerne mal eine zirkuläre Referenz auf die Exception selbst
                            // angegeben. Das klappt nicht, weil die Referenz noch nicht registriert ist,
                            // weil der Typ noch nicht klar ist (s.u.)
                        }
                    }
					else
					{
                        if (property != null)
                        {
                            object objPropertyValue = abstractHessianInput.ReadObject(property.PropertyType);
                            propertyValueMap.Add(property, objPropertyValue);
                        } else
                            // ignore (z. B. Exception Stacktrace "stackTrace" von Java)
                            abstractHessianInput.ReadObject();
						//property.SetValue(result, objPropertyValue);
					}
				}
				
			}
			abstractHessianInput.ReadEnd();

			object result =  null;
			try
			{
#if COMPACT_FRAMEWORK
            	//CF TODO: tbd
#else
				try
				{
					result = Activator.CreateInstance(this.m_type, new object[2]{_message, _innerException});	
				}
				catch(Exception)
				{
					try
					{
						result = Activator.CreateInstance(this.m_type, new object[1]{_innerException});		
					}
					catch(Exception)
					{
						try
						{
							result = Activator.CreateInstance(this.m_type, new object[1]{_message});			
						}
						catch(Exception)
						{
							result = Activator.CreateInstance(this.m_type);			
						}
					}
                }
#endif

            }
			catch(Exception)
			{
				result = new Exception(_message, _innerException);
			}
			foreach (KeyValuePair<object, object> entry in propertyValueMap)
			{
				PropertyInfo propertyInfo = (PropertyInfo) entry.Key;
				object value = entry.Value;
				try {propertyInfo.SetValue(result, value, null);} catch(Exception){}
			}

            // besser spät als gar nicht.
            int refer = abstractHessianInput.AddRef(result);


			return result;
		}
	}
}
