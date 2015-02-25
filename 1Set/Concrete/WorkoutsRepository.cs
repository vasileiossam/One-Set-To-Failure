using System;
using Set.Models;
using SQLite.Net;
using Set.Abstract;

namespace Set.Concrete
{
	public class WorkoutsRepository : BaseRepository<Workout>, IWorkoutsRepository
	{
		public WorkoutsRepository(SQLiteConnection connection)
			: base(connection)
		{

		}



    }
}