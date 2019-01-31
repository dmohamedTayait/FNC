using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TayaIT.Enterprise.EMadbatah.OpenXml.Word
{
    public class DocumentNotOpenException : ApplicationException
    {
        public DocumentNotOpenException(string message)
            : base(message)
        {
        }

        public DocumentNotOpenException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
