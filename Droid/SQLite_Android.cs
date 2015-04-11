using System;
using Set.Abstract;
using Set;
using Xamarin.Forms;
using System.IO;
using Set.Droid;
using SQLite.Net.Async;
using SQLite.Net;
using SQLite.Net.Platform.XamarinAndroid;

[assembly: Dependency (typeof (SQLite_Android))]

namespace Set.Droid
{
	public class SQLite_Android : ISQLite
	{
		public const string FileName = "OneSet.db3";

		public SQLite_Android ()
		{
		}

		#region ISQLite implementation

		public static string GetPathName()
		{
			var documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			var pathName = Path.Combine (documentsPath, FileName);
			return pathName;
		}

		public SQLiteAsyncConnection GetConnection ()
		{
			try
			{
				var pathName = GetPathName();

				if (!File.Exists(pathName))
				{
					var s = Forms.Context.Resources.OpenRawResource(Set.Droid.Resource.Raw.OneSet);  // RESOURCE NAME ###

					// create a write stream
					var writeStream = new FileStream(pathName, FileMode.OpenOrCreate, FileAccess.Write);
				
					// write to the stream
					ReadWriteStream(s, writeStream);
				}					

				var platform = new SQLitePlatformAndroid ();
				var connectionWithLock = new SQLiteConnectionWithLock (
					platform,
					new SQLiteConnectionString (pathName, true));

				var connection = new SQLiteAsyncConnection (() => connectionWithLock);

				return connection;
			}
			catch(Exception ex)
			{
				throw new Exception ("Cannot get connection", ex);
			}

//			var connectionParameters = new SQLiteConnectionString(path, false); 
//			var platform = new SQLitePlatformAndroid();
//			var sqliteConnectionPool = new SQLiteConnectionPool(platform); 
//			var conn = new SQLiteAsyncConnection(
//				() => sqliteConnectionPool.GetConnection(connectionParameters)
//			); 
//
//			// Return the database connection 
//			return conn;
		}
		#endregion

		/// <summary>
		/// helper method to get the database out of /raw/ and into the user filesystem
		/// </summary>
		void ReadWriteStream(Stream readStream, Stream writeStream)
		{
			int Length = 256;
			Byte[] buffer = new Byte[Length];
			int bytesRead = readStream.Read(buffer, 0, Length);
			// write the required bytes
			while (bytesRead > 0)
			{
				writeStream.Write(buffer, 0, bytesRead);
				bytesRead = readStream.Read(buffer, 0, Length);
			}
			readStream.Close();
			writeStream.Close();
		}
	}
}