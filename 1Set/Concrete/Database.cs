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

		private List<RepsIncrement> _repsIncrements;
		public List<RepsIncrement> RepsIncrements
		{
			get
			{
				if (_repsIncrements == null)
				{
					_repsIncrements = new List<RepsIncrement>();

					// no increment
                    _repsIncrements.Add(new RepsIncrement(1, 0, 0));

					// +1 in every workout
					_repsIncrements.Add(new RepsIncrement(10, 1, 1));

					// +1 in every other workout
					_repsIncrements.Add(new RepsIncrement(20, 1, 2));
				}
				return _repsIncrements;
			}
		}

        private List<ImagePack> _imagePack;
        public List<ImagePack> ImagePacks
        {
            get
            {
                if (_imagePack == null)
                {
                    _imagePack = new List<ImagePack>();
                    _imagePack.Add(new ImagePack() { ImagePackId = 1, Title = "25 Fitness Quotes" });
					_imagePack.Add(new ImagePack() { ImagePackId = 2, Title = "20 Inspirational and Motivational Quotes" });
                }
                return _imagePack;
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

		public async Task RecalcStatistics()
		{
			var workoutsList = await App.Database.WorkoutsRepository.AllAsync ();
			foreach (var workout in workoutsList)
			{
				await workout.LoadAsync ();
				workout.Trophies = WorkoutRules.GetTrophies(workout);
				await App.Database.WorkoutsRepository.SaveAsync(workout);
			}
			App.TotalTrophies = await App.Database.WorkoutsRepository.GetTotalTrophies ();
		}

	}
}

