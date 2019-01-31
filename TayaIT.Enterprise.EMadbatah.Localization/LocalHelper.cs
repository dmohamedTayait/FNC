using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TayaIT.Enterprise.EMadbatah.Localization
{
    public class LocalHelper
    {
        public static string GetLocalizedString(string locStringKeyName)
        {
            return EmadbatahRes.ResourceManager.GetString(locStringKeyName);// return rmEn.GetString(locStringKeyName);
        }
    }
}
