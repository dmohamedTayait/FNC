using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using TayaIT.Enterprise.EMadbatah.Model;
using TayaIT.Enterprise.EMadbatah.Model.VecSys;

namespace TayaIT.Enterprise.EMadbatah.Vecsys
{
    public class VecsysParser
    {
        public static TransFile ParseTransXml(string filePath)
        {
            TransFile sessionFile = new TransFile();

            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            XmlNodeList speakersNodes = doc.SelectNodes("//SpeakerList/Speaker");
            foreach (XmlNode node in speakersNodes)
            {
                Speaker spk = new Speaker();
                spk.ch = node.Attributes["ch"].Value;
                spk.dur = double.Parse(node.Attributes["dur"].Value);
                spk.gender = int.Parse(node.Attributes["gender"].Value);
                spk.lang = node.Attributes["lang"].Value;
                spk.lconf = double.Parse(node.Attributes["lconf"].Value);
                spk.nw = double.Parse(node.Attributes["nw"].Value);
                spk.spkid = node.Attributes["spkid"].Value;
                spk.tconf = double.Parse(node.Attributes["tconf"].Value);

                sessionFile.SpeakerList.Add(spk);
            }

            XmlNodeList segmentNodes = doc.SelectNodes("//SegmentList/SpeechSegment");
            foreach (XmlNode node in segmentNodes)
            {
                SpeechSegment seg = new SpeechSegment();
                seg.ch = node.Attributes["ch"].Value;
                seg.etime = double.Parse(node.Attributes["etime"].Value);
                seg.lang = node.Attributes["lang"].Value;
                seg.lconf = double.Parse(node.Attributes["lconf"].Value);
                seg.sconf = double.Parse(node.Attributes["sconf"].Value);
                seg.spkid = node.Attributes["spkid"].Value;
                seg.stime = double.Parse(node.Attributes["stime"].Value);
                seg.trs = double.Parse(node.Attributes["trs"].Value);
                seg.text = node.InnerText.Trim();

                //XmlNodeList wordNodes = node.SelectNodes("/Word");

                //XmlDocument subDoc = new XmlDocument();
                //subDoc.LoadXml("<Words>\r\n" + node.InnerXml.Trim() + "</Words>");
                //XmlNodeList wordNodes =   subDoc.SelectNodes("//Word");
                List<Word> words = new List<Word>();
                foreach (XmlNode subNode in node.ChildNodes)
                {
                    Word word = new Word();
                    word.conf = double.Parse(subNode.Attributes["conf"].Value);
                    word.dur = double.Parse(subNode.Attributes["dur"].Value);
                    word.stime = double.Parse(subNode.Attributes["stime"].Value);
                    word.value = subNode.InnerText.Trim();
                    words.Add(word);
                }
                seg.words = words;
                sessionFile.SpeechSegmentList.Add(seg);
            }
            return sessionFile;
        }

        public static List<Paragraph> combineSegments(List<SpeechSegment> speechSegments)
        {
            List<Paragraph> paragraphs = new List<Paragraph>();
            if (speechSegments.Count == 1)
            {
                Paragraph paragraph = new Paragraph();
                paragraph.speechSegmentsList.Add(speechSegments[0]);
                paragraphs.Add(paragraph);
                return paragraphs;
            }
            int sumTaken = 0;
            for (int i = 0; i < speechSegments.Count - 1; )
            {
                Paragraph paragraph = new Paragraph();
                paragraph.speechSegmentsList.Add(speechSegments[i]);

                string curSpkID = speechSegments[i].spkid;
                i++;
                
                while (i < speechSegments.Count && curSpkID == speechSegments[i].spkid )
                {
                    paragraph.speechSegmentsList.Add(speechSegments[i]);
                     curSpkID = speechSegments[i].spkid;
                    i++;
                }
                paragraphs.Add(paragraph);
                sumTaken += paragraph.speechSegmentsList.Count;
                if (sumTaken < speechSegments.Count && i == speechSegments.Count - 1)
                {
                    Paragraph lastParagraph = new Paragraph();
                    lastParagraph.speechSegmentsList.Add(speechSegments[i]);
                    paragraphs.Add(lastParagraph);
                }
                
            }

            return paragraphs;
        }

    }
}
