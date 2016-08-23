using System;
using SQLite;

namespace Set.Entities
{
	[Table("Calendar")]
	public class Calendar
	{
		[PrimaryKey, AutoIncrement]
		public int CalendarId { get; set; }
		public DateTime Date { get; set; }
		public string Notes { get; set; }
    }
}