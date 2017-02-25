using System.Threading.Tasks;
using OneSet.Abstract;
using OneSet.Models;
using SQLite;

namespace OneSet.Data
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
			    await _connection.ExecuteAsync("DELETE FROM RoutineDays WHERE ExerciseId = ?", id);
			    await _connection.ExecuteAsync("DELETE FROM Workouts WHERE ExerciseId = ?", id);
                await _connection.ExecuteAsync("DELETE FROM Exercises WHERE ExerciseId = ?", id);
			    return 0;
			}
		}

	}
}
