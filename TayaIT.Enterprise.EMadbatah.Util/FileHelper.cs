using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TayaIT.Enterprise.EMadbatah.Util
{
    public static class FileHelper
    {
        public static long GetMP3FileDurationAsLong(string mp3FilePath)
        {
            TagLib.File f = TagLib.File.Create(mp3FilePath);
            return f.Properties.Duration.Ticks;
        }

        public static TimeSpan GetMP3FileDurationAsTimeSpan(string mp3FilePath)
        {
            TagLib.File f = TagLib.File.Create(mp3FilePath);
            return f.Properties.Duration;
        }
    }
}
