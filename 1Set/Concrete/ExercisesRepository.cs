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

	}
}
