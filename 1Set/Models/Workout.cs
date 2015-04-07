using System;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using Set;
using System.Threading.Tasks;
using Set.Concrete;
using Xamarin.Forms;
using Set.Abstract;

namespace Set.Models
{
	[Table("Workouts")]
	public class Workout
	{
		public Workout ()
		{
            Created = DateTime.Today;
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

		public async Task Load()
		{
			try
			{
				// update previous and target workout only when adding a new workout
				// if (WorkoutId == 0)
				{
					PreviousWorkout = await App.Database.WorkoutsRepository.GetPreviousWorkout (ExerciseId, Created);
					if (PreviousWorkout != null)
					{
						PreviousReps = PreviousWorkout.Reps;
						PreviousWeight = PreviousWorkout.Weight;
					}

					dynamic targetWorkout = await WorkoutRules.GetTargetWorkout (this);
					if (targetWorkout != null)
					{
						TargetReps = targetWorkout.TargetReps;
						TargetWeight = targetWorkout.TargetReps;
					}
				}
			}
			catch(Exception ex)
			{
				App.ShowErrorPage (this, ex);
			}
		}
	}
}

