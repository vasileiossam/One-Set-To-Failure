using System;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;
using Set.Models;
using System.Linq;

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

        private static bool CanCalculateTarget(Workout workout)
        {
            var workoutCount = workout.Exercise.RepsIncrement.WorkoutCount; 

            // target is calculated in every workout
            if (workoutCount == 0) return true;

            // target is calculated in every Nth workout

            // how many workouts for this exercise?
			var count = App.Database.WorkoutsRepository.All.Where(x => x.ExerciseId == workout.ExerciseId).Count();
            if (workout.WorkoutId == 0) count++;

            return IsDivisible(count, workoutCount);
        }


        public static object GetTargetWorkout(Workout workout)
        {
            int targetReps = 0;
            double targetWeight = 0;
			var startingReps = (int) workout.Exercise.StartingReps; 

            if (workout.PreviousWorkout == null)
            {
				targetReps = startingReps;
                targetWeight = 0;
            }
            else
            if (CanCalculateTarget(workout))
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
                    if (workout.PreviousWorkout.Reps < workout.Exercise.RepsForWeightDn)
                    {
						targetReps = startingReps;
                        targetWeight = GetPreviousWeight(workout.Exercise.PlateWeight, workout.PreviousWorkout.Weight);
                    }
                    // advance to next Weight
                    else if (targetReps > workout.Exercise.RepsForWeightUp)
                    {
                        targetReps = startingReps;
                        targetWeight = GetNextWeight(workout.Exercise.PlateWeight, workout.PreviousWorkout.Weight);
                    }
                    // stay in same weight but increase Reps
                    else
                    {
                        targetReps = workout.PreviousWorkout.Reps + workout.Exercise.RepsIncrement.Increment;
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
    }
}

