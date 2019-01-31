using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TayaIT.Enterprise.EMadbatah.Model.VecSys
{
    public class Paragraph
    {
        public Paragraph()
        {
            speechSegmentsList = new List<SpeechSegment>();
        }
        public List<SpeechSegment> speechSegmentsList { get; set; }
    }
}
