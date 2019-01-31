using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TayaIT.Enterprise.EMadbatah.Util;

namespace TayaIT.Enterprise.EMadbatah.DAL
{
    public class AttendantHelper
    {
        public static bool AddNewAttendant(string attendant_name, string job_title, int eparliamentID, int typeID, int stateID, long sessionID, string fName, string sName, string tName)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    Attendant attendant = new Attendant
                    {
                        Name = attendant_name,
                        JobTitle = job_title,
                        EparlimentID = eparliamentID,
                        Type = typeID,
                        State = stateID,
                        FirstName = fName,
                        SecondName = sName,
                        TribeName = tName
                    };
                    //context.Attendants.AddObject(attendant);

                    Session session = context.Sessions.FirstOrDefault(c => c.ID == sessionID);
                    session.Attendants.Add(attendant);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.AttendantHelper.AddNewAttendant(" + attendant_name + "," + eparliamentID + ")");
                return false;
            }
        }


        public static int DeleteAttendantByID(long attendant_id)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    Attendant attendant = context.Attendants.FirstOrDefault(c => c.ID == attendant_id);
                    context.DeleteObject(attendant);
                    int res = context.SaveChanges();
                    return res;
                }
                
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.AttendantHelper.DeleteAttendantByID(" + attendant_id + ")");
                return -1;
            }
        }

        public static int DeleteAttendantByEPID(long attendantEPID)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    Attendant attendant = context.Attendants.FirstOrDefault(c => c.EparlimentID == attendantEPID);
                    context.DeleteObject(attendant);
                    int res = context.SaveChanges();
                    return res;
                }

            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.AttendantHelper.DeleteAttendantByEPID(" + attendantEPID + ")");
                return -1;
            }
        }

        public static int DeleteAttendantByName(string attendant_name)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    Attendant attendant = context.Attendants.FirstOrDefault(c => c.Name == attendant_name);
                    context.DeleteObject(attendant);
                    int res = context.SaveChanges();
                    return res;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.AttendantHelper.DeleteAttendantByName(" + attendant_name + ")");
                return -1;
            }
        }

        public static int UpdateAttendant(long attendant_id, string attendant_name, string job_title, int typeID, int stateID)
        {
            try
            {
                Attendant updated_attendant = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    updated_attendant = context.Attendants.FirstOrDefault(c => c.ID == attendant_id || c.Name == attendant_name);
                    if (updated_attendant != null)
                    {
                        //update the Attendant attributes
                        updated_attendant.Name = attendant_name == null ? updated_attendant.Name : attendant_name;
                        updated_attendant.JobTitle = job_title == null ? updated_attendant.JobTitle : job_title;
                        updated_attendant.Type = typeID == null ? updated_attendant.Type : typeID;
                        updated_attendant.State = stateID == null ? updated_attendant.State : stateID;
                    }
                    int res = context.SaveChanges();
                    return res;
                }
                
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.AttendantHelper.UpdateAttendant(" + attendant_name + "," + job_title + ")");
                return -1;
            }
        }

        public static Attendant GetAttendantById(long attendant_id)
        {
            try
            {
                Attendant attendant = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    attendant = context.Attendants.FirstOrDefault(c => c.ID == attendant_id);
                }
                return attendant;
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.AttendantHelper.GetAttendantById(" + attendant_id + ")");
                return null;
            }
        }

        public static long GetUnknownAttendantId(long sessionID)
        {
            try
            {
                
                Attendant attendant = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    attendant = context.Sessions.FirstOrDefault(c => c.ID == sessionID).Attendants.FirstOrDefault(c => c.Name == "غير معرف");
                    if (attendant != null)
                        return attendant.ID;
                    else
                    {
                        //Attendant unknownAttendant = new Attendant();
                        //unknownAttendant.State = (int)Model.AttendantState.Attended;
                        //unknownAttendant.Type = (int)Model.AttendantType.UnKnown;
                        //unknownAttendant.EparlimentID = -1;
                        //unknownAttendant.JobTitle = "";
                        //unknownAttendant.Name = "غير معرف";
                        
                        
                        //AddNewAttendant("غير معرف", "", -1, 7, 1, sessionID);
                        attendant = new Attendant
                        {
                            Name = "غير معرف",
                            JobTitle = "",
                            EparlimentID = -1,
                            Type = 7,
                            State = 1
                        };
                        //context.Attendants.AddObject(attendant);

                        Session session = context.Sessions.FirstOrDefault(c => c.ID == sessionID);
                        session.Attendants.Add(attendant);
                        context.SaveChanges();



                        //attendant = context.Sessions.FirstOrDefault(c => c.ID == sessionID).Attendants.FirstOrDefault(c => c.Name == "غير معرف");
                        return attendant.ID;

                    }
                }
                return -1;
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.AttendantHelper.GetUnknownAttendantId(" + sessionID + ")");
                return -1;
            }
        }

        public static List<Attendant> GetAttendantsBySessionId(long sessionid)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    Session s = context.Sessions.FirstOrDefault(c => c.ID == sessionid);
                    if(s != null)
                        return s.Attendants.ToList<Attendant>();

                }
                return null;
                
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.AttendantHelper.GetAttendantsBySessionId(" + sessionid + ")");
                return null;
            }
        }


        public static List<Attendant> GetAttendants(long sessionID, long agendaitemID, long subagendaid, bool isAllSpeakers)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    List<Attendant> attendants = new List<Attendant>();
                    List<SessionContentItem> scis = null;

                    if (!isAllSpeakers && (agendaitemID < 0 || subagendaid < 0))
                    {
                        attendants = (from att in context.Sessions.FirstOrDefault(s => s.ID == sessionID).Attendants
                                         where att.State != null && att.State == 1
                                         select att).ToList<Attendant>();
                        return attendants;
                    }

                    if(subagendaid > 0)
                        scis = context.AgendaSubItems.FirstOrDefault(c => c.ID == subagendaid).SessionContentItems.ToList<SessionContentItem>();
                    else if(agendaitemID > 0)
                        scis = context.AgendaItems.FirstOrDefault(c => c.ID == agendaitemID).SessionContentItems.ToList<SessionContentItem>();

                    if (scis != null)
                    {
                        foreach (SessionContentItem sci in scis)
                        {
                            if (!attendants.Contains(sci.Attendant))
                            {
                                if (isAllSpeakers)
                                    attendants.Add(sci.Attendant);
                                else if (sci.Attendant.State != null && sci.Attendant.State.Value == 1)
                                    attendants.Add(sci.Attendant);
                            }
                        }
                        return attendants;
                    }
                        

                }
                return null;

            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.AttendantHelper.GetAttendantsByAgendaItemID(" + agendaitemID + ")");
                return null;
            }
        }


        public static Attendant GetAttendantByName(string attendant_name)
        {
            try
            {
                Attendant attendant = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    attendant = context.Attendants.FirstOrDefault(c => c.Name == attendant_name);
                }
                return attendant;
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.AttendantHelper.GetAttendantByName(" + attendant_name + ")");
                return null;
            }
        }


    }
}
