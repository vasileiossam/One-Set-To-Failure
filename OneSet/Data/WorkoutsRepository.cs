using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using OneSet.Abstract;
using OneSet.Models;
using SQLite;

namespace OneSet.Data
{
	public class WorkoutsRepository : BaseRepository<Workout>, IWorkoutsRepository
	{
		public WorkoutsRepository(SQLiteAsyncConnection connection)
			: base(connection)
		{

		}

        public override async Task<List<Workout>> AllAsync()
        {
            // http://www.captechconsulting.com/blog/nicholas-cipollina/cross-platform-sqlite-support-%E2%80%93-part-1
            try
            {
                List<Workout> list;
                using (await Mutex.LockAsync().ConfigureAwait(false))
                {
                    list = await _connection.Table<Workout>().OrderBy(x => x.Created).ToListAsync().ConfigureAwait(false);
                }

                return list;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return null;
        }

        public override async Task<Workout> FindAsync(int id)
        {
            var result = await base.FindAsync(id);
            return result;
        }

        public async Task<List<Workout>> GetWorkouts(DateTime date)
		{
		    using (await Mutex.LockAsync ().ConfigureAwait (false))
			{				
				const string sql = @"SELECT *  
                    FROM Workouts
                    WHERE Created = ?";
	
				var list = await _connection.QueryAsync<Workout> (sql, date);
                return list;
			}
		}

        public async Task<Workout> GetPreviousWorkout(Workout workout)
        {
			using (await Mutex.LockAsync ().ConfigureAwait (false))
			{				
				var previousWorkout = await _connection.Table<Workout> ()
					.Where (x => (x.ExerciseId == workout.ExerciseId) && (x.Created < workout.Created))
					.OrderByDescending (x => x.Created)
					.FirstOrDefaultAsync ();
                return previousWorkout;
			}
        }

		public async Task<int> GetTotalTrophies()
		{
			using (await Mutex.LockAsync ().ConfigureAwait (false))
			{				
				var sql = @"SELECT SUM(Trophies) AS TotalTrophies FROM Workouts";
                // couldn't make ExecuteScalarAsync<int> or ExecuteScalarAsync<int?> work properly; 
                // it was throwing NullReferenceException when no data
                var result = await _connection.QueryAsync<int>(sql);
                if (result != null && result.Count > 0) return result[0];
                return 0;
            }
		}

		public async Task<int> GetTrophies(DateTime date)
		{
            using (await Mutex.LockAsync ().ConfigureAwait (false))
			{				
				var sql = @"SELECT SUM(Trophies) AS DayTrophies FROM Workouts WHERE Created = ?";
                // couldn't make ExecuteScalarAsync<int> or ExecuteScalarAsync<int?> work properly; 
                // it was throwing NullReferenceException when no data
                var result = await _connection.QueryAsync<int>(sql, date);
                if (result != null && result.Count > 0) return result[0];
                return 0;
			}
		}

        public async Task<int> GetWorkoutsCount(int exerciseId)
        {
            using (await Mutex.LockAsync().ConfigureAwait(false))
            {
                var sql = @"SELECT Count(*) FROM Workouts WHERE ExerciseId = ?";
                var result = await _connection.QueryAsync<int>(sql, exerciseId);
                if (result != null && result.Count > 0) return result[0];
                return 0;
            }
        }
    }
}