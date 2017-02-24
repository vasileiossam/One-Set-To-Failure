using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneSet.Abstract;
using OneSet.Converters;
using OneSet.Models;
using SQLite;
using Xamarin.Forms;

namespace OneSet.Data
{
	public class Database 
	{
		private static readonly AsyncLock Mutex = new AsyncLock ();
		private readonly SQLiteAsyncConnection _connection;

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
        public List<ImagePack> ImagePacks => _imagePack ?? (_imagePack = new List<ImagePack>
        {
            new ImagePack {ImagePackId = 1, Title = "25 Fitness Quotes"},
            new ImagePack {ImagePackId = 2, Title = "20 Inspirational and Motivational Quotes"}
        });

	    /// <summary>
	    /// if the database doesn't exist, it will create the database and all the tables.
	    /// </summary>
	    public Database(SQLiteAsyncConnection connection)
		{
			_connection = connection;
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
	}
}

