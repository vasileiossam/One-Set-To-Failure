using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OneSet.Models;
using SQLite;

namespace OneSet.Data
{
	public class Database 
	{
		//private static readonly AsyncLock Mutex = new AsyncLock ();
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

	    public Database(SQLiteAsyncConnection connection)
		{
			_connection = connection;
		    CreateTables();
		}

        /// <summary>
        /// Create tables if not exist
        /// </summary>
        private void CreateTables()
		{
            _connection.CreateTableAsync<Exercise>().Wait();
            _connection.CreateTableAsync<RoutineDay>().Wait();
            _connection.CreateTableAsync<Workout>().Wait();
            _connection.CreateTableAsync<Calendar>().Wait();
        }

        public async Task ClearWorkoutData()
		{
            await _connection.ExecuteAsync("DELETE FROM Calendar");
            await _connection.ExecuteAsync("DELETE FROM Workouts");
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

