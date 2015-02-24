using System;
using Set.Models;
using SQLite.Net;
using Set.Abstract;

namespace Set.Concrete
{
	public class ExerciseRepository : BaseRepository<Exercise>, IExerciseRepository
	{
        public ExerciseRepository(SQLiteConnection connection)
			: base(connection)
		{

		}
	}
}
