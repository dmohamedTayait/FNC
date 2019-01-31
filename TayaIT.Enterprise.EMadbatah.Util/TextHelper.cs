using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TayaIT.Enterprise.EMadbatah.Util
{
    public class TextHelper
    {
        public static string StripHTML(string htmlString)
        {
            string pattern = @"<(.|\n)*?>";


            htmlString = htmlString.Replace("&amp;", "&");
            htmlString = htmlString.Replace("&gt;", ">");
            htmlString = htmlString.Replace("&lt;", "<");

            string cleaned = Regex.Replace(htmlString, "<script.*?</script>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            cleaned = Regex.Replace(cleaned, "<!--.*?-->", String.Empty, RegexOptions.Singleline);


            cleaned = Regex.Replace(cleaned, pattern, string.Empty);


            return cleaned;
        }

        public static string Truncate(string source, int length, string trailingChars)
        {
            if (!string.IsNullOrEmpty(source))
            {
                if (source.Length > length)
                {
                    source = source.Substring(0, length);
                    if (!string.IsNullOrEmpty(trailingChars.Trim()))
                        source += trailingChars;
                }

                return source;
            }
            else
                return source;
        }

        //public static string WrapTextInHTML(string source, int length, string trailingChars)
        //{
        //    if (!string.IsNullOrEmpty(source))
        //    {
        //        if (source.Length > length)
        //        {
        //            source = source.Substring(0, length);
        //            if (!string.IsNullOrEmpty(trailingChars.Trim()))
        //                source += trailingChars;
        //        }

        //        return source;
        //    }
        //    else
        //        return source;
        //}

        private static Random random = new Random((int)DateTime.Now.Ticks);//thanks to McAden
        public static string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }
    }
}
