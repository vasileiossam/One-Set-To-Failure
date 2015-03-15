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
		const string FileName = "settings.json";

		public SettingsStorage ()
		{

		}


		public void Save(Settings settings)
		{
			var folder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var path = Path.Combine(folder, FileName);

			using (StreamWriter sw = File.CreateText (path)) {
				var json = JsonConvert.SerializeObject (settings);
				sw.Write (json);
			}
		}

		public Settings Load()
		{
			try
			{
				Settings settings;
				var folder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				var path = Path.Combine(folder, FileName);

				if (File.Exists (path)) {
					using (StreamReader sr = File.OpenText (path)) {
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

