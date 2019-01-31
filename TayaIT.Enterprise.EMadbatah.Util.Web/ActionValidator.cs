// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

/// <summary>
/// Summary description for ActionValidator
/// </summary>
namespace TayaIT.Enterprise.EMadbatah.Util.Web
{
    public static class ActionValidator
    {
        private const int DURATION = 10; // 10 min period

        /*
         * Type of actions and their maximum value per period
         * 
         */
        public enum ActionType
        {
            None = 0,
            Revisit = 10000,  // Welcome to revisit as many times as user likes
            JSReq = 50000,    // Not must of a problem for us
        }

        private class HitInfo
        {
            public int Hits;
            private DateTime _ExpiresAt = DateTime.Now.AddMinutes(DURATION);
            public DateTime ExpiresAt { get { return _ExpiresAt; } set { _ExpiresAt = value; } }
        }

        public static List<string> AllowedIPList
        {
            get
            {
                List<string> allowedIPList = new List<string>();
                if (HttpContext.Current.Cache["AllowedIPList"] == null)
                {
                    allowedIPList.AddRange(ConfigManager.Get<string>("AllowedIPList").Split(",".ToCharArray()));
                    HttpContext.Current.Cache["AllowedIPList"] = allowedIPList;
                }
                else
                    allowedIPList = (List<string>)HttpContext.Current.Cache["AllowedIPList"];

                return allowedIPList;

            }
        }

        public static List<string> ForbiddenIPList
        {
            get
            {
                List<string> forbiddenIPList = new List<string>();
                if (HttpContext.Current.Cache["ForbiddenIPList"] == null)
                {
                    forbiddenIPList.AddRange(ConfigManager.Get<string>("ForbiddenIPList").Split(",".ToCharArray()));
                    HttpContext.Current.Cache["ForbiddenIPList"] = forbiddenIPList;
                }
                else
                    forbiddenIPList = (List<string>)HttpContext.Current.Cache["ForbiddenIPList"];

                return forbiddenIPList;

            }
        }

        public static bool IsValid(ActionType actionType)
        {
            HttpContext context = HttpContext.Current;

            if (ForbiddenIPList.Contains(context.Request.UserHostAddress))
            {
                return false;
            }

            if (AllowedIPList.Contains(context.Request.UserHostAddress))
            {
                return true;
            }

            if (context.Request.Browser.Crawler) return false;

            string key = actionType.ToString() + context.Request.UserHostAddress;

            var hit = (HitInfo)(context.Cache[key] ?? new HitInfo());

            if (hit.Hits > (int)actionType) return false;
            else hit.Hits++;

            if (hit.Hits == 1)
                context.Cache.Add(key, hit, null, DateTime.Now.AddMinutes(DURATION),
                    System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);

            return true;
        }
    }
}