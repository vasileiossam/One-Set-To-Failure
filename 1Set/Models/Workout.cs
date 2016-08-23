using System;
using System.Threading.Tasks;
using AutoMapper;
using SQLite;

namespace Set.Models1
{
	public class Workout
	{
		public Workout ()
		{
		}

		public int WorkoutId { get; set; }
        public int ExerciseId { get; set; }
		public DateTime Created { get; set; }
        public string Notes { get; set; }
        public int Reps { get; set; }
        public double Weight { get; set; }
		public int Trophies { get; set; }

        [Ignore]
		public Workout PreviousWorkout { get; set; }

		public int PreviousReps { get; set; }
		public double PreviousWeight { get; set; }
		public int TargetReps { get; set; }
		public double TargetWeight { get; set; }

 		[Ignore]
		public Exercise Exercise { get; set; }

		[Ignore]
		public string RepsImage {
			get
			{
				if (Reps == 0)
					return "ic_fa_circle";
				return "ic_fa_circle";
			}
		}

		[Ignore]
		public string WeightImage {
			get
			{
				if (Weight == 0)
					return "ic_fa_circle_o";
				return "ic_fa_circle";
			}
		}

		


	}
}

