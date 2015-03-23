using System;
using Set.Models;
using SQLite.Net;
using Set.Abstract;
using System.Collections.Generic;
using System.Linq;

namespace Set.Concrete
{
	public class CalendarRepository : BaseRepository<Calendar>, ICalendarRepository
	{
		public CalendarRepository(SQLiteConnection connection)
			: base(connection)
		{

		}
	}
}
