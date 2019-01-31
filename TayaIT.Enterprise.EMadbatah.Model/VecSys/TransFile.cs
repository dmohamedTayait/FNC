using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TayaIT.Enterprise.EMadbatah.Model.VecSys
{
    public class TransFile
    {
        public TransFile()
        {
            SpeakerList = new List<Speaker>();
            SpeechSegmentList = new List<SpeechSegment>();
        }

        public List<Speaker> SpeakerList 
        { 
            get; 
            set; 
        }
        public List<SpeechSegment> SpeechSegmentList { get; set; }
    }
}
