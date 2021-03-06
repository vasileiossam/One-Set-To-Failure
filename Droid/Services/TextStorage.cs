﻿using System.IO;
using System.Text;
using OneSet.Abstract;
using OneSet.Droid.Services;
using Xamarin.Forms;

[assembly: Dependency (typeof (TextStorage))]

namespace OneSet.Droid.Services
{
	public class TextStorage : ITextStorage
    {
        private static string DownloadsFolder
        {
            get
            {
                var filePath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).ToString();
				filePath = Path.Combine (filePath, "OneSet/");
				if (!Directory.Exists (filePath))
				{
					Directory.CreateDirectory (filePath);
				}
                return filePath;
            }
        }

		public string Save(StringBuilder text, string fileName)
        {
			var pathName = Path.Combine (DownloadsFolder, fileName);
			File.WriteAllText (pathName, text.ToString ());
			return fileName;
        }
    }
}

