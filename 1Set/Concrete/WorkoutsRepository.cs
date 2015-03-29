using System;
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
			var sql = @"SELECT *  
                    FROM Workouts
                    WHERE Created = ?";

			return await _connection.QueryAsync<Workout> (sql, date);
		}

        public async Task<Workout> GetPreviousWorkout(int exerciseId, DateTime created)
        {
			var all = await AllAsync();
			var result = all.Where(x => (x.ExerciseId == exerciseId) && (x.Created < created)).OrderByDescending(x => x.Created).FirstOrDefault(); 
			return result;
        }
    }
}