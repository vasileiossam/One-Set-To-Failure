using System;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace Set.Models
{
    public class WorkoutRules
    {
        public WorkoutRules()
        {

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

        private bool CanCalculateTarget(int workoutCount )
        {
            // target is calculated in every workout
            if (workoutCount == 0) return true;

            // target is calculated in every Nth workout
????    
        }


        public static object GetTargetWorkout(Workout workout)
        {
            int targetReps = 0;
            double targetWeight = 0;

            var repsIncrement = workout.Exercise.RepsIncrement.Increment; 
            var workoutCount = workout.Exercise.RepsIncrement.WorkoutCount; 
            var startingReps = workout.Exercise.StartingReps; 
            var repsForWeightUp = workout.Exercise.RepsForWeightUp; 
            var repsForWeightDn = workout.Exercise.RepsForWeightDn;

            if (workout.PreviousWorkout == null)
            {
                targetReps = startingReps;
                targetWeight = 0;
            }
            else
            if (CanCalculateTarget(workoutCount))
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
                    if (workout.PreviousWorkout.Rep < repsForWeightDn)
                    {
                        targetReps = startingReps;
                        targetWeight = GetPreviousWeight(workout.Exercise.PlateWeight, workout.PreviousWorkout.Weight);
                    }
                    // advance to next Weight
                    else if (targetReps > repsToAdvance)
                    {
                        targetReps = startingReps;
                        targetWeight = GetNextWeight(workout.Exercise.PlateWeight, workout.PreviousWorkout.Weight);
                    }
                    // stay in same weight but increase Reps
                    else
                    {
                        targetReps = workout.PreviousWorkout.Reps + repsIncrement;
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

            return new object () {TargetReps = targetReps, TargetWeight = targetWeight};
        }
    }
}

