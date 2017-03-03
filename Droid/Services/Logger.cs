using System;
using System.IO;

namespace OneSet.Droid.Services
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

        private static string FileName => Path.Combine (PersonalFolder, "OneSet.log");

        private static string PersonalFolder
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
                var line = $"{DateTime.Now:s}\t {messageType}\t {tag}\t {message}\r";
                sw.Write(line);
            }
        }
    }
}

