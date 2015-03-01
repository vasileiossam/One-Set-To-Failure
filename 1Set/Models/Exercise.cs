using System;
using SQLite.Net.Attributes;
using System.Collections.Generic;
using SQLiteNetExtensions.Attributes;

namespace Set.Models
{
	[Table("Exercises")]
	public class Exercise
	{
		public Exercise ()
		{

		}

		[PrimaryKey, AutoIncrement]
		public int ExerciseId { get; set; }
		
        public string Name { get; set; }
	    public string Notes { get; set; }
		public float PlateWeight {get; set; }

        public byte[] Image { get; set; }

		[OneToMany]
		public List<RoutineDay> RoutineDays {get; set;}
	}
}

