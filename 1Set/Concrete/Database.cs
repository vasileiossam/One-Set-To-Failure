using System;
using SQLite.Net;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Set.Abstract;
using Set.Models;
using Set.Concrete;

namespace Set
{
	public class Database 
	{
		static object locker = new object ();
		private SQLiteConnection _connection;

        private WorkoutsRepository _workoutRepository;
        public WorkoutsRepository WorkoutsRepository
        {
            get
            {
                if (_workoutRepository == null)
                {
                    _workoutRepository = new WorkoutsRepository(_connection);
                }
                return _workoutRepository;
            }
        }

		private RoutineDaysRepository _routineDaysRepository;
		public RoutineDaysRepository RoutineDaysRepository
		{
			get
			{
				if (_routineDaysRepository == null)
				{
					_routineDaysRepository = new RoutineDaysRepository(_connection);
				}
				return _routineDaysRepository;
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
			_connection.CreateTable<RoutineDay>();
			_connection.CreateTable<Exercise>();
			_connection.CreateTable<Workout>();
		}
	}
}

