using System.Configuration;
using System.Diagnostics;
using System.IO;
using System;

namespace RoomService.NTrace
{
public class LocalStorageXmlWriterTraceListener : XmlWriterTraceListener
    {        
        public LocalStorageXmlWriterTraceListener(string initializeData)
            : base(initializeData)
        {
        }

        public LocalStorageXmlWriterTraceListener(string initializeData, string name)
            : base(initializeData, name)
        {
        }	  
    }
}
