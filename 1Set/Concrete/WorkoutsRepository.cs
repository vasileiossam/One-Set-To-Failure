using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Set.Abstract;
using Set.Models;
using SQLite.Net.Async;
using System.Diagnostics;

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
			var list = new List<Workout> ();
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

		public override async Task<List<Workout>> AllAsync()
		{
			// http://www.captechconsulting.com/blog/nicholas-cipollina/cross-platform-sqlite-support-%E2%80%93-part-1
			try
			{
				List<Workout> list = new List<Workout> ();
				using (await Mutex.LockAsync ().ConfigureAwait (false)) 
				{
					list = await _connection.Table<Workout> ().OrderBy(x=>x.Created).ToListAsync ().ConfigureAwait (false);
				}

				return list;
			}
			catch(Exception ex)
			{
				Debug.WriteLine (ex.Message);
			}
			return null;
		}
    }
}