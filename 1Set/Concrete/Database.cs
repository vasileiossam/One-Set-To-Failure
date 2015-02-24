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
		private SQLiteConnection _connection;

        private WorkoutRepository _workoutRepository;
        public WorkoutRepository WorkoutRepository
        {
            get
            {
                if (_workoutRepository == null)
                {
                    _workoutRepository = new WorkoutRepository(_connection);
                }
                return _workoutRepository;
            }
        }

		/// <summary>
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
	}
}

