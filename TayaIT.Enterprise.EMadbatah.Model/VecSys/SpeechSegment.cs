using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TayaIT.Enterprise.EMadbatah.Model.VecSys
{
    public class SpeechSegment
    {
        public SpeechSegment()
        {
            words = new List<Word>();
        }
        public string ch { get; set; }
        public double sconf { get; set; }
        public double stime { get; set; }
        public double etime { get; set; }
        public string spkid { get; set; }
        public string lang { get; set; }
        public double lconf { get; set; }
        public double trs { get; set; }
        public List<Word> words { get; set; }
        public string text { get; set; }
    }
}
