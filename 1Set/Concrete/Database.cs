using System;
using SQLite.Net;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Set.Abstract;
using Set.Models;

namespace Set
{
	public class Database 
	{
		static object locker = new object ();

		SQLiteConnection _connection;

		/// <summary>
		/// Initializes a new instance of the <see cref="Tasky.DL.TaskDatabase"/> TaskDatabase. 
		/// if the database doesn't exist, it will create the database and all the tables.
		/// </summary>
		/// <param name='path'>
		/// Path.
		/// </param>
		public Database()
		{
			_connection = DependencyService.Get<ISQLite> ().GetConnection ();

			// create the tables
			_connection.CreateTable<Exercise>();
		}


		public IEnumerable<Exercise> GetItemsNotDone ()
		{
			lock (locker) {
				return _connection.Query<Exercise>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
			}
		}

	}
}

