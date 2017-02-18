using System;
using OneSet.Models;
using System.Threading.Tasks;
using OneSet.Droid;
using Xamarin.Forms;
using System.IO;
using System.Text;

[assembly: Dependency (typeof (Exporter))]

namespace OneSet.Droid
{
	public class Exporter : IExporter
    {
        private String PersonalFolder
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

		public string ExportToCsv(StringBuilder csv)
        {
			var fileName = Path.Combine (PersonalFolder, "OneSet.csv");
			File.WriteAllText (fileName, csv.ToString ());
			return fileName;
        }
    }
}

