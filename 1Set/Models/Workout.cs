using System;
using SQLite.Net.Attributes;

namespace Set.Models
{
	public class Workout
	{
		public Workout ()
		{
		}


		[PrimaryKey, AutoIncrement]
		public int WorkoutID { get; set; }

        public int ExerciseID { get; set; }

		public DateTime StartDate { get; set; }
        
        public int Reps { get; set; }
        public float Weight { get; set; }
		
        public string Notes { get; set; }

	}
}

