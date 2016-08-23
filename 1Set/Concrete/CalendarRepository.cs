using System;
using System.Threading.Tasks;
using Set.Abstract;
using Set.Entities;
using SQLite;

namespace Set.Concrete
{
	public class CalendarRepository : BaseRepository<Calendar>, ICalendarRepository
	{
		public CalendarRepository(SQLiteAsyncConnection connection)
			: base(connection)
		{

		}

		public async Task<Calendar> FindAsync (DateTime date) 
		{
			using (await Mutex.LockAsync ().ConfigureAwait (false)) 
			{
				var entity = await _connection.Table<Calendar> ()
					.Where (x => x.Date == date)
					.FirstOrDefaultAsync ();

				return entity;
			}
		}

		public async Task<string> GetCalendarNotes (DateTime date)
		{
			var calendar = await FindAsync (date);
			if (calendar == null)
			{
				return string.Empty;
			}
			return calendar.Notes;
		}
	}
}
