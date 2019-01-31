using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TayaIT.Enterprise.EMadbatah.Util
{
    public class Localization
    {
        public static string GetCurrentThreadLang()
        {
            string cultName = Thread.CurrentThread.CurrentUICulture.Name;
            if (cultName.ToLower().IndexOf("ar") > -1)
                return "ar";
            else
                return "en";
        }

        public static void SetCurrentThreadLang(string newLang)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(newLang);
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(newLang);
        }
    }
}
