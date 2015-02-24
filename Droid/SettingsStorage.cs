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


		public async Task SaveAsync(Settings settings)
		{
			var folder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var path = Path.Combine(folder, FileName);

			using (StreamWriter sw = File.CreateText (path)) {
				var json = JsonConvert.SerializeObject (settings);
				await sw.WriteAsync (json);
			}
		}

		public async Task<Settings> LoadAsync()
		{
			Settings settings;
			var folder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var path = Path.Combine(folder, FileName);

			if (File.Exists (path)) {
				using (StreamReader sr = File.OpenText (path)) {
					var json = await sr.ReadToEndAsync ();
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

