using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for BaseUserControl
/// </summary>
public class BaseUserControl : System.Web.UI.UserControl
{
    public string ChartType { get; set; }
    public string[] XVals { get; set; }
    public int[] YVals { get; set; }
}