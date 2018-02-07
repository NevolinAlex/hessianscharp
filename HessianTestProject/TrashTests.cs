using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hessiancsharp.io;
using HessianTestProject.TestClasses;
using NUnit.Framework;

namespace HessianTestProject
{
    public class TrashTests
    {
	    [Test]
	    public void TestEnumSerializing()
	    {
			MemoryStream stream = new MemoryStream();
		    CHessianOutput out1 = new CHessianOutput(stream);
		    ObjectWithEnum obj = new ObjectWithEnum();
		    out1.StartReply();
		    out1.WriteObject(obj);
		    out1.CompleteReply();
		    byte[] data = stream.ToArray();
		    File.WriteAllBytes(@"C:\Users\disap\Desktop\Projects\test.txt", data);
		    CHessianInput input = new CHessianInput(new MemoryStream(data));
		    var obj = input.ReadReply(typeof(DocumentReferenceInfoType));
		}
    }
}
