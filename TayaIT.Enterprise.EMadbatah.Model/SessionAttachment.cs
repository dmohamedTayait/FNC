using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TayaIT.Enterprise.EMadbatah.Model
{
    public class SessionAttachment
    {
        public SessionAttachment(long id, string name, FileExtensionType fileType, int order, long sessionID, byte[] fileContent) 
        {
            Name = name;
            ID = id;
            Order = order;
            SessionID = sessionID;
            FileType = fileType;
            FileContent = fileContent;
        }

        public string Name { get; set; }
        public long ID { get; set; }
        public int Order { get; set; }
        public long SessionID { get; set; }
        public FileExtensionType FileType { get; set; }
        public byte[] FileContent { get; set; }

    }
}
