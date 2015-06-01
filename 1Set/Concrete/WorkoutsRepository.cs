﻿using System;
using Set.Models;
using SQLite.Net;
using SQLite.Net.Async;
using Set.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Set.Concrete
{
	public class WorkoutsRepository : BaseRepository<Workout>, IWorkoutsRepository
	{
		public WorkoutsRepository(SQLiteAsyncConnection connection)
			: base(connection)
		{

		}

		public async Task<List<Workout>> GetWorkouts(DateTime date)
		{
			List<Workout> list = new List<Workout> ();
			using (await Mutex.LockAsync ().ConfigureAwait (false))
			{				
				var sql = @"SELECT *  
                    FROM Workouts
                    WHERE Created = ?";
	
				list = await _connection.QueryAsync<Workout> (sql, date);
				return list;
			}
		}

        public async Task<Workout> GetPreviousWorkout(int exerciseId, DateTime created)
        {
			using (await Mutex.LockAsync ().ConfigureAwait (false))
			{				
				var workout = await _connection.Table<Workout> ()
					.Where (x => (x.ExerciseId == exerciseId) && (x.Created < created))
					.OrderByDescending (x => x.Created)
					.FirstOrDefaultAsync ();

				return workout;
			}
        }

		public async Task<int> GetTotalTrophies()
		{
			using (await Mutex.LockAsync ().ConfigureAwait (false))
			{				
				var sql = @"SELECT SUM(Trophies) AS TotalTrophies FROM Workouts";
				var totalTrophies = await _connection.ExecuteScalarAsync<int?>(sql);
				return totalTrophies ?? 0;
			}
		}

		public async Task<int> GetTrophies(DateTime date)
		{
			using (await Mutex.LockAsync ().ConfigureAwait (false))
			{				
				var sql = @"SELECT SUM(Trophies) AS DayTrophies FROM Workouts WHERE Created = ?";
				var dayTrophies = await _connection.ExecuteScalarAsync<int?>(sql, date);
				return dayTrophies ?? 0;
			}
		}
    }
}