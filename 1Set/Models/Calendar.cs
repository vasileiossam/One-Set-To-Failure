using System;
using SQLite.Net.Attributes;

namespace Set.Models
{
	[Table("Calendar")]
	public class Calendar
	{
		[PrimaryKey, AutoIncrement]
		public int CalendarId { get; set; }
		public DateTime Date { get; set; }
		public string Notes { get; set; }

		public Calendar ()
		{
		}
	}
}

