using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace TayaIT.Enterprise.EMadbatah.Util
{

    /// <summary>
    /// This is a helper to get value out of web.config AppSettings section easier
    /// </summary>
    public static class ConfigManager
    {
        private static object locker = new object();

        private static AppSettingsReader _reader;


        static ConfigManager()
        {
            lock (locker)
            {
                _reader = new AppSettingsReader();
            }
        }

        /// <summary>
        /// Gets the value of the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            Check.AssertValidString(key);
            object data = _reader.GetValue(key, typeof(T));
            T value = (T)Convert.ChangeType(data, typeof(T));
            return value;
        }

        public static string GetConnectionString(string key)
        {
            return ConfigurationManager.ConnectionStrings[key].ConnectionString;
        }
    }
}
