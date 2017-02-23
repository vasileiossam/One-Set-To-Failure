using System;
using SQLite;

namespace OneSet.Models
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