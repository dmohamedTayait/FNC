using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;

namespace TayaIT.Enterprise.EMadbatah.Util.Web
{
    public static class WebHelper
    {

        public static string GetQSValue(string paramName, HttpContext context)
        {
            string val = context.Request.QueryString[paramName];
            val = val != null ? val : context.Request[paramName];
            if (!string.IsNullOrEmpty(val) && val.Trim() != string.Empty)
            {
                return val;
            }
            else
                return null;
        }

        public static Dictionary<string, string> ExtractPostParameters(HttpRequest request, bool isCaseSensitive)
        {
            var postParameters = new Dictionary<string, string>();
            if (request.Params.Count > 0)
            {
                string[] paramItems;
                using (var sr = new StreamReader(request.InputStream, Encoding.UTF8))
                {
                    paramItems = sr.ReadToEnd().Split("&".ToCharArray());
                }

                var parameters = (from d in
                                      (from d in paramItems
                                       select d.Split("=".ToCharArray()))
                                  where d.Length == 2
                                  select new KeyValuePair<string, string>
                                      (d[0].Trim(), d[1].Trim()));

                foreach (var pair in parameters)
                {
                    var key = isCaseSensitive ? pair.Key : pair.Key.ToLower();

                    if (postParameters.ContainsKey(key))
                    {
                        postParameters[key] = pair.Value;
                    }
                    else
                    {
                        postParameters.Add(pair.Key, pair.Value);
                    }
                }
            }
            return postParameters;
        }
    }
}
