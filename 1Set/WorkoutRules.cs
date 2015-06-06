using System;
using System.Threading.Tasks;
using Set.Models;

namespace Set
{
    public class WorkoutRules
    {
        public WorkoutRules()
        {

        }


		private static bool IsDivisible(int x, int n)
        {
           return (x % n) == 0;
        }        

        private static double GetNextWeight(double plateWeight, double previousWorkoutWeight)
        {
            if ((previousWorkoutWeight > 0) && (plateWeight > 0)) 
            {
                return previousWorkoutWeight + plateWeight;
            }
            return 0;
        }
        
        private static double GetPreviousWeight(double plateWeight, double previousWorkoutWeight)
        {
            if ((previousWorkoutWeight > 0) && (plateWeight > 0)) 
            {
                var weight = previousWorkoutWeight - plateWeight;
                if (weight < 0) return 0;
                return weight;
            }
            return 0;
        }

        private static async Task<bool> CanCalculateTargetAsync(Workout workout)
        {
			// TODO when I'll implement the settings in the exercise level I have to replace the WorkoutCount with workout.Exercise.RepsIncrement.WorkoutCount 
			var workoutCount = App.Settings.RepsIncrement.WorkoutCount; 

            // target is calculated in every workout
            if (workoutCount == 0) return true;

            // target is calculated in every Nth workout

            // how many workouts for this exercise?
			var count = await App.Database.WorkoutsRepository.Table.Where(x => x.ExerciseId == workout.ExerciseId).CountAsync();
            if (workout.WorkoutId == 0) count++;

            return IsDivisible(count, workoutCount);
        }


        public static async Task<object> GetTargetWorkoutAsync(Workout workout)
        {
            int targetReps;
            double targetWeight;

			// TODO when I'll implement the settings in the exercise level I have to replace the MinReps with (int) workout.Exercise.MinReps; 
			var minReps = App.Settings.MinReps; 
			var maxReps = App.Settings.MaxReps;

			var startingReps = minReps;
				
            if (workout.PreviousWorkout == null)
            {
				targetReps = startingReps;
                targetWeight = 0;
            }
            else
            if (await CanCalculateTargetAsync(workout))
            {
                // no previous workout exist, this is the first workout for this exercise
                if (workout.PreviousWorkout == null)
                {
					targetReps = startingReps;
                    targetWeight = 0;
                }
                else
                {
                    // go back to previous Weight
					if (workout.PreviousWorkout.Reps < minReps)
                    {
						targetReps = startingReps;
                        targetWeight = GetPreviousWeight(workout.Exercise.PlateWeight, workout.PreviousWorkout.Weight);
                    }
                    // advance to next Weight
					else if (workout.PreviousWorkout.Reps >= maxReps)
                    {
                        targetReps = startingReps;
                        targetWeight = GetNextWeight(workout.Exercise.PlateWeight, workout.PreviousWorkout.Weight);
                    }
                    // stay in same weight but increase Reps
                    else
                    {
						// TODO when I'll implement the settings in the exercise level I have to replace the increment with workout.Exercise.RepsIncrement.Increment
						targetReps = workout.PreviousWorkout.Reps + App.Settings.RepsIncrement.Increment;
					    
                        targetWeight = workout.PreviousWorkout.Weight;
                    }
                }
            }
            else
            {
                // suggest previous workout Reps and Weight
                targetReps = workout.PreviousWorkout.Reps;
                targetWeight = workout.PreviousWorkout.Weight;
            }

            return new {TargetReps = targetReps, TargetWeight = targetWeight};
        }

		public static int GetTrophies(Workout workout)
		{
			// TODO when I'll implement the settings in the exercise level I have to replace the MinReps with (int) workout.Exercise.MinReps; 
			var minReps = App.Settings.MinReps;

		    // we need at least one previous workout to start collecting trophies
			if (workout.PreviousReps == 0)
			{
				return 0;
			}

			// still on same weight
			if (Math.Abs (workout.TargetWeight - workout.Weight) < Units.WeightTolerance)
			{
				// calc how many more reps since last time
				if (Math.Abs (workout.TargetWeight - workout.PreviousWeight) < Units.WeightTolerance)
				{
					return (workout.Reps - workout.TargetReps) + (workout.TargetReps - workout.PreviousReps);
				} else
				{
					return 0;
				}
			}
			else
				// down to less weight
				if (workout.TargetWeight > workout.Weight)
				{
					return -1;
				} 
				else 
					// level up
					if (workout.TargetWeight < workout.Weight)
					{
						return workout.Reps - minReps;
					}

			return 0;
		}
    }
}

