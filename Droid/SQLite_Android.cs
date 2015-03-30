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

namespace Set
{
	public class SQLite_Android : ISQLite
	{
		public SQLite_Android ()
		{
		}

		#region ISQLite implementation
		public SQLiteAsyncConnection GetConnection ()
		{
			var sqliteFilename = "OneSet.db3";
			var documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			var path = Path.Combine (documentsPath, sqliteFilename);
			var platform = new SQLitePlatformAndroid ();

			var connectionWithLock = new SQLiteConnectionWithLock (
				platform,
				new SQLiteConnectionString (path, true));

			var connection = new SQLiteAsyncConnection (() => connectionWithLock);

			return connection;


//
//			var sqliteFilename = "OneSet.db3";
//			string documentsPath = System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal); 
//			var path = Path.Combine(documentsPath, sqliteFilename);
//
//			// This is where we copy in the prepopulated database
//			Console.WriteLine (path);
//			if (!File.Exists(path))
//			{
//				var s = Forms.Context.Resources.OpenRawResource(Set.Droid.Resource.Raw.OneSet);  // RESOURCE NAME ###
//
//				// create a write stream
//				FileStream writeStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
//				// write to the stream
//				ReadWriteStream(s, writeStream);
//			}
//
//
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