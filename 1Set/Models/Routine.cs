using System;
using SQLite.Net.Attributes;

namespace Set.Models
{
	public class Routine
	{
		public Routine ()
		{
		}


		[PrimaryKey, AutoIncrement]
		public int RoutineID { get; set; }
        
        public int ExerciseID { get; set; }        
        public int DayOfWeek { get; set; }
        public int RowNumber { get; set; }
	}
}

