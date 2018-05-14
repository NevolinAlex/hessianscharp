/*
***************************************************************************************************** 
* HessianCharp - The .Net implementation of the Hessian Binary Web Service Protocol (www.caucho.com) 
* Copyright (C) 2004-2005  by D. Minich, V. Byelyenkiy, A. Voltmann
* http://www.hessiancsharp.com
*
* This library is free software; you can redistribute it and/or
* modify it under the terms of the GNU Lesser General Public
* License as published by the Free Software Foundation; either
* version 2.1 of the License, or (at your option) any later version.
*
* This library is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
* Lesser General Public License for more details.
*
* You should have received a copy of the GNU Lesser General Public
* License along with this library; if not, write to the Free Software
* Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
* 
* You can find the GNU Lesser General Public here
* http://www.gnu.org/licenses/lgpl.html
* or in the license.txt file in your source directory.
******************************************************************************************************  
* You can find all contact information on http://www.hessiancsharp.com	
******************************************************************************************************
*
*
******************************************************************************************************
* Last change: 2005-08-14
* By Andre Voltmann	
* Licence added.
******************************************************************************************************
*/

#region NAMESPACES
using System;
using System.Collections; using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;

#endregion

namespace hessiancsharp.io
{
	/// <summary>
	/// Serializing an object for known object types.
	/// Analog to the JavaSerializer - Class from 
	/// the Hessian implementation
	/// </summary>
	public class CObjectSerializer : AbstractSerializer
	{
		#region CLASS_FIELDS
		/// <summary>
		/// Propertys of the objectType
		/// </summary>
		private List<Object> m_alProperties = new List<Object>();
		#endregion
		#region CONSTRUCTORS
		/// <summary>
		/// Construktor.
		/// </summary>
		/// <param name="type">Type of the objects, that have to be
		/// serialized</param>
		public CObjectSerializer(Type type)
		{
			for (; type!=null; type = type.BaseType) 
			{
				PropertyInfo [] properties = type.GetProperties(BindingFlags.Public|
					BindingFlags.Instance|
					BindingFlags.NonPublic|
					BindingFlags.GetProperty |
					BindingFlags.DeclaredOnly);
				if (properties != null) 
				{
					for (int i = 0; i< properties.Length; i++)
					{
						//if ((properties[i].Attributes & PropertyAttributes.NotSerialized) == 0)
						if (!Attribute.IsDefined(properties[i], typeof(XmlIgnoreAttribute)))
							if (!this.m_alProperties.Contains(properties[i])) 
						    {
							    this.m_alProperties.Add(properties[i]);
						    }
					}
				}
				
			}
		}
		#endregion
		#region PUBLIC_METHODS
		
        /// <summary>
        /// Serialiaztion of objects
        /// </summary>
        /// <param name="obj">Object to serialize</param>
        /// <param name="abstractHessianOutput">HessianOutput - Instance</param>
        public override void WriteObject(object obj, AbstractHessianOutput abstractHessianOutput)
        {
            if (abstractHessianOutput.AddRef(obj))
                return;
            Type type = obj.GetType();
            string typeName = type.FullName;
            object[] customAttributes = type.GetCustomAttributes(typeof(CTypeNameAttribute), false);
            if (customAttributes.Length > 0)
              typeName = ((CTypeNameAttribute)customAttributes[0]).Name;
            abstractHessianOutput.WriteMapBegin(typeName);
            List<Object> serPropertys = GetSerializablePropertyList();
            for (int i = 0; i < serPropertys.Count; i++)
            {
                PropertyInfo property = (PropertyInfo)serPropertys[i];
                abstractHessianOutput.WriteString(property.Name);
                abstractHessianOutput.WriteObject(property.GetValue(obj, null));
            }
            abstractHessianOutput.WriteMapEnd();
        }

        public virtual List<Object> GetSerializablePropertyList()
        {
            return m_alProperties;
        }


		#endregion
	}
}
