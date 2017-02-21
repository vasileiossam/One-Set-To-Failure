using System;
using Xamarin.Forms;
using System.IO;
using OneSet.Abstract;
using OneSet.Droid;
using SQLite;

[assembly: Dependency (typeof (SQLiteAndroid))]

namespace OneSet.Droid
{
	#if DEBUG
	//public class TraceListener : ITraceListener
	//{
	//	public void Receive (string message)
	//	{
	//		Console.WriteLine(message);
	//		Logger.Info ("TraceListener", message);
	//	}
	//}
	#endif

    // ReSharper disable once InconsistentNaming
	public class SQLiteAndroid : ISQLite
	{
		public const string FileName = "OneSet.db3";

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
					var s = Forms.Context.Resources.OpenRawResource(Resource.Raw.OneSet);  // RESOURCE NAME ###

					// create a write stream
					var writeStream = new FileStream(pathName, FileMode.OpenOrCreate, FileAccess.Write);
				
					// write to the stream
					ReadWriteStream(s, writeStream);
				}					

				// var platform = new SQLitePlatformAndroid ();
				// storeDateTimeAsTicks  = false
				// set it to false if I want the date fields to created as string instead of bigint
				// http://stackoverflow.com/questions/21460271/community-sqlite-not-handling-dates-datetimes-correctly
				// var connectionStr = new SQLiteConnectionString (pathName, false); 
				// var connectionWithLock = new SQLiteConnectionWithLock (platform, connectionStr);

				#if DEBUG
				//var listener = new TraceListener();
				//connectionWithLock.TraceListener = listener;
				#endif

				var connection = new SQLiteAsyncConnection(pathName, false);
				return connection;
			}
			catch(Exception ex)
			{
				throw new Exception ("Cannot get connection", ex);
			}
		}
		#endregion

		/// <summary>
		/// helper method to get the database out of /raw/ and into the user filesystem
		/// </summary>
		void ReadWriteStream(Stream readStream, Stream writeStream)
		{
			const int length = 256;
			var buffer = new byte[length];
			var bytesRead = readStream.Read(buffer, 0, length);
			// write the required bytes
			while (bytesRead > 0)
			{
				writeStream.Write(buffer, 0, bytesRead);
				bytesRead = readStream.Read(buffer, 0, length);
			}
			readStream.Close();
			writeStream.Close();
		}
	}
}