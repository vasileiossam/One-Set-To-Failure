using System;
using Set.Models;
using SQLite.Net;
using SQLite.Net.Async;
using Set.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Set.Concrete
{
	public class CalendarRepository : BaseRepository<Calendar>, ICalendarRepository
	{
		public CalendarRepository(SQLiteAsyncConnection connection)
			: base(connection)
		{

		}

		public async Task<string> GetCalendarNotes (DateTime date)
		{
			var all = await AllAsync ();
			var calendar = all.FirstOrDefault (x => x.Date == date);
			if (calendar == null)
			{
				return string.Empty;
			}
			return calendar.Notes;
		}
	}
}
