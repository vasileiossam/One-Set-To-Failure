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
		///  hide rest timer
		///  30 secs
		///  45 secs
		///  1 min
		///  1 min 30 secs
		///  1 min 45 secs
		///  2 min
		///  2 min 30 secs
		/// </summary>
		private List<RestTimer> _restTimers;
		public List<RestTimer> RestTimers
		{
			get
			{
				if (_restTimers == null)
				{
					_restTimers = new List<RestTimer>();

					_restTimers.Add(new RestTimer(1, AppResources.ZeroSecsRestTimerDescription, 0 ));
					_restTimers.Add(new RestTimer(2, "30 " + AppResources.SecondsRestTimerDescription, 30));
					_restTimers.Add(new RestTimer(3, "45 " + AppResources.SecondsRestTimerDescription, 45));
					_restTimers.Add(new RestTimer(4, "1 " + AppResources.MinuteRestTimerDescription, 60));
					_restTimers.Add(new RestTimer(5, "1 " + AppResources.MinuteRestTimerDescription + " 30 " + AppResources.SecondsRestTimerDescription, 60 + 30));
					_restTimers.Add(new RestTimer(6, "1 " + AppResources.MinuteRestTimerDescription + " 45 " + AppResources.SecondsRestTimerDescription, 60 + 45));
					_restTimers.Add(new RestTimer(7, "2 " + AppResources.MinutesRestTimerDescription, 2 * 60 ));
					_restTimers.Add(new RestTimer(8, "2 " + AppResources.MinutesRestTimerDescription + " 30 " + AppResources.SecondsRestTimerDescription, 2 * 60 + 30 ));
				}
				return _restTimers;
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
			//	_connection.DropTable<Workout> ();

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
	}
}

