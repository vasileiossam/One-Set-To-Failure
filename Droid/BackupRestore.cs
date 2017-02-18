using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.IO;
using OneSet.Abstract;
using OneSet.Models;

[assembly:Dependency(typeof(OneSet.Droid.BackupRestore))]

namespace OneSet.Droid
{
	public class BackupRestore : IBackupRestore
	{

		public async Task Backup()
		{
			// backup SQLITE
			var source = SQLite_Android.GetPathName ();
			var dest = Path.Combine(GetBackupFolder(), SQLite_Android.FileName); 
			File.Copy (source, dest, true);

			// backup settings
			source = SettingsStorage.GetPathName ();
			if (File.Exists (source))
			{
				dest = Path.Combine(GetBackupFolder(), SettingsStorage.FileName); 
				File.Copy (source, dest, true);
			}

			// following statement will prevent a compiler warning about async method lacking await
			await Task.FromResult(0);
		}

		public async Task Restore()
		{
			// restore SQLITE
			var source = Path.Combine(GetBackupFolder(), SQLite_Android.FileName); 
			var dest = SQLite_Android.GetPathName ();
			File.Copy (source, dest, true);

			// restore settings
			source = Path.Combine(GetBackupFolder(), SettingsStorage.FileName); 
			if (File.Exists (source))
			{
				dest = SettingsStorage.GetPathName ();
				File.Copy (source, dest, true);
			}

			// following statement will prevent a compiler warning about async method lacking await
			await Task.FromResult(0);
		}

		public async Task<BackupInfo> GetBackupInfo()
		{
			// following statement will prevent a compiler warning about async method lacking await
			await Task.FromResult(0);

			return new BackupInfo () {
				BackupFolder = GetBackupFolder(),
				LastBackupDate = GetLastBackupDate()
			};
		}

		private string GetBackupFolder()
		{
			var rootPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
			var backupFolder = System.IO.Path.Combine(rootPath, "OneSet");

			if (!Directory.Exists(backupFolder))
			{
				Directory.CreateDirectory(backupFolder);
			}

			return backupFolder;
		}

		private DateTime? GetLastBackupDate()
		{
			var pathName = Path.Combine(GetBackupFolder(), SQLite_Android.FileName); 
			if (!File.Exists (pathName))
				return null;
			return File.GetLastWriteTime (pathName);
		}
	}
}

