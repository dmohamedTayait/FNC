using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Web;
using TayaIT.Enterprise.EMadbatah.BLL;
using TayaIT.Enterprise.EMadbatah.Model;
using TayaIT.Enterprise.EMadbatah.Util;
using TayaIT.Enterprise.EMadbatah.Util.Web;
using TayaIT.Enterprise.EMadbatah.DAL;
using TayaIT.Enterprise.EMadbatah.Config;


namespace TayaIT.Enterprise.EMadbatah.Web.Framework
{
    class ReorderHandler : BaseHandler
    {
        #region variables
        int firstItemPosition;
        int secondItemPosition;
        int objectType;
        #endregion
        
        #region properties
        public int FirstItemPosition
        {
            get {return this.firstItemPosition; }
            set{this.firstItemPosition = value;}
        }
        public int SecondItemPosition
        {
            get { return this.secondItemPosition; }
            set { this.secondItemPosition = value; }
        }
        public int ObjectType
        {
            get { return this.objectType; }
            set { this.objectType = value; }
        }
        #endregion
        #region IHttpHandler Members

        protected override void HandleRequest()
        {

            //if (_context.Request.ContentType.Contains("json")) // these are ajax json posts 
            //{
            WebFunctions.SessionsFunctions function;

            if (AjaxFunctionName != null && Enum.TryParse<WebFunctions.SessionsFunctions>(AjaxFunctionName, true, out function))
            {
                string jsonStringOut = null;
                long reorderID1;
                long reorderID2;
                int newOrder1 ;
                int newOrder2;
                long sessionID;

                switch (function)
                {
                    case WebFunctions.SessionsFunctions.ReorderSessionFiles:
                        if (ReorderID1 != null && long.TryParse(ReorderID1, out reorderID1)
                            && NewOrder1 != null && int.TryParse(NewOrder1, out newOrder1)
                            && ReorderID2 != null && long.TryParse(ReorderID2, out reorderID2)
                            && NewOrder2 != null && int.TryParse(NewOrder2, out newOrder2)
                            && SessionID != null && long.TryParse(SessionID, out sessionID))
                            {
                                bool sucess = SessionFileHelper.UpdateSessionFileOrder(reorderID1, newOrder1, reorderID2, newOrder2);
                                if (sucess)
                                {
                                    SessionDetails sd = EMadbatahFacade.GetSessionBySessionID(sessionID);
                                    if (sd.Status == Model.SessionStatus.Approved)
                                    {
                                        EMadbatahFacade.UpdateSessionStatus(sessionID, Model.SessionStatus.Completed);
                                    }
                                }

                                jsonStringOut = SerializeObjectInJSON(sucess);                               
                            }                                
                        break;
                    case WebFunctions.SessionsFunctions.ReOrderAttachment:
                        if (ReorderID1 != null && long.TryParse(ReorderID1, out reorderID1)
                            && ReorderID2 != null && long.TryParse(ReorderID2, out reorderID2)
                                && NewOrder1 != null && int.TryParse(NewOrder1, out newOrder1)
                                && NewOrder2 != null && int.TryParse(NewOrder2, out newOrder2)
                            && SessionID != null && long.TryParse(SessionID, out sessionID))
                        {
                            bool sucess = AttachmentHelper.UpdateAttachementFileOrder(reorderID1, newOrder1, reorderID2, newOrder2);
                            if (sucess)
                            {
                                SessionDetails sd = EMadbatahFacade.GetSessionBySessionID(sessionID);
                                if (sd.Status == Model.SessionStatus.Approved)
                                {
                                    EMadbatahFacade.UpdateSessionStatus(sessionID, Model.SessionStatus.Completed);
                                }
                            }
                            jsonStringOut = SerializeObjectInJSON(sucess);
                        }
                        break;
                }
                if (jsonStringOut != null)
                {
                    _context.Response.AddHeader("Encoding", "UTF-8");
                    _context.Response.Write(jsonStringOut);
                }
            }
        }
        

        
        #endregion

        #region methods
        //private void SwapObjectsPositions(long id, int order, int objectType)
        //{
        //    switch (objectType)
        //    {
        //        case (int)FileType.Attachement:
        //            AttachmentHelper.UpdateAttachementOrder(id, order);
        //            break;
        //        case (int)FileType.SessionFile:
                    
        //            SessionFileHelper.UpdateSessionFileOrder(id, order);
        //            break;
        //        default:
        //            break;
        //    }
        //}

        public string NewOrder1
        {
            get
            {
                return WebHelper.GetQSValue(Constants.QSKeyNames.NEW_ORDER1, _context);
            }
        }
        public string NewOrder2
        {
            get
            {
                return WebHelper.GetQSValue(Constants.QSKeyNames.NEW_ORDER2, _context);
            }
        }

        public string ReorderID1
        {
            get
            {
                return WebHelper.GetQSValue(Constants.QSKeyNames.REORDER_ID1, _context);
            }
        }
        public string ReorderID2
        {
            get
            {
                return WebHelper.GetQSValue(Constants.QSKeyNames.REORDER_ID2, _context);
            }
        }

        #endregion
    }
}
