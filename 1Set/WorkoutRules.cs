using System;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Set.Models;
using System.Linq;
using System.Threading.Tasks;

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

        private static async Task<bool> CanCalculateTarget(Workout workout)
        {
			// TODO when I'll implement the settings in the exercise level I have to replace the WorkoutCount with workout.Exercise.RepsIncrement.WorkoutCount 
			var workoutCount = App.Settings.RepsIncrement.WorkoutCount; 

            // target is calculated in every workout
            if (workoutCount == 0) return true;

            // target is calculated in every Nth workout

            // how many workouts for this exercise?
			var list = await App.Database.WorkoutsRepository.AllAsync();
			var collection = new ObservableCollection<Workout> (list);
			var count = collection.Where(x => x.ExerciseId == workout.ExerciseId).Count();
            if (workout.WorkoutId == 0) count++;

            return IsDivisible(count, workoutCount);
        }


        public static async Task<object> GetTargetWorkout(Workout workout)
        {
            int targetReps = 0;
            double targetWeight = 0;

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
            if (await CanCalculateTarget(workout))
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
						else if (targetReps > maxReps)
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
			// we need at least one previous workout to start collecting trophies
			if (workout.PreviousReps == 0)
			{
				return 0;
			}

			// still on same weight
			if (Math.Abs (workout.TargetWeight - workout.Weight) < Units.WeightTolerance)
			{
				return workout.Reps - workout.TargetReps;
			}
			else
				// down to less weight
				if (workout.TargetWeight > workout.Weight)
				{
					return -10;
				} 
				else 
					// up to more weight
					if (workout.TargetWeight < workout.Weight)
					{
						return +10;
					}

			return 0;
		}
    }
}

