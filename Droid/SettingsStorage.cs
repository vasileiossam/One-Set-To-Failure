using System;
using Set.Models;
using System.Threading.Tasks;
using Set.Droid;
using Xamarin.Forms;
using System.IO;
using Newtonsoft.Json;

[assembly: Dependency (typeof (SettingsStorage))]

namespace Set.Droid
{
	public class SettingsStorage : ISettingsStorage
	{
		public const string FileName = "settings.json";

		public SettingsStorage ()
		{

		}

		public static string GetPathName()
		{
			var folder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var pathName = Path.Combine(folder, FileName);
			return pathName;
		}

		public void Save(Settings settings)
		{
			var pathName = GetPathName ();

			using (StreamWriter sw = File.CreateText (pathName)) {
				var json = JsonConvert.SerializeObject (settings);
				sw.Write (json);
			}
		}

		public Settings Load()
		{
			try
			{
				Settings settings;
				var pathName = GetPathName ();

				if (File.Exists (pathName)) {
					using (StreamReader sr = File.OpenText (pathName)) {
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
			catch(Exception ex)
			{
				throw ex; 
			}
		}
	}
}

