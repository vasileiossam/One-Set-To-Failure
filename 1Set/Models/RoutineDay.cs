using System;
using SQLite.Net.Attributes;

namespace Set.Models
{
	public class RoutineDay
	{
		public RoutineDay ()
		{
		}


		[PrimaryKey, AutoIncrement]
		public int RoutineDayId { get; set; }
        
        public int ExerciseId { get; set; }        
        public int DayOfWeek { get; set; }
        public int RowNumber { get; set; }
	}
}

