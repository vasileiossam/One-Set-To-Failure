using System;
using Set.Models;
using SQLite.Net;
using Set.Abstract;

namespace Set.Concrete
{
	public class SetRepository : BaseRepository<Set>, ISetRepository
	{
        public SetRepository(SQLiteConnection connection)
			: base(connection)
		{

		}
	}
}
