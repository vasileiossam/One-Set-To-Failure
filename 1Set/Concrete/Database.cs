using System;
using SQLite.Net.Async;
using SQLite.Net;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Set.Abstract;
using Set.Models;
using Set.Concrete;
using Set.Resx;
using System.Threading.Tasks;

namespace Set
{
	public class Database 
	{
		private static readonly AsyncLock Mutex = new AsyncLock ();
		private readonly SQLiteAsyncConnection _connection;

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

		private CalendarRepository _calendarRepository;
		public CalendarRepository CalendarRepository
		{
			get
			{
				if (_calendarRepository == null)
				{
					_calendarRepository = new CalendarRepository(_connection);
				}
				return _calendarRepository;
			}
		}

		private ExercisesRepository _exercisesRepository;
		public ExercisesRepository ExercisesRepository
		{
			get
			{
				if (_exercisesRepository == null)
				{
					_exercisesRepository = new ExercisesRepository(_connection);
				}
				return _exercisesRepository;
			}
		}
	
		/// <summary>
		///    +1
		///    +2
		///    +1 every other workout
        ///    +1 every 3 workouts
        ///    +1 every 4 workouts
		/// </summary>
		/// <value>The rest timers.</value>
		private List<RepsIncrement> _repsIncrements;
		public List<RepsIncrement> RepsIncrements
		{
			get
			{
				if (_repsIncrements == null)
				{
					_repsIncrements = new List<RepsIncrement>();
                    
                    _repsIncrements.Add(new RepsIncrement(1, 0, 0));
					_repsIncrements.Add(new RepsIncrement(2, 1, 0));
					_repsIncrements.Add(new RepsIncrement(3, 2, 0));
					_repsIncrements.Add(new RepsIncrement(4, 1, 2));
					_repsIncrements.Add(new RepsIncrement(5, 1, 3));
					_repsIncrements.Add(new RepsIncrement(6, 1, 4));
				}
				return _repsIncrements;
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
			CreateDatabaseAsync ();
		}

		public async Task CreateDatabaseAsync ()
		{
			//	_connection.DropTable<Exercise> ();
			//	_connection.DropTable<RoutineDay> ();
			//await _connection.DropTableAsync<Workout> ();

			try
			{
				using (await Mutex.LockAsync ().ConfigureAwait (false)) 
				{
					await _connection.CreateTableAsync<Exercise> ().ConfigureAwait (false);
					await _connection.CreateTableAsync<RoutineDay> ().ConfigureAwait (false);
					await _connection.CreateTableAsync<Workout> ().ConfigureAwait (false);
					await _connection.CreateTableAsync<Calendar> ().ConfigureAwait (false);
				}
			}
			catch(Exception ex)
			{
				App.ShowErrorPage (this, ex);
			}
		}

		public async Task ClearWorkoutData()
		{
			await _connection.ExecuteAsync ("DELETE FROM Workouts");
		}

		public async Task LoadLifeFitnessData()
		{
			await _connection.ExecuteAsync ("INSERT INTO Exercises (Name, PlateWeight) VALUES (?, ?)", "LF Pectoral Fly", 7);
			await _connection.ExecuteAsync ("INSERT INTO Exercises (Name, PlateWeight) VALUES (?, ?)", "LF Chess Press", 7);
			await _connection.ExecuteAsync ("INSERT INTO Exercises (Name, PlateWeight) VALUES (?, ?)", "LF Shoulder Press", 7);
			await _connection.ExecuteAsync ("INSERT INTO Exercises (Name, PlateWeight) VALUES (?, ?)", "LF Triceps Extension", 7);
			await _connection.ExecuteAsync ("INSERT INTO Exercises (Name, PlateWeight) VALUES (?, ?)", "LF Biceps Curl", 7);
			await _connection.ExecuteAsync ("INSERT INTO Exercises (Name, PlateWeight) VALUES (?, ?)", "LF Lat Pulldown", 7);
		}
	}
}

