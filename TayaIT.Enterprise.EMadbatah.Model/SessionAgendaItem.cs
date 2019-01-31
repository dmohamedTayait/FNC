using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TayaIT.Enterprise.EMadbatah.Model
{
    public class SessionAgendaItem
    {

        public SessionAgendaItem()
        {
            SubAgendaItems = new List<SessionAgendaItem>();
        }

        //public SessionAgendaItem(int? eparlimentID, int? eparlimentParentID, string text, int? order)
        //{
        //    SubAgendaItems = new List<SessionAgendaItem>();
        //    EparlimentID = eparlimentID;
        //    EparlimentParentID = eparlimentParentID;
        //    Text = text;
        //    Order = order;
        //}

        public SessionAgendaItem(int? eparlimentID, int? eparlimentParentID, string text, int? order, string qFrom, string qTo)
        {
            SubAgendaItems = new List<SessionAgendaItem>();
            EparlimentID = eparlimentID;
            EparlimentParentID = eparlimentParentID;
            Text = text;
            Order = order;
            QuestionTo = qTo;
            QuestionFrom = qFrom;
        }

        public SessionAgendaItem(long id, int? eparlimentID, int? eparlimentParentID, string text, int? order, string qFrom, string qTo)
        {
            ID = id;
            SubAgendaItems = new List<SessionAgendaItem>();
            EparlimentID = eparlimentID;
            EparlimentParentID = eparlimentParentID;
            Text = text;
            Order = order;
            QuestionTo = qTo;
            QuestionFrom = qFrom;
        }

        //public SessionAgendaItem(int? eparlimentID, int? eparlimentParentID, string text, int? order, List<SessionAgendaItem> subAgendaItems)
        //{
        //    SubAgendaItems = new List<SessionAgendaItem>();
        //    EparlimentID = eparlimentID;
        //    EparlimentParentID = eparlimentParentID;
        //    Order = order;
        //    Text = text;

        //}

        public SessionAgendaItem(long id, int? eparlimentID, int? eparlimentParentID, string text, int? order, bool? isCustom)
        {
            SubAgendaItems = new List<SessionAgendaItem>();
            EparlimentID = eparlimentID;
            EparlimentParentID = eparlimentParentID;
            ID = id;
            Text = text;
            Order = order;
            IsCustom = isCustom;
            //QuestionTo = qTo;
            //QuestionFrom = qFrom;
        }
        public SessionAgendaItem(long agendaItemID, string text)
        {
            ID = agendaItemID;
            Text = text;
        }
        public long ID { get; set; }
        public int? EparlimentID { get; set; }
        public int? EparlimentParentID { get; set; }
        public string Text { get; set; }
        public List<SessionAgendaItem> SubAgendaItems { get; set; }
        public bool IsGroupSubAgendaItems { get; set; }

        public int? Order { get; set; }
        public string QuestionFrom { get; set; }
        public string QuestionTo { get; set; }

        public bool? IsCustom { get; set; }

        public bool IsQuestion 
        {
            get 
            {
                if (string.IsNullOrEmpty(QuestionFrom) && string.IsNullOrEmpty(QuestionTo))
                    return false;
                else
                    return true;
            }
        }
    }
}
