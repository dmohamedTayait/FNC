using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using TayaIT.Enterprise.EMadbatah.DAL;
using TayaIT.Enterprise.EMadbatah.Util.Web;
using TayaIT.Enterprise.EMadbatah.Config;
using TayaIT.Enterprise.EMadbatah.Util;
using TayaIT.Enterprise.EMadbatah.Model;
using TayaIT.Enterprise.EMadbatah.BLL;

namespace TayaIT.Enterprise.EMadbatah.Web.Framework
{
    class FileHandler: BaseHandler
    {
        protected override void HandleRequest()
        {

            WebFunctions.FileFunctions function;

            if (AjaxFunctionName != null && Enum.TryParse<WebFunctions.FileFunctions>(AjaxFunctionName, true, out function))
            {

                long sessionID = 0;
                string jsonStringOut = null;
                switch (function)
                {
                    case WebFunctions.FileFunctions.GetFilesStatus:

                        if (SessionID != null && long.TryParse(SessionID, out sessionID) != null)
                        {
                            try
                            {
                                jsonStringOut = SerializeObjectInJSON(EMadbatahFacade.GetSessionMadbatahFilesStatus(sessionID).ToString());
                            }
                            catch (Exception)
                            {
                                jsonStringOut = SerializeObjectInJSON("ERROR");
                            }
                            
                        }

                        
                        break;

                    case WebFunctions.FileFunctions.Upload:
                        try
                        {
                            if (SessionID != null && long.TryParse(SessionID, out sessionID) && AttachmentFileName != null)
                            {
                                // string strFileName = Path.GetFileName(_context.Request.Files[0].FileName);
                                //string strExtension = Path.GetExtension(_context.Request.Files[0].FileName).ToLower();
                                //string strSaveLocation = _context.Server.MapPath("Upload") + "\\" + strFileName;
                                // _context.Request.Files[0].SaveAs(strSaveLocation);
                                long newId = -1;
                                if (_context.Request.ContentLength > 0)
                                {
    

                                    /*byte[] buffer = new byte[_context.Request.ContentLength];
                                    using (BinaryReader br = new BinaryReader(_context.Request.InputStream))
                                        br.Read(buffer, 0, buffer.Length);
                                    */
                                    byte[] fileData = null;
                                    /*using (var reader = new BinaryReader(_context.Request.InputStream))
                                    {
                                        fileData = reader.ReadBytes((int)_context.Request.InputStream.Length);
                                    }
                                   */
                                    
                                    string folderPath = _context.Server.MapPath("~") + @"\Files\";
                                    if (!Directory.Exists(folderPath))
                                        Directory.CreateDirectory(folderPath);

                                    string filePath = folderPath + AttachmentFileName;
                                    if (_context.Request.Files.Count != 0)
                                    {
                                        //works on IE
                                        _context.Request.Files[0].SaveAs(filePath);
                                        fileData = File.ReadAllBytes(filePath);
                                    }
                                    else
                                    {
 // work on other browsers
                                        byte[] buffer = new byte[_context.Request.ContentLength];
                                        using (BinaryReader br = new BinaryReader(_context.Request.InputStream))
                                            br.Read(buffer, 0, buffer.Length);
                                    fileData = buffer;
                                    
                                    }
                                   

                                    if (fileData.Length > 0)
                                    {
                                        newId = AttachmentHelper.AddNewAttachement(sessionID, AttachmentFileName, Path.GetExtension(AttachmentFileName).ToLower().Replace(".", ""), fileData);
                                        if (newId > -1)
                                        {
                                            EMadbatahFacade.UpdateSessionStatus(sessionID, Model.SessionStatus.InProgress);
                                        }
                                    }

                                    if (File.Exists(filePath))
                                        File.Delete(filePath);

                                }

                                var response = new { success = true, fileName = TextHelper.Truncate(AttachmentFileName, 17, "..."), id = newId };
                                jsonStringOut = SerializeObjectInJSON(response);
                                //if (jsonStringOut != null)
                                //{
                                //    _context.Response.AddHeader("Encoding", "UTF-8");
                                //    _context.Response.ContentType = "text/plain";
                                //    _context.Response.Write(jsonStringOut);
                                //}
                            }

                        }
                        catch
                        {
                            jsonStringOut = SerializeObjectInJSON(new { sucess = false });
                        }
                        break;
                    case WebFunctions.FileFunctions.Download:
                        //expected fileID , FileType
                        if (SessionID != null && long.TryParse(SessionID, out sessionID) && 
                            FileType != null && Enum.IsDefined(typeof(FileExtensionType), FileType) &&
                            FileVersion != null && Enum.IsDefined(typeof(FileVersion), FileVersion))
                        {
                            FileExtensionType fileType = (FileExtensionType)Enum.Parse(typeof(FileExtensionType), FileType);
                            FileVersion fileVersion = (FileVersion)Enum.Parse(typeof(FileVersion), FileVersion);

                            byte[] fileData = null;
                            if(fileVersion == Model.FileVersion.draft)
                               fileData = (fileType == FileExtensionType.pdf) ? SessionHelper.GetSessionPDFFile(sessionID) : SessionHelper.GetSessionWordFile(sessionID);
                            else
                                fileData = (fileType == FileExtensionType.pdf) ? SessionHelper.GetSessionPDFFinalFile(sessionID) : SessionHelper.GetSessionWordFinalFile(sessionID);

                            SessionDetails sd = EMadbatahFacade.GetSessionBySessionID(sessionID);
                            string fileName = EMadbatahFacade.GetSessionName(sd.Season, sd.Stage, sd.Serial) + "-"+ sd.Date.Day + "-" + sd.Date.Month + "-" + sd.Date.Year+"." +FileType;
                            if (fileData != null && fileData.Length > 0)
                            {
                                SendFile(fileData, fileName, fileType);
                            }

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


        

        public void SendFile(byte[] fileData,string fileName, FileExtensionType fileType)
        {

            _context.Response.ClearHeaders();
           // _context.Response.AppendHeader("Etag", ImgHash);
            _context.Response.ContentType = (fileType == FileExtensionType.pdf ? "application/pdf" : "application/ms-word");
            _context.Response.AppendHeader("Content-Length", fileData.Length.ToString());
            _context.Response.AppendHeader("content-disposition", "attachment; filename=\"" + fileName + "\"");
            _context.Response.AppendHeader("Content-Transfer-Encoding", "binary");
            _context.Response.CacheControl = "public";
            _context.Response.BinaryWrite(fileData);
        }


        public string AttachmentFileName
        {
            get
            {
                string toRet = WebHelper.GetQSValue(Constants.QSKeyNames.ATTACHMENT_FILE_NAME, _context); //Firefox
                if (string.IsNullOrEmpty(toRet))
                {
                    toRet = new FileInfo(_context.Request.Files[0].FileName).Name; //IE
                }
                return toRet;
            }
        }


        public string FileType
        {
            get
            {
                return WebHelper.GetQSValue(Constants.QSKeyNames.FILE_TYPE, _context);
            }
        }

        public string FileVersion
        {
            get
            {
                return WebHelper.GetQSValue(Constants.QSKeyNames.FILE_VERSION, _context);
            }
        }
        //qqfile
    }
}
