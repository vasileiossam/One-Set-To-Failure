using System;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace Set.Models
{
	[Table("RoutineDays")]
	public class RoutineDay
	{
		public RoutineDay ()
		{
		}

		[PrimaryKey, AutoIncrement]
		public int RoutineDayId { get; set; }

		[ForeignKey(typeof(Exercise))]
        public int ExerciseId { get; set; }        
	      
		public int DayOfWeek { get; set; }
        public int RowNumber { get; set; }
		public int IsActive {get; set;}

		[Ignore]
		public Exercise Exercise { get; set; }

		[Ignore]
		public Workout Workout {get; set;}


	}
}

