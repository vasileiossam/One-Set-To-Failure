using System;
using Set.Models;
using SQLite.Net;
using Set.Abstract;

namespace Set.Concrete
{
	public class ExercisesRepository : BaseRepository<Exercise>, IExercisesRepository
	{
		public ExercisesRepository(SQLiteConnection connection)
			: base(connection)
		{

		}

		public override int Delete(int id)
		{
			lock (_locker) 
			{
				_connection.Execute ("DELETE FROM RoutineDays WHERE ExerciseId = ?", id);
				_connection.Execute ("DELETE FROM Workouts WHERE ExerciseId = ?", id);
				return _connection.Delete<Exercise>(id);			
			}
		}
	}
}
