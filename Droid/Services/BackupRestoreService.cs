using System;
using System.IO;
using System.Threading.Tasks;
using OneSet.Abstract;
using OneSet.Droid.Services;
using OneSet.Models;
using Xamarin.Forms;

[assembly:Dependency(typeof(BackupRestoreService))]

namespace OneSet.Droid.Services
{
	public class BackupRestoreService : IBackupRestoreService
	{
        private static string DownloadsFolder
        {
            get
            {
                var filePath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).ToString();
                filePath = Path.Combine(filePath, "OneSet/");
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                return filePath;
            }
        }

        public async Task Backup()
		{
			// backup SQLITE
			var source = SQLiteAndroid.GetPathName ();
			var dest = Path.Combine(DownloadsFolder, SQLiteAndroid.FileName); 
			File.Copy (source, dest, true);

			// backup settings
			source = SettingsStorage.GetPathName ();
			if (File.Exists (source))
			{
				dest = Path.Combine(DownloadsFolder, SettingsStorage.FileName); 
				File.Copy (source, dest, true);
			}

			// following statement will prevent a compiler warning about async method lacking await
			await Task.FromResult(0);
		}

		public async Task Restore()
		{
			// restore SQLITE
			var source = Path.Combine(DownloadsFolder, SQLiteAndroid.FileName); 
			var dest = SQLiteAndroid.GetPathName ();
			File.Copy (source, dest, true);

			// restore settings
			source = Path.Combine(DownloadsFolder, SettingsStorage.FileName); 
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

			return new BackupInfo
			{
				BackupFolder = DownloadsFolder,
				LastBackupDate = GetLastBackupDate()
			};
		}

		private DateTime? GetLastBackupDate()
		{
			var pathName = Path.Combine(DownloadsFolder, SQLiteAndroid.FileName); 
			if (!File.Exists (pathName))
				return null;
			return File.GetLastWriteTime (pathName);
		}
	}
}

