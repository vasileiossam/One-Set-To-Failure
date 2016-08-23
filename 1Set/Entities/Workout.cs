using System;
using SQLite;
 
namespace Set.Entities
{
	[Table("Workouts")]
	public class Workout
	{
		[PrimaryKey, AutoIncrement]
		public int WorkoutId { get; set; }
		[Indexed]
        public int ExerciseId { get; set; }
		public DateTime Created { get; set; }
        public string Notes { get; set; }
        public int Reps { get; set; }
        public double Weight { get; set; }
		public int Trophies { get; set; }
		public int PreviousReps { get; set; }
		public double PreviousWeight { get; set; }
		public int TargetReps { get; set; }
		public double TargetWeight { get; set; }

        [Ignore]
        public Exercise Exercise { get; set; }

        public Workout()
        {
            Created = DateTime.Today;
        }
    }
}