using System;
using System.IO;
using Android.Content;
using Android.Util;
using Android.App;

namespace Set
{
    public class Logger
    {
        public static void Info(string tag, string message)
        {
            Log("info", tag, message);
        }

        public static void Error(string tag, string message)
        {
            Log("error", tag, message);
        }

        public static void Error(string message)
        {
			Log("error", "OneSet", message);
        }

        private static string FileName 
        {
            get
            {
				return Path.Combine (PersonalFolder, "OneSet.log");
            }
        }

        private static String PersonalFolder
        {
            get
            {
                var filePath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).ToString();
				filePath = Path.Combine (filePath, "oneset/files/");
				if (!Directory.Exists (filePath))
				{
					Directory.CreateDirectory (filePath);
				}
                return filePath;
            }
        }

        public static void Log(string messageType, string tag, string message)
        {
            using (var sw = new StreamWriter(FileName, true))
            {
                var line = string.Format("{0}\t {1}\t {2}\t {3}\r", DateTime.Now.ToString("s"), messageType, tag, message);
                sw.Write(line);
            }
        }
    }
}

