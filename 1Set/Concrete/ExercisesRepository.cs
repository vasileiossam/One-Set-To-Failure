using System;
using Set.Models;
using SQLite.Net;
using SQLite.Net.Async;
using Set.Abstract;
using System.Threading.Tasks;

namespace Set.Concrete
{
	public class ExercisesRepository : BaseRepository<Exercise>, IExercisesRepository
	{
		public ExercisesRepository(SQLiteAsyncConnection connection)
			: base(connection)
		{

		}

		public override async Task<int> DeleteAsync(int id)
		{
			using (await Mutex.LockAsync ().ConfigureAwait (false))
			{				
				await _connection.ExecuteAsync ("DELETE FROM RoutineDays WHERE ExerciseId = ?", id);
				await _connection.ExecuteAsync ("DELETE FROM Workouts WHERE ExerciseId = ?", id);
				return await _connection.DeleteAsync<Exercise> (id);			
			}
		}
	}
}
