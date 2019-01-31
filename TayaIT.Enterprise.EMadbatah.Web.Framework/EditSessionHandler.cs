using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.Script.Serialization;
using TayaIT.Enterprise.EMadbatah.Model;
using TayaIT.Enterprise.EMadbatah.BLL;
using TayaIT.Enterprise.EMadbatah.Config;
using TayaIT.Enterprise.EMadbatah.Util.Web;
using System.Collections;
using TayaIT.Enterprise.EMadbatah.DAL;
using TayaIT.Enterprise.EMadbatah.Model.VecSys;
using TayaIT.Enterprise.EMadbatah.Vecsys;
using System.Xml;
using TayaIT.Enterprise.EMadbatah.Util;

namespace TayaIT.Enterprise.EMadbatah.Web.Framework
{
    public class EditSessionHandler : BaseHandler
    {
        public string AgendaID
        {
            get
            {
                return WebHelper.GetQSValue(Constants.QSKeyNames.AGENDA_ID, _context);
            }
        }
        public string AgendaItemText
        {
            get
            {
                return WebHelper.GetQSValue(Constants.QSKeyNames.AGENDA_ITEM_TEXT, _context);
            }
        }
        // variables to load stored session values in
        int FragOrder ;
        //bool firstFrag = false;
        //bool lastFrag = false;
        double stime,etime,duration;
        long Session_ID ,Session_File_ID;
        Hashtable current_session_info;
        protected override void HandleRequest()
        {
            WebFunctions.EditWizard function;
            if (AjaxFunctionName != null && Enum.TryParse<WebFunctions.EditWizard>(AjaxFunctionName, true, out function))
            {
                string jsonStringOut = null;
                long agendItemID;
                long attendantID;
                switch (function)
                {
                    case WebFunctions.EditWizard.GetSpeakerJobTitle:
                        {
                            if (AttendantID != null && long.TryParse(AttendantID, out attendantID))
                            {
                                Attendant att = AttendantHelper.GetAttendantById(attendantID);
                                if(att != null)
                                    jsonStringOut = SerializeObjectInJSON(att.JobTitle);
                                else
                                    jsonStringOut = SerializeObjectInJSON("error");
                                //List<SessionAgendaItem> items = EditorFacade.GetAgendaSubItemsByAgendaID(agendItemID);
                                //jsonStringOut = SerializeObjectInJSON(items);
                                break;
                            }
                            else
                                break;
                        }

                    case WebFunctions.EditWizard.GetAgendaSubItems:
                        {
                            if (AgendaID != null && long.TryParse(AgendaID, out agendItemID))
                            {
                                List<SessionAgendaItem> items = EditorFacade.GetAgendaSubItemsByAgendaID(agendItemID);
                                jsonStringOut = SerializeObjectInJSON(items);
                                break;
                            }
                            else
                                break;
                        }
                    case WebFunctions.EditWizard.AddAgendaItem:
                        {
                            long parentAgendaItemID = -1;
                            if (AgendaItemText != null && AgendaItemID != null && long.TryParse(AgendaItemID,out parentAgendaItemID))
                            {
                                // collect sessionContentItem attributes/values
                                loadSessionValues();
                                long id = EditorFacade.AddNewAgendaItem(AgendaItemText,Session_ID, parentAgendaItemID);
                                jsonStringOut = SerializeObjectInJSON(id);
                                break;
                            }
                            else
                                break;
                        }
                    case WebFunctions.EditWizard.UpdateAgendaItem:
                        {
                            if (AgendaItemText != null && long.TryParse(AgendaID, out agendItemID))
                            {
                                string res = EditorFacade.UpdateAgendaItem(agendItemID, AgendaItemText);
                                jsonStringOut = SerializeObjectInJSON(res);
                                break;
                            }
                            else
                                break;
                        }
                    case WebFunctions.EditWizard.DoNext:
                        {
                            Session currentSession = null;
                                Hashtable result = SaveAndExit(out currentSession);; 
                                if(result["Message"] == "fail")
                                {
                                    jsonStringOut = SerializeObjectInJSON(result);
                                    break;
                                }
                                else{
                                    try
                                    {
                                        SessionAudioFile file = SessionFileFacade.GetSessionFilesByID(Session_File_ID);
                                        int NextItemOrderID = FragOrder + 1;
                                        SessionContentItem nextContentItem = SessionContentItemFacade.GetSessionContentItemByIdAndFragmentOrder(Session_File_ID, NextItemOrderID);
                                        if (nextContentItem == null)
                                        {
                                            nextContentItem = GetContentFromXML(file.Name.ToLower().Replace(".mp3", ".trans.xml"), NextItemOrderID);
                                            current_session_info["IsGroupSubAgendaItems"] = bool.Parse(WebHelper.GetQSValue(Constants.QSKeyNames.IsGroupSubAgendaItems, _context));
                                        }
                                        else
                                        {
                                            current_session_info["PargraphStartTime"] = nextContentItem.StartTime;
                                            current_session_info["PargraphEndTime"] = nextContentItem.EndTime;
                                            current_session_info["SameAsPrevSpeaker"] = nextContentItem.MergedWithPrevious;
                                            current_session_info["IsGroupSubAgendaItems"] = nextContentItem.AgendaItem.IsGroupSubAgendaItems;
                                            current_session_info["Ignored"] = nextContentItem.Ignored;

                                        }
                                        result["PargraphStartTime"] = current_session_info["PargraphStartTime"];
                                        result["PargraphEndTime"] = current_session_info["PargraphEndTime"];
                                        result["SameAsPrevSpeaker"] = current_session_info["SameAsPrevSpeaker"];
                                        result["IsGroupSubAgendaItems"] = current_session_info["IsGroupSubAgendaItems"];
                                        result["Ignored"] = current_session_info["Ignored"];

                                        // compose response[json object] 
                                        SessionContentItem retItem = getReturnedContentItemObject(nextContentItem);
                                        result["Item"] = retItem;
                                        result["ItemFragOrder"] = NextItemOrderID.ToString();
                                        List<AgendaItem> AgendaItems = currentSession.AgendaItems.ToList<AgendaItem>();
                                        List<SessionAgendaItem> retAgendaItems = new List<SessionAgendaItem>();
                                        foreach (AgendaItem agendaItem in AgendaItems)
                                        {
                                            SessionAgendaItem newSessionAgendaItem = new SessionAgendaItem(agendaItem.ID, agendaItem.Name);
                                            newSessionAgendaItem.IsGroupSubAgendaItems = agendaItem.IsGroupSubAgendaItems;
                                            retAgendaItems.Add(newSessionAgendaItem);
                                        }

                                        result["AgendaItems"] = retAgendaItems;
                                        List<SessionAgendaItem> retAgendaSubItems = null;
                                        if (retItem.AgendaItemID != 0)
                                            retAgendaSubItems = EditorFacade.GetAgendaSubItemsByAgendaID(retItem.AgendaItemID);
                                        result["AgendaSubItems"] = retAgendaSubItems;
                                        result["ItemOrder"] = checkItemPosition(file.Name.ToLower().Replace(".mp3", ".trans.xml"), NextItemOrderID);
                                        // save last FragOrder in user session
                                        current_session_info["FragOrderInXml"] = NextItemOrderID;
                                        if (FragOrder == 0)
                                            SessionFileHelper.UpdateSessionFileStatus(Session_File_ID, (int)Model.SessionFileStatus.InProgress,null);
                                    }
                                    catch (Exception ex)
                                    {
                                        result["Message"] = "fail";
                                       
                                    }
                                }
                                
                                jsonStringOut = SerializeObjectInJSON(result);
                                break;
                            
                        }
                    case WebFunctions.EditWizard.DoPrevious:
                        {
                            // collect sessionContentItem attributes/values
                            Session currentSession = null;
                            Hashtable result = SaveAndExit(out currentSession); ;
                              if (result["Message"] == "fail")
                              {
                                  jsonStringOut = SerializeObjectInJSON(result);
                                  break;
                              }
                              else
                              {

                                  //loadSessionValues();
                                  //Hashtable result = new Hashtable();
                                  int prevItemOrderID = FragOrder - 1;
                                  // check current user privellge
                                  SessionAudioFile file = SessionFileFacade.GetSessionFilesByID(Session_File_ID);
                                  if (CurrentUser.ID == file.UserID)
                                  {
                                      result["Message"] = "success";
                                      // load item from DB or xml
                                      SessionContentItem prevContentItem = SessionContentItemFacade.GetSessionContentItemByIdAndFragmentOrder(Session_File_ID, prevItemOrderID);
                                      if (prevContentItem == null)
                                      {
                                          prevContentItem = GetContentFromXML(file.Name.ToLower().Replace(".mp3", ".trans.xml"), prevItemOrderID);
                                          current_session_info["IsGroupSubAgendaItems"] = bool.Parse(WebHelper.GetQSValue(Constants.QSKeyNames.IsGroupSubAgendaItems, _context));
                                      }
                                      else
                                      {
                                          current_session_info["PargraphStartTime"] = prevContentItem.StartTime;
                                          current_session_info["PargraphEndTime"] = prevContentItem.EndTime;
                                          current_session_info["SameAsPrevSpeaker"] = prevContentItem.MergedWithPrevious;
                                          current_session_info["IsGroupSubAgendaItems"] = prevContentItem.AgendaItem.IsGroupSubAgendaItems;
current_session_info["Ignored"] = prevContentItem.Ignored;

                                          


                                      }
                                      //get prev item from db if exist, meshmesh
                                      if (prevItemOrderID - 1 >= 0)
                                      {
                                          SessionContentItem prevPrevContentItem = SessionContentItemHelper.GetPrevSessionContentItem(Session_File_ID, prevItemOrderID);
                                          if (prevPrevContentItem != null)
                                          {
                                              result["prevAgendaItemID"] = prevPrevContentItem.AgendaItemID;
                                              result["prevAttendantID"] = prevPrevContentItem.AttendantID;
                                              if (prevPrevContentItem.AgendaSubItemID != null)
                                                  result["prevAgendaSubItemID"] = prevPrevContentItem.AgendaSubItemID;
                                              if (prevPrevContentItem.Attendant.JobTitle != null && !string.IsNullOrEmpty(prevPrevContentItem.Attendant.JobTitle))
                                                  result["prevAttendantJobTitle"] = prevPrevContentItem.Attendant.JobTitle;

                                          }
                                      }

                                      result["PargraphStartTime"] = current_session_info["PargraphStartTime"];
                                      result["PargraphEndTime"] = current_session_info["PargraphEndTime"];
                                      result["SameAsPrevSpeaker"] = current_session_info["SameAsPrevSpeaker"];
                                      result["IsGroupSubAgendaItems"] = current_session_info["IsGroupSubAgendaItems"];
                                      result["Ignored"] = current_session_info["Ignored"];

                                      SessionContentItem retItem = getReturnedContentItemObject(prevContentItem);
                                      result["Item"] = retItem;
                                      result["ItemFragOrder"] = prevItemOrderID.ToString();
                                      List<AgendaItem> AgendaItems = currentSession.AgendaItems.ToList<AgendaItem>();
                                      List<SessionAgendaItem> retAgendaItems = new List<SessionAgendaItem>();
                                      foreach (AgendaItem agendaItem in AgendaItems)
                                      {
                                          SessionAgendaItem newSessionAgendaItem = new SessionAgendaItem(agendaItem.ID, agendaItem.Name);
                                          retAgendaItems.Add(newSessionAgendaItem);
                                      }
                                      result["AgendaItems"] = retAgendaItems;
                                      List<SessionAgendaItem> retAgendaSubItems = null;
                                      if (retItem.AgendaItemID != 0)
                                          retAgendaSubItems = EditorFacade.GetAgendaSubItemsByAgendaID(retItem.AgendaItemID);
                                      result["AgendaSubItems"] = retAgendaSubItems;
                                      result["ItemOrder"] = checkItemPosition(file.Name.ToLower().Replace(".mp3", ".trans.xml"), prevItemOrderID);
                                      // save last FragOrder in user session
                                      current_session_info["FragOrderInXml"] = FragOrder - 1;
                                  }
                                  else
                                  {
                                      result["Message"] = "fail";
                                  }
                              }
                            jsonStringOut = SerializeObjectInJSON(result);
                            break;
                        }
                    case WebFunctions.EditWizard.BackToOriginalContent:
                        {                            
                            Hashtable result = new Hashtable();
                            // collect sessionContentItem attributes/values


                            long sfid = -1;
                            long scid = -1;
                            if (SessionContentItemID != null && long.TryParse(SessionContentItemID, out scid) &&
                                SessionFileID != null && long.TryParse(SessionFileID, out sfid))
                            {
                                SessionContentItem sci = SessionContentItemHelper.GetSessionContentItemById(scid);
                                if (sci != null)
                                {
                                    FragOrder = sci.FragOrderInXml;
                                    Session_File_ID = sfid;
                                }

                                //FragOrder = 
                            }
                            else
                            {
                                loadSessionValues();
                            }


                            
                            SessionAudioFile file = SessionFileFacade.GetSessionFilesByID(Session_File_ID);
                            SessionContentItem ContentItem = null;
                            // check current user privellge
                            //if (CurrentUser.ID == file.UserID || CurrentUser.Role == UserRole.Admin)
                            //{
                                result["Message"] = "success";
                                ContentItem = GetContentFromXML(file.Name.ToLower().Replace(".mp3", ".trans.xml"), FragOrder);
                                result["Item"] = ContentItem;
                                result["ItemOrder"] = checkItemPosition(file.Name.ToLower().Replace(".mp3", ".trans.xml"), FragOrder);
                                // save last FragOrder in user session
                                //current_session_info["FragOrderInXml"] = FragOrder;
                            //}
                            //else
                            //{
                            //    result["Message"] = "fail";
                            //}
                            jsonStringOut = SerializeObjectInJSON(result);
                            break;
                        }
                    case WebFunctions.EditWizard.SaveAndExit:
                        {
                            Session currentSession = null;
                            jsonStringOut = SerializeObjectInJSON(SaveAndExit(out currentSession));
                            break;
                        }
                    case WebFunctions.EditWizard.UpdateSessionFileStatusCompleted:
                        {
                            long sessionID;
                            Session currentSession = null;
                            jsonStringOut = SerializeObjectInJSON(SaveAndExit(out currentSession));
                            SessionFileHelper.UpdateSessionFileStatus(Session_File_ID, (int)Model.SessionFileStatus.Completed,null);
                           
                            if(SessionID != null && long.TryParse(SessionID,out sessionID))
                            {
                                SessionDetails sd = EMadbatahFacade.GetSessionDetailsBySessionID(sessionID);
                                var notCompletedSessionFiles = from sf in sd.SessionFiles
                                                        where ((sf.ID != Session_File_ID) && 
                                                               (sf.Status != Model.SessionFileStatus.Completed))                                                        
                                                        select sf;

                                if (notCompletedSessionFiles.ToList<SessionAudioFile>().Count == 0)
                                {
                                    EMadbatahFacade.UpdateSessionStatus(sessionID, Model.SessionStatus.Completed);
                                }

                               
                                //sd.SessionFiles
                            }
                            break;
                        }
                    case WebFunctions.EditWizard.SplitItem:
                        {
                            long currentFragOrder = -1;
                            
                            if (CurrentFragOrder != null && long.TryParse(CurrentFragOrder, out currentFragOrder) &&
                                XmlFilePath != null && SplittedText != null &&
                                SessionFileID != null && long.TryParse(SessionFileID, out Session_File_ID))
                            {
                                try
                                {
                                   XmlDocument doc = new XmlDocument();
                                    doc.Load(XmlFilePath);

                                    StringBuilder sb = new StringBuilder();
                                    
                                    

                                    XmlDocument spansDoc = new XmlDocument();//nbsp
                                    spansDoc.LoadXml("<doc>" + HttpUtility.HtmlDecode(SplittedText).Replace("&nbsp;", "") + "</doc>");
                                    
                                    //XmlNodeList spanNodes = spansDoc.SelectNodes("//doc/span"); //modefied by usama and saber for text when splitted all spaces are removed
                                    XmlNodeList spanNodes = spansDoc.SelectNodes("//span[@data-stime]");

                                    if (spanNodes.Count == 0)
                                    {
                                        jsonStringOut = SerializeObjectInJSON("false");
                                        break;
                                    }
                                    foreach (XmlNode node in spanNodes)
                                    {
                                        if (node.Attributes["data-stime"] == null)
                                            continue;
                                        sb.Append("<Word stime=\"" + node.Attributes["data-stime"].Value + "\" dur=\"0.25\" conf=\"0.886\">" + node.InnerText + "</Word>");
                                    }

                                    //<Word stime="4.89" dur="0.25" conf="0.886"> لأ </Word>
                                    sb.Append("</SpeechSegment>");
                                    //XmlNode refNode = doc.SelectSingleNode("/AudioDoc/SegmentList/SpeechSegment[2]");

                                    // get 1st random string 
                                    string Rand1 = TextHelper.RandomString(4);
                                    string Rand2 = TextHelper.RandomString(4);
                                    string speakerID = "TT_" + Rand1 + "-" + Rand2;


                                    string header = "<SpeechSegment ch=\"1\" sconf=\"1.00\" stime=\"" + spanNodes[0].Attributes["data-stime"].Value + "\" etime=\"" + spanNodes[spanNodes.Count - 1].Attributes["data-stime"].Value + "\" spkid=\""+speakerID+"\" lang=\"ara-fnc\" lconf=\"1.00\" trs=\"1\">";
                                    XmlDocumentFragment xmlDocFrag = doc.CreateDocumentFragment();
                                    xmlDocFrag.InnerXml = header + sb.ToString();

                                    
                                    //as currentfragorder is out of combine so we have to calculate its relative pos in xml without combine
                                    TransFile tf = new TransFile();
                                    tf = VecsysParser.ParseTransXml(XmlFilePath);
                                    List<Paragraph> p = VecsysParser.combineSegments(tf.SpeechSegmentList);
                                    int curPos = 0;
                                    for (int i = 0; i <= currentFragOrder; i++)
                                        curPos += p[i].speechSegmentsList.Count;
                                    if (curPos == 0)
                                        curPos = 1;//for below curPos - 1
                                    //doc.DocumentElement.ChildNodes[3].ChildNodes.Item(2).AppendChild(xmlDocFrag);
                                    doc.DocumentElement.ChildNodes[3].InsertAfter(xmlDocFrag, doc.DocumentElement.ChildNodes[3].ChildNodes.Item(curPos-1));

                                    //doc.InsertAfter(xmlDocFrag, doc.DocumentElement.ChildNodes[3].ChildNodes[2]);

                                    doc.Save(XmlFilePath);

                                    SessionContentItemHelper.UpdateSessionContentItemOrders(Session_File_ID, currentFragOrder);

                                    current_session_info = (Hashtable)_context.Session["current_session_info"];
                                    current_session_info["PargraphEndTime"] = double.Parse(spanNodes[0].Attributes["data-stime"].Value);
                                    
                                    jsonStringOut = SerializeObjectInJSON("true");
                                }
                                catch (Exception ex)
                                {
                                    jsonStringOut = SerializeObjectInJSON("false");
                                }
                            }
                            break;
                        }
                    default:
                        { break; }
                }
                if (jsonStringOut != null)
                {
                    _context.Response.AddHeader("Encoding", "UTF-8");
                    _context.Response.Write(jsonStringOut);
                }
            }
            else
                return;
        }
        public Hashtable SaveAndExit(out Session session)
        {
            // collect sessionContentItem attributes/values

            //edit mode
            //editmode

            

            loadSessionValues();
            long? AgendaSubItemID = null;
            if (WebHelper.GetQSValue(Constants.QSKeyNames.AgendaSubItemID, _context) != null)
                AgendaSubItemID = long.Parse(WebHelper.GetQSValue(Constants.QSKeyNames.AgendaSubItemID, _context));
            int AgendaItemID;
            long SpeakerID;
            bool SameAsPrevSpeaker = bool.Parse(WebHelper.GetQSValue(Constants.QSKeyNames.SameAsPrevSpeaker, _context));
            bool IsGroupSubAgendaItems = bool.Parse(WebHelper.GetQSValue(Constants.QSKeyNames.IsGroupSubAgendaItems, _context));
            bool Ignored = bool.Parse(WebHelper.GetQSValue(Constants.QSKeyNames.Ignored, _context));
            string SpeakerJob = WebHelper.GetQSValue(Constants.QSKeyNames.SpeakerJob, _context);
            string Text = HttpUtility.UrlDecode(WebHelper.GetQSValue(Constants.QSKeyNames.Text, _context));
            string Comments = WebHelper.GetQSValue(Constants.QSKeyNames.Comments, _context);
            string Footer = WebHelper.GetQSValue(Constants.QSKeyNames.Footer, _context);
            Hashtable result = new Hashtable();
            EditorPageMode editPageMode = EditorPageMode.Edit;

            session = null;

            if (EditPageMode != null)
                Enum.TryParse<EditorPageMode>(EditPageMode, out editPageMode);

            if (int.TryParse(WebHelper.GetQSValue(Constants.QSKeyNames.AgendaItemID, _context), out AgendaItemID) && long.TryParse(WebHelper.GetQSValue(Constants.QSKeyNames.SpeakerID, _context), out SpeakerID)
                )
            {
                // check current user privellge
                SessionAudioFile file = SessionFileFacade.GetSessionFilesByID(Session_File_ID);
                if (CurrentUser.ID == file.UserID || (CurrentUser.ID != file.UserID && CurrentUser.Role != UserRole.DataEntry))
                {
                    result["Message"] = "success";

                    session = SessionHelper.GetSessionDetailsByID(Session_ID);
                     
                    // save item to DB
                    SessionContentItem currContentItem = null;
                    if (editPageMode == EditorPageMode.Edit)
                        currContentItem = SessionContentItemFacade.GetSessionContentItemByIdAndFragmentOrder(Session_File_ID, FragOrder);
                    else
                    {
                        currContentItem = SessionContentItemHelper.GetSessionContentItemById(long.Parse(CorrectSessionContentItemID));
                    }
                    if (currContentItem != null) // update exist
                    {

                        //int status = (int)currContentItem.StatusID;
                        //if ((int)SpeakerID == (int)Model.AttendantType.UnKnown)
                        //    status = (int)Model.SessionContentItemStatus.Rejected;
                        
                        //ibrahim, in case of unknown attendant, we will check the below two conditions only, which will record the user who did the update
                        //and the status will remain rejected in all cases, neither fixed nor modified after approve

                        AgendaItem sessionUnknownItem = AgendaHelper.GetAgendaItemByNameAndSessionID("غير معرف", Session_ID);
                        if (sessionUnknownItem == null)
                        {
                            sessionUnknownItem = new AgendaItem();
                            sessionUnknownItem.IsCustom = true;
                            sessionUnknownItem.Name = "غير معرف";
                            sessionUnknownItem.EParliamentID = null;
                            sessionUnknownItem.Order = null;
                            AgendaHelper.AddAgendaItem(sessionUnknownItem, Session_ID);

                        }

                        //usama march
                        if (currContentItem.AgendaItem.IsGroupSubAgendaItems != IsGroupSubAgendaItems)
                        {
                            AgendaHelper.MarkSubAgendaItemsAsGrouped(AgendaItemID, IsGroupSubAgendaItems);
                            currContentItem.AgendaItem.IsGroupSubAgendaItems = IsGroupSubAgendaItems;
                            current_session_info["IsGroupSubAgendaItems"] = IsGroupSubAgendaItems;
                        }

                        long unknownAgendaItemID = AgendaHelper.GetAgendaItemByNameAndSessionID("غير معرف", Session_ID).ID;

                        if ((int)SpeakerID == (int)Model.AttendantType.UnKnown || AgendaItemID == unknownAgendaItemID )
                        {
                            int status = (int)Model.SessionContentItemStatus.Rejected;
                            if (CurrentUser.Role != UserRole.DataEntry)
                                SessionContentItemHelper.UpdateSessionContentItem(currContentItem.ID, Text, SpeakerID, AgendaItemID, AgendaSubItemID, SpeakerJob, Comments, Footer, status, true, CurrentUser.ID, SameAsPrevSpeaker, Ignored);
                            else
                                SessionContentItemFacade.UpdateSessionContentItem(currContentItem.ID, Text, SpeakerID, AgendaItemID, AgendaSubItemID, SpeakerJob, Comments, Footer, (Model.SessionContentItemStatus)status, SameAsPrevSpeaker, Ignored);
                        
                        }
                        else
                            if (currContentItem.StatusID == (int)Model.SessionContentItemStatus.Approved && session.SessionStatusID == (int)Model.SessionStatus.Approved)
                            {
                                SessionContentItemFacade.UpdateSessionContentItem(currContentItem.ID, Text, SpeakerID, AgendaItemID, AgendaSubItemID, SpeakerJob, Comments, Footer, Model.SessionContentItemStatus.ModefiedAfterApprove, SameAsPrevSpeaker, Ignored);
                                SessionHelper.UpdateSessionStatus(Session_ID, (int)Model.SessionStatus.Completed);
                            }
                            else if (currContentItem.StatusID == (int)Model.SessionContentItemStatus.Rejected)
                                    SessionContentItemFacade.UpdateSessionContentItem(currContentItem.ID, Text, SpeakerID, AgendaItemID, AgendaSubItemID, SpeakerJob, Comments, Footer, Model.SessionContentItemStatus.Fixed, SameAsPrevSpeaker, Ignored);
                            else if (CurrentUser.Role != UserRole.DataEntry)
                                    SessionContentItemHelper.UpdateSessionContentItem(currContentItem.ID, Text, SpeakerID, AgendaItemID, AgendaSubItemID, SpeakerJob, Comments, Footer, currContentItem.StatusID, true, CurrentUser.ID, SameAsPrevSpeaker, Ignored);
                            else
                                    SessionContentItemFacade.UpdateSessionContentItem(currContentItem.ID, Text, SpeakerID, AgendaItemID, AgendaSubItemID, SpeakerJob, Comments, Footer, (Model.SessionContentItemStatus)currContentItem.StatusID, SameAsPrevSpeaker, Ignored);
                        
                    }
                    else // create new
                    {
                        if (SameAsPrevSpeaker)
                        {
                            SessionContentItem prevContentItem = SessionContentItemFacade.GetSessionContentItemByIdAndFragmentOrder(Session_File_ID, FragOrder - 1);
                        }
                        int? agendaSubItemInt;
                        int temp;
                        if(int.TryParse(AgendaSubItemID.ToString(), out temp))
                            agendaSubItemInt = temp;
                        else
                            agendaSubItemInt = null; 

                        int status = (int)Model.SessionContentItemStatus.Approved;
                        if ((int)SpeakerID == (int)Model.AttendantType.UnKnown)
                            status = (int)Model.SessionContentItemStatus.Rejected;

                        SessionContentItemFacade.AddNewSessionContentItem(Session_File_ID, (long)Session_ID, Text, (int)SpeakerID, AgendaItemID, agendaSubItemInt, CurrentUser.ID, status, Comments, SpeakerJob, Footer, SameAsPrevSpeaker, FragOrder, (float)stime, (float)etime, (float)duration, Ignored);
                    }

                    if (session.SessionStatusID == (int)Model.SessionStatus.Approved)
                        SessionHelper.UpdateSessionStatus(Session_ID, (int)Model.SessionStatus.Completed);
                    if (session.SessionStatusID == (int)Model.SessionStatus.Completed)
                        SessionHelper.UpdateSessionStatus(Session_ID, (int)Model.SessionStatus.InProgress);

                    // update session file LastInsertedFragNumInXml && LastModefiedDate
                    if (editPageMode == EditorPageMode.Edit)
                        SessionFileFacade.UpdateSessionFileFragOrderAndModifiedDate(Session_File_ID, FragOrder, DateTime.Now);
                    else
                        SessionFileHelper.UpdateSessionFileModifiedDate(Session_File_ID, DateTime.Now);
                    //SessionFileHelper.UpdateSessionFileStatus(Session_File_ID, (int)Model.SessionFileStatus.Completed);

                    int editMode = 0;
                    if (EditPageMode != null && int.TryParse(EditPageMode, out editMode))
                    {
                        if (editMode == 2)
                        {
                            int nRejected = -1;
                            int nModefiedAfterApprove = -1;
                            int nFixed = -1;
                            Hashtable tblStats = EMadbatahFacade.GetSessionStatistics(CurrentUser, session.ID);
                            if (tblStats != null)
                            {
                                nRejected = int.Parse(tblStats[(int)Model.SessionContentItemStatus.Rejected].ToString());
                                nModefiedAfterApprove = int.Parse(tblStats[(int)Model.SessionContentItemStatus.ModefiedAfterApprove].ToString());
                                nFixed = int.Parse(tblStats[(int)Model.SessionContentItemStatus.Fixed].ToString());
                            }

                            var notCompletedSessionFiles = from sf in session.SessionFiles
                                                           where (sf.Status != (int)Model.SessionFileStatus.Completed)
                                                           select sf;

                            if (notCompletedSessionFiles.ToList<SessionFile>().Count == 0
                                && tblStats != null
                                && nRejected == 0
                                && nModefiedAfterApprove == 0
                                && nFixed == 0)
                                //&& !DAL.SessionContentItemHelper.DoesSessionContainsUndefinedAttendants(session.ID))
                            {
                                EMadbatahFacade.UpdateSessionStatus(session.ID, Model.SessionStatus.Completed);
                            }
                        }
                    }
                }
                else
                {
                    result["Message"] = "fail";
                }
                return result;
                //break;
            }
            else
                return null;
                //break;
        }
        private void loadSessionValues()
        {
            current_session_info = (Hashtable)_context.Session["current_session_info"];
            FragOrder = (int)current_session_info["FragOrderInXml"];
            stime = (double)current_session_info["PargraphStartTime"];
            etime = (double)current_session_info["PargraphEndTime"];
            duration = Math.Round(etime - stime, 2);
            Session_ID = (long)current_session_info["SessionID"];
            Session_File_ID = (long)current_session_info["SessionFileID"];
        }
        private SessionContentItem getReturnedContentItemObject(SessionContentItem nextContentItem)
        {
            SessionContentItem retItem = new SessionContentItem();
            retItem.ID = nextContentItem.ID;
            retItem.AgendaItemID = nextContentItem.AgendaItemID;
            retItem.AgendaSubItemID = nextContentItem.AgendaSubItemID;
            retItem.Text = nextContentItem.Text;
            retItem.AttendantID = nextContentItem.AttendantID;
            retItem.CommentOnAttendant = nextContentItem.CommentOnAttendant;
            retItem.CommentOnText = nextContentItem.CommentOnText;
            retItem.PageFooter = nextContentItem.PageFooter;
            retItem.FragOrderInXml = nextContentItem.FragOrderInXml;
            //retItem.Attendant = nextContentItem.Attendant;
            return retItem;
        }
        private SessionContentItem GetContentFromXML(string fName, int ItemFragOrderInXML)
        {
            TransFile tf = new TransFile();
            string xmlFilePath = _context.Server.MapPath("~") + fName.ToLower().Replace(".mp3", ".trans.xml");
            tf = TayaIT.Enterprise.EMadbatah.Vecsys.VecsysParser.ParseTransXml(xmlFilePath);
            List<Paragraph> p = VecsysParser.combineSegments(tf.SpeechSegmentList);
            // compose HTML spans from speech segments and set editor value
            string html = getHTML(p[ItemFragOrderInXML]);
            // compose SessionItem object
            SessionContentItem item = new SessionContentItem();
            item.Text = html;
            string ItemOrderFlag;
            if (ItemFragOrderInXML == 0)
            {
                //firstFrag = true;
                ItemOrderFlag = "first";
            }
            else if (ItemFragOrderInXML == p.Count - 1)
            {
                //lastFrag = false;
                ItemOrderFlag = "last";
            }
            else
                ItemOrderFlag = "null";

            if (current_session_info != null)
            {
                current_session_info["ItemOrderFlag"] = ItemOrderFlag;
                current_session_info["PargraphStartTime"] = p[ItemFragOrderInXML].speechSegmentsList[0].stime;
                current_session_info["PargraphEndTime"] = p[ItemFragOrderInXML].speechSegmentsList[p[ItemFragOrderInXML].speechSegmentsList.Count - 1].etime;
            }
            //current_session_info["PargraphStartTime"] = p[FragOrder].speechSegmentsList[0].stime;
            //current_session_info["PargraphEndTime"] = p[FragOrder].speechSegmentsList[p[FragOrder].speechSegmentsList.Count - 1].etime;

            return item;
        }
        private string checkItemPosition(string fName, int ItemFragOrderInXML)
        {
            TransFile tf = new TransFile();
            string xmlFilePath = _context.Server.MapPath("~") + fName.ToLower().Replace(".mp3", ".trans.xml");
            tf = TayaIT.Enterprise.EMadbatah.Vecsys.VecsysParser.ParseTransXml(xmlFilePath);
            List<Paragraph> p = VecsysParser.combineSegments(tf.SpeechSegmentList);
            string ItemOrderFlag;
            if (ItemFragOrderInXML == 0)
                ItemOrderFlag = "first";
            else if (ItemFragOrderInXML == p.Count - 1)
                ItemOrderFlag = "last";
            else
                ItemOrderFlag = "null";
            return ItemOrderFlag;
        }
        private string getHTML(Paragraph p)
        {
            string output = "";
            foreach (SpeechSegment segment in p.speechSegmentsList)
            {
                foreach (TayaIT.Enterprise.EMadbatah.Model.VecSys.Word word in segment.words)
                {
                    output += "<span class='segment' data-stime='" + word.stime.ToString() + "'>" + word.value + "</span> ";
                }
            }
            return output;
        }
        public string EditPageMode
        {
            get
            {
                return WebHelper.GetQSValue(Constants.QSKeyNames.EDIT_PAGE_MODE, _context);
            }
        }
        public string CorrectSessionContentItemID
        {
            get
            {
                return WebHelper.GetQSValue(Constants.QSKeyNames.SESSION_CONTENT_ITEM_ID, _context);
            }
        }

        public string CurrentFragOrder
        {
            get
            {
                return WebHelper.GetQSValue(Constants.QSKeyNames.FRAGORDER, _context);
            }
        }
        public string XmlFilePath
        {
            get
            {
                return WebHelper.GetQSValue(Constants.QSKeyNames.XMLPATH, _context);
            }
        }
        public string SplittedText
        {
            get
            {
                return WebHelper.GetQSValue(Constants.QSKeyNames.SPLITTEDTEXT, _context);
            }
        }


    }
}