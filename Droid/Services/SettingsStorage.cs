using System;
using System.IO;
using Newtonsoft.Json;
using OneSet.Abstract;
using OneSet.Droid.Services;
using OneSet.Models;
using Xamarin.Forms;

[assembly: Dependency (typeof (SettingsStorage))]

namespace OneSet.Droid.Services
{
	public class SettingsStorage : ISettingsStorage
	{
		public const string FileName = "settings.json";

	    public static string GetPathName()
		{
			var folder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var pathName = Path.Combine(folder, FileName);
			return pathName;
		}

		public void Save(Settings settings)
		{
			var pathName = GetPathName ();

			using (var sw = File.CreateText (pathName)) {
				var json = JsonConvert.SerializeObject (settings);
				sw.Write (json);
			}
		}

		public Settings Load()
		{
		    Settings settings;
		    var pathName = GetPathName ();

		    if (File.Exists (pathName)) {
		        using (var sr = File.OpenText (pathName)) {
		            var json = sr.ReadLine ();
		            settings = JsonConvert.DeserializeObject<Settings> (json);

		        }
		    } 
		    else 
		    {
		        settings = new Settings ();
		    }

		    return settings;
		}
	}
}

