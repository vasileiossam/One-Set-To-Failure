using System;
using Set.Models;
using SQLite.Net;
using Set.Abstract;

namespace Set.Concrete
{
	public class WorkoutRepository : BaseRepository<Workout>, IWorkoutRepository
	{
		public WorkoutRepository(SQLiteConnection connection)
			: base(connection)
		{

		}

	}
}
