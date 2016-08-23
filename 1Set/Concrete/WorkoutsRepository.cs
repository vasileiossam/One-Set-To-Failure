using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Set.Abstract;
using Set.Entities;
using System.Diagnostics;
using SQLite;
using System.Linq;

namespace Set.Concrete
{
	public class WorkoutsRepository : BaseRepository<Workout>, IWorkoutsRepository
	{
		public WorkoutsRepository(SQLiteAsyncConnection connection)
			: base(connection)
		{

		}

        public async Task LoadRelations(Workout workout)
        {
            workout.Exercise = await App.Database.ExercisesRepository.FindAsync(workout.ExerciseId);
        }

        public async Task LoadRelations(List<Workout> workouts)
        {
            var exercises = await App.Database.ExercisesRepository.AllAsync();
            foreach(var workout in workouts)
            {
                workout.Exercise = exercises.FirstOrDefault(x => x.ExerciseId == workout.ExerciseId);
            }
        }

        public override async Task<List<Workout>> AllAsync()
        {
            // http://www.captechconsulting.com/blog/nicholas-cipollina/cross-platform-sqlite-support-%E2%80%93-part-1
            try
            {
                List<Workout> list = new List<Workout>();
                using (await Mutex.LockAsync().ConfigureAwait(false))
                {
                    list = await _connection.Table<Workout>().OrderBy(x => x.Created).ToListAsync().ConfigureAwait(false);
                }

                await LoadRelations(list);
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
            await LoadRelations(result);
            return result;
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
                await LoadRelations(list);
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

                await LoadRelations(previousWorkout);
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