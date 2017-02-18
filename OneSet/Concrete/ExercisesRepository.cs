using System.Threading.Tasks;
using OneSet.Abstract;
using OneSet.Entities;
using SQLite;

namespace OneSet.Concrete
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
                var item = FindAsync(id);
                if (item != null)
                {
                    await _connection.ExecuteAsync("DELETE FROM RoutineDays WHERE ExerciseId = ?", id);
                    await _connection.ExecuteAsync("DELETE FROM Workouts WHERE ExerciseId = ?", id);
                    return await _connection.DeleteAsync(item);
                }
                return 0;
			}
		}

	}
}
