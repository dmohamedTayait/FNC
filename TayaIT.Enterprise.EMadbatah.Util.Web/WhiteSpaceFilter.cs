
using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace TayaIT.Enterprise.EMadbatah.Util.Web
{
    public class WhitespaceFilter : HttpOutputFilter
    {
        //private static Regex reg = new Regex(@"(?<=[^])\t{2,}|(?<=[>])\s{2,}(?=[<])|(?<=[>])\s{2,11}(?=[<])|(?=[\n])\s{2,}");
        private static Regex _reg;

        /// <summary>
        /// Primary constructor.
        /// </summary>
        /// <param name="baseStream">The stream to wrap in gzip.  Must have CanWrite.</param>
        /// <param name="reg"></param>
        public WhitespaceFilter(Stream baseStream, Regex reg)
            : base(baseStream)
        {
            _reg = reg;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            byte[] data = new byte[count];
            Buffer.BlockCopy(buffer, offset, data, 0, count);
            string html = System.Text.Encoding.Default.GetString(buffer);

            html = _reg.Replace(html, string.Empty);
            //html = reg.Replace(html, string.Empty);//usama

            byte[] outdata = System.Text.Encoding.Default.GetBytes(html);
            BaseStream.Write(outdata, 0, outdata.GetLength(0));
        }

    }
}
