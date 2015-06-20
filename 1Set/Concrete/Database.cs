using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Set.Abstract;
using Set.Concrete;
using Set.Models;
using SQLite.Net.Async;
using Xamarin.Forms;
using System.Text;
using System.Linq;
using System.Diagnostics;

namespace Set
{
	public class Database 
	{
		private static readonly AsyncLock Mutex = new AsyncLock ();
		private readonly SQLiteAsyncConnection _connection;

        private WorkoutsRepository _workoutRepository;
        public WorkoutsRepository WorkoutsRepository
        {
            get { return _workoutRepository ?? (_workoutRepository = new WorkoutsRepository(_connection)); }
        }

		private RoutineDaysRepository _routineDaysRepository;
		public RoutineDaysRepository RoutineDaysRepository
		{
			get { return _routineDaysRepository ?? (_routineDaysRepository = new RoutineDaysRepository(_connection)); }
		}

		private CalendarRepository _calendarRepository;
		public CalendarRepository CalendarRepository
		{
			get { return _calendarRepository ?? (_calendarRepository = new CalendarRepository(_connection)); }
		}

		private ExercisesRepository _exercisesRepository;
		public ExercisesRepository ExercisesRepository
		{
			get { return _exercisesRepository ?? (_exercisesRepository = new ExercisesRepository(_connection)); }
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

					// +1 in every three workouts
					_repsIncrements.Add(new RepsIncrement(30, 1, 3));

					// +1 in every four workout
					_repsIncrements.Add(new RepsIncrement(40, 1, 4));

					// +1 in every five workout
					_repsIncrements.Add(new RepsIncrement(50, 1, 5));
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
                    _imagePack = new List<ImagePack>
                    {
                        new ImagePack() {ImagePackId = 1, Title = "25 Fitness Quotes"},
                        new ImagePack() {ImagePackId = 2, Title = "20 Inspirational and Motivational Quotes"}
                    };
                }
                return _imagePack;
            }
        }

	    /// <summary>
	    /// if the database doesn't exist, it will create the database and all the tables.
	    /// </summary>
	    public Database()
		{
			_connection = DependencyService.Get<ISQLite> ().GetConnection ();
			CreateDatabaseAsync ();
		}

		public async Task CreateDatabaseAsync ()
		{
			try
			{
				using (await Mutex.LockAsync ().ConfigureAwait (false)) 
				{
					//await _connection.DropTableAsync<Exercise> ().ConfigureAwait (false);
					//await _connection.DropTableAsync<RoutineDay> ().ConfigureAwait (false);
					//await _connection.DropTableAsync<Workout> ().ConfigureAwait (false);
					//await _connection.DropTableAsync<Calendar> ().ConfigureAwait (false);

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
			var exersisesList = await App.Database.ExercisesRepository.AllAsync ();
			var workoutsList = await App.Database.WorkoutsRepository.AllAsync ();

			foreach (var workout in workoutsList)
			{
				workout.Exercise = exersisesList.FirstOrDefault (x => x.ExerciseId == workout.ExerciseId);

				if (!await workout.LoadAsync ())
				{
					break;
				}
				workout.Trophies = WorkoutRules.GetTrophies(workout);
				await App.Database.WorkoutsRepository.SaveAsync(workout);
			}
			App.TotalTrophies = await App.Database.WorkoutsRepository.GetTotalTrophies ();
		}

		public async Task<string> ExportToCsv()
		{
			try
			{
				var sb = new StringBuilder();
				var workouts = await WorkoutsRepository.AllAsync();
				workouts = workouts.OrderBy(x => x.Created).ToList();
				var exercises = await ExercisesRepository.AllAsync();

				sb.AppendLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\"", 
					"Date",
					"Exercise",
					"Reps",
					"Weight",
					"Target Reps",
					"Target Weight",
					"Trophies",
					"Workout Notes"
				));

				foreach(Workout workout in workouts)
				{
					workout.Exercise = exercises.FirstOrDefault(x => x.ExerciseId == workout.ExerciseId);
					if (workout.Exercise == null) continue;

					var line = string.Format("{0},\"{1}\",{2},{3},{4},{5},{6},\"{7}\"", 
						workout.Created.Date.ToString("d"),
						workout.Exercise.Name,
						workout.Reps,
						workout.Weight,
						workout.TargetReps,
						workout.TargetWeight,
						workout.Trophies,
						workout.Notes
					);

					sb.AppendLine(line);
				}

				return DependencyService.Get<IExporter> ().ExportToCsv (sb);
			}
			catch(Exception ex)
			{
				App.ShowErrorPage (this, ex);				
			}

			return string.Empty;
		}
	}
}

