using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public async Task<Workout> GetPreviousWorkout(int exerciseId, DateTime created)
        {
			using (await Mutex.LockAsync ().ConfigureAwait (false))
			{				
				var previousWorkout = await _connection.Table<Workout> ()
					.Where (x => x.ExerciseId == exerciseId && x.Created < created)
					.OrderByDescending (x => x.Created)
					.FirstOrDefaultAsync ();
                return previousWorkout;
			}
        }

		public async Task<int> GetTotalTrophies()
		{
			using (await Mutex.LockAsync ().ConfigureAwait (false))
			{				
				const string sql = @"SELECT coalesce(SUM(Trophies), 0) FROM Workouts";
                var result = await _connection.ExecuteScalarAsync<int>(sql);
                return result;
            }
		}

		public async Task<int> GetTrophies(DateTime date)
		{
            using (await Mutex.LockAsync ().ConfigureAwait (false))
			{				
				const string sql = @"SELECT coalesce(SUM(Trophies), 0) FROM Workouts WHERE Created = ?";
                var result = await _connection.ExecuteScalarAsync<int>(sql, date);
                return result;
            }
		}

        public async Task<int> GetWorkoutsCount(int exerciseId)
        {
            using (await Mutex.LockAsync().ConfigureAwait(false))
            {
                const string sql = @"SELECT coalesce(Count(*), 0) FROM Workouts WHERE ExerciseId = ?";
                var result = await _connection.ExecuteScalarAsync<int>(sql, exerciseId);
                return result;
            }
        }
    }
}