using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TayaIT.Enterprise.EMadbatah.Util;

namespace TayaIT.Enterprise.EMadbatah.DAL
{
    public static class AttachmentHelper
    {
        public static long AddNewAttachement(long sessionID, string fileName, string filetype, byte[] fileContents)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    
                    var x = (from y in context.Attachements
                             where y.SessionID == sessionID
                             select (int?)y.Order).Max();
                    if (x == null)
                        x = 0;
                    Attachement item = new Attachement
                    {
                        Name = fileName,
                        SessionID = sessionID,
                        Order = (int)x + 1,
                        FileType = filetype,
                        FileContent = fileContents
                    };
                    context.Attachements.AddObject(item);
                    context.SaveChanges();
                    return item.ID;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.AttachmentHelper.AddNewAttachement(" + sessionID + "," + fileName + ")");
                return -1;
            }

        }

        public static int DeleteAttachementByID(int attachementID)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    Attachement attachment = context.Attachements.FirstOrDefault(c => c.ID == attachementID);
                    context.DeleteObject(attachment);
                    int res = context.SaveChanges();
                    return res;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.AttachmentHelper.DeleteAttachement(" + attachementID + ")");
                return -1;
            }
        }

        public static int DeleteAttachementByName(string attachment_name)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    Attachement attachment = context.Attachements.FirstOrDefault(c => c.Name == attachment_name);
                    context.DeleteObject(attachment);
                    int res = context.SaveChanges();
                    return res;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.AttachmentHelper.DeleteAttachementByName(" + attachment_name + ")");
                return -1;
            }
        }

        public static int UpdateAttachment(long attachment_id,
                                                  string attachment_name,
                                                  int order,
                                                  long session_id,
                                                  string file_type,
                                                  byte[] file_content)
        {
            try
            {

                Attachement updated_attachment = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    updated_attachment = context.Attachements.FirstOrDefault(c => c.ID == attachment_id || c.Name == attachment_name);
                    if (updated_attachment != null)
                    {
                        //update the attachment attributes
                        updated_attachment.Name = string.IsNullOrEmpty(attachment_name) ? updated_attachment.Name : attachment_name;
                        updated_attachment.Order = order == null ? updated_attachment.Order : order;
                        updated_attachment.SessionID = session_id == null ? updated_attachment.SessionID : session_id;
                        updated_attachment.FileType = string.IsNullOrEmpty(file_type) ? updated_attachment.FileType : file_type;
                        updated_attachment.FileContent = file_content == null ? updated_attachment.FileContent : file_content;
                    }
                    int res = context.SaveChanges();
                    return res;
                }
                
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.AttachmentHelper.UpdateAttachment(" + attachment_id + "," + attachment_name + ")");
                return -1;
            }
        }

        public static int UpdateAttachmentOrder(long attachment_id, int new_order)
        {
            try
            {
                Attachement updated_attachment = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    updated_attachment = context.Attachements.FirstOrDefault(c => c.ID == attachment_id);
                    if (updated_attachment != null)
                    {
                        //update the attachment attributes
                        updated_attachment.Order = new_order == null ? updated_attachment.Order : new_order;
                    }
                    int res = context.SaveChanges();
                    return res;
                }
                //return updated_attachment;
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.AttachmentHelper.UpdateAttachmentOrder(" + attachment_id + "," + new_order + ")");
                return -1;
            }
        }



        public static List<Attachement> GetSessionAttachments(long sessionID)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    return (from obj in context.Attachements
                            where obj.SessionID == sessionID
                            select obj).ToList();
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.AttachmentHelper.GetSessionAttachments(" + sessionID + ")");
                return null;
            }
        }
        public static Attachement GetAttachementByID(int attachementID)
        {
            try
            {
                Attachement attachement = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    attachement = context.Attachements.FirstOrDefault(c => c.ID == attachementID);
                }
                return attachement;
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.AttachmentHelper.GetAttachementByID(" + attachementID + ")");
                return null;
            }
        }
        public static Attachement GetAttachementByOrder(int attachementOrder)
        {
            try
            {
                Attachement attachement = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    attachement = context.Attachements.FirstOrDefault(c => c.Order == attachementOrder);
                }
                return attachement;
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.AttachmentHelper.GetAttachementByOrder(" + attachementOrder + ")");
                return null;
            }
        }

        /*public static int UpdateAttachementOrder(long attachmentID, int newOrder)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    Attachement updated_attachment = context.Attachements.FirstOrDefault(c => c.ID == attachmentID);
                    if (updated_attachment != null)
                    {
                        updated_attachment.Order = newOrder;
                        int res = context.SaveChanges();
                        return res;
                    }
                    else
                        return -1;
                }
              
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.AttachmentHelper.ChangeAttachementOrder(" + attachmentID + "," + newOrder + ")");
                return -1;
            }
        }*/

        public static bool UpdateAttachementFileOrder(long attachFileID1, int newOrder1, long attachFileID2, int newOrder2)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    Attachement updatedAttachFile1 = context.Attachements.FirstOrDefault(c => c.ID == attachFileID1);
                    Attachement updatedAttachFile2 = context.Attachements.FirstOrDefault(c => c.ID == attachFileID2);

                    if (updatedAttachFile1 != null && updatedAttachFile2 != null)
                    {
                        updatedAttachFile1.Order = newOrder1;
                        updatedAttachFile2.Order = newOrder2;
                        int res = context.SaveChanges();
                        return true;
                    }
                    else
                        return false;

                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.AttachmentHelper.UpdateAttachementFileOrder(" + attachFileID1 + "," + newOrder1 + "," + attachFileID2 + "," + newOrder2 + ")");
                return false; ;
            }
        }

    }
}
