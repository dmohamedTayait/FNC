<%@ Application Language="C#" %>

<%@ Import Namespace="System.IO" %>

<%@ Import Namespace="TayaIT.Enterprise.EMadbatah.Config" %>
<%@ Import Namespace="TayaIT.Enterprise.EMadbatah.Util" %>
<%@ Import Namespace="TayaIT.Enterprise.EMadbatah.BLL" %>
<%@ Import Namespace="TayaIT.Enterprise.EMadbatah.Model" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup
        // Code that runs on application startup
        //LogHelper.LogMessage("Application_Start" + Context.Request.Url.ToString(), "Global.asax", System.Diagnostics.TraceEventType.Information);
        AppConfig conf = AppConfig.GetInstance();

        loadAppCache();

        if (EMadbatahFacade.GetUsersCount() != -1 && EMadbatahFacade.GetUsersCount() == 0)
        {
            EMadbatahFacade.AddNewUser(new EMadbatahUser(UserRole.Admin, conf.MainAdmin.Name, conf.MainAdmin.DomainUserName, conf.MainAdmin.Email, true));
        }
        if (!(Context.Request.Url.Host == "localhost"))
        {
            System.Diagnostics.Process myproc = new System.Diagnostics.Process();
            foreach (System.Diagnostics.Process thisproc in System.Diagnostics.Process.GetProcesses())
            {
                if (thisproc.ProcessName.StartsWith("WINWORD"))
                {
                    thisproc.Kill();
                }
            }
        }

    }


    void loadAppCache()
    {
        /*if (!(Context.Request.Url.Host == "localhost"))
        {*/
            DirectoryInfo htmlDirInfo = new DirectoryInfo(Server.MapPath(@"~\" + Constants.WebSiteFolderNames.HTML_TEMPLATES));
            foreach (FileInfo fileInfo in htmlDirInfo.GetFiles())
            {
                Application[fileInfo.Name] = File.ReadAllText(fileInfo.FullName, Encoding.UTF8);
            }

            DirectoryInfo mailDirInfo = new DirectoryInfo(Server.MapPath(@"~\" + Constants.WebSiteFolderNames.MAIL_TEMPLATES));
            foreach (FileInfo fileInfo in mailDirInfo.GetFiles())
            {
                Application[fileInfo.Name] = File.ReadAllText(fileInfo.FullName, Encoding.UTF8);
            }
        //}
        
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    {
        //if (!Request.AppRelativeCurrentExecutionFilePath.EndsWith(Constants.PageNames.ERROR_PAGE))
        //{
        //    HttpContext Context = HttpContext.Current;
        //    System.Exception exception = Context.Server.GetLastError();
        //    if (exception != null && !exception.Message.Equals("File does not exist."))
        //    {
        //        LogHelper.LogException(exception, "Global.asax");
        //        Response.Redirect(Constants.PageNames.ERROR_PAGE + "?" + Constants.QSKeyNames.ERROR_TYPE + "=" + (int)ErrorType.Generic);
        //    }
        //    else
        //    {
        //        //LogHelper.LogMessage("ERROR happend in url : " + Context.Request.Url.ToString(), "Global.asax", System.Diagnostics.TraceEventType.Error);
        //        Server.ClearError();
        //    }
        //}
    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }

    protected void Application_BeginRequest(object sender, EventArgs e)
    {
        if (Request.HttpMethod == "GET")
        {
            if (Request.AppRelativeCurrentExecutionFilePath.EndsWith(".aspx"))
            {
                //Response.Filter = new Dropthings.Web.Util.ScriptDeferFilter(Response);
                //Response.Filter = new Dropthings.Web.Util.StaticContentFilter(Response, conf.ImgPrefix, conf.JsPrefix, conf.CssPrefix);
                //Response.Filter = new Dropthings.Web.Util.WhitespaceFilter(Response.Filter, new Regex(AppConstants.REGXP_WHITESPACE_FILTER));
            }
        }
    }
       
</script>
