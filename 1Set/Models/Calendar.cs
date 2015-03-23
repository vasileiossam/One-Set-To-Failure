using System;
using SQLite.Net.Attributes;
using System.Collections.Generic;
using SQLiteNetExtensions.Attributes;
using System.Linq;
using AutoMapper;

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

