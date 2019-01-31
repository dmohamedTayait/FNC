using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TayaIT.Enterprise.EMadbatah.Config
{
    public class Preferences
    {
        //private Language _uiLang, _searchLang;
        //ArabicOption _defaultArabicOption;
        //private bool _resultsInNewWindow, _expandRefineColumn;
        //private int _view, _resultsPerPage;
        //private ExaleadView _exaleadView;
        //public Preferences()
        //{
        //    _uiLang = getLanguageFromString(ConfigManager.Get<string>("UILang"));
        //    _searchLang = getLanguageFromString(ConfigManager.Get<string>("SearchLang"));
        //    _resultsInNewWindow = ConfigManager.Get<bool>("ResultsInNewWindow");
        //    _expandRefineColumn = ConfigManager.Get<bool>("ExpandRefineColumn");
        //    _view = ConfigManager.Get<int>("View");
        //    _resultsPerPage = ConfigManager.Get<int>("DEFAULT_RESULTS_PER_PAGE");
        //    _defaultArabicOption = (ArabicOption)ConfigManager.Get<int>("DefaultArabicOption");
        //    _exaleadView = (ExaleadView)ConfigManager.Get<int>("DEFAULT_EXALEAD_VIEW");

        //    FillPrefValuesFromCookie();
        //}

        //private void FillPrefValuesFromCookie()
        //{
        //    string cookieName = Constants.CookieNames.Preferences;


        //    HttpRequest req = HttpContext.Current.Request;
        //    HttpResponse res = HttpContext.Current.Response;

        //    if (req.Cookies[cookieName] != null)
        //    {
        //        HttpCookie cookie = req.Cookies[cookieName];
        //        string[] arrCookieVals = cookie.Value.Split("@".ToCharArray());

        //        foreach (string val in arrCookieVals)
        //        {
        //            if (val.StartsWith("userInterfaceLanguage"))
        //                _uiLang = getLanguageFromString(val.Replace("userInterfaceLanguage", ""));
        //            else if (val.StartsWith("indexSearchLanguage"))
        //                _searchLang = getLanguageFromString(val.Replace("indexSearchLanguage", ""));
        //            else if (val.StartsWith("openResultsInNewWindow"))
        //                _resultsInNewWindow = bool.Parse(val.Replace("openResultsInNewWindow", ""));
        //            else if (val.StartsWith("openRefineYourSearch"))
        //                _expandRefineColumn = bool.Parse(val.Replace("openRefineYourSearch", ""));
        //            else if (val.StartsWith("defaultResultsView"))
        //                _view = int.Parse(val.Replace("defaultResultsView", ""));
        //            else if (val.StartsWith("resultsPerPage"))
        //                _resultsPerPage = int.Parse(val.Replace("resultsPerPage", ""));
        //            else if (val.StartsWith("defaultArabicOption"))
        //                _defaultArabicOption = (ArabicOption)int.Parse(val.Replace("defaultArabicOption", "").Replace(";", ""));
        //            else if (val.StartsWith("defaultExaleadView"))
        //                _exaleadView = (ExaleadView)int.Parse(val.Replace("defaultExaleadView", "").Replace(";", ""));
        //        }
        //    }
        //}

        //public string GetCookieVal()
        //{
        //    return new StringBuilder("@userInterfaceLanguage").Append(_uiLang)
        //    .Append("@indexSearchLanguage").Append(_searchLang)
        //    .Append("@openResultsInNewWindow").Append(_resultsInNewWindow.ToString().ToLower())
        //    .Append("@openRefineYourSearch").Append(_expandRefineColumn.ToString().ToLower())
        //    .Append("@resultsPerPage").Append(_resultsPerPage)
        //    .Append("@defaultArabicOption").Append((int)_defaultArabicOption)
        //    .Append("@defaultExaleadView").Append((int)_exaleadView)
        //    .Append("@defaultResultsView").Append(_view).ToString();
        //}

        public void SavePreferencesToCookie()
        {
            //HttpRequest req = HttpContext.Current.Request;
            //HttpResponse res = HttpContext.Current.Response;

            //string cookieName = Constants.CookieNames.Preferences;

            //HttpCookie newCookie = new HttpCookie(cookieName, this.GetCookieVal());
            ////cookie name cookie_preferences
            //if (req.Cookies[cookieName] != null)
            //    res.Cookies.Set(newCookie);//set the existing cookie to the response with the new value
            //else
            //    res.Cookies.Add(newCookie);//add new cookie to the response
        }

        //private Language getLanguageFromString(string langStr)
        //{
        //    return (Language)Enum.Parse(typeof(Language), langStr);
        //}

        //public Language UiLang { get { return _uiLang; } }
        //public Language SearchLang { get { return _searchLang; } }
        //public bool ResultsInNewWindow { get { return _resultsInNewWindow; } }
        //public bool ExpandRefineColumn { get { return _expandRefineColumn; } set { _expandRefineColumn = value; } }
        //public int View { get { return _view; } set { _view = value; } }
        //public int ResultsPerPage { get { return _resultsPerPage; } }
        //public ArabicOption DefaultArabicOption { get { return _defaultArabicOption; } set { _defaultArabicOption = value; } }
        //public ExaleadView ExaleadView { get { return _exaleadView; } set { _exaleadView = value; } }
    }
}
