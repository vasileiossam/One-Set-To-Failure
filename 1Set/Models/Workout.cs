using System;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace Set.Models
{
	[Table("Workouts")]
	public class Workout
	{
		public Workout ()
		{
		}


		[PrimaryKey, AutoIncrement]
		public int WorkoutId { get; set; }

		[Indexed]
		[ForeignKey(typeof(Exercise))]
        public int ExerciseId { get; set; }

		public DateTime Created { get; set; }
        public string Notes { get; set; }

        public int Reps { get; set; }
        public double Weight { get; set; }

		[OneToOne]		      
		public Exercise Exercise { get; set; }
	}
}

