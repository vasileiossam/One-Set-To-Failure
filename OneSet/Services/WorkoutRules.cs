using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OneSet.Abstract;
using OneSet.Models;

namespace OneSet.Services
{
    public class WorkoutRules : IWorkoutRules
    {
        private readonly IUnitsService _units;
        private readonly IWorkoutsRepository _workoutsRepository;

        public WorkoutRules(IUnitsService units, IWorkoutsRepository workoutsRepository)
        {
            _units = units;
            _workoutsRepository = workoutsRepository;
        }

        private static bool IsDivisible(int x, int n)
        {
           return x % n == 0;
        }        

        private static double GetNextWeight(List<float> weightsDone, double plateWeight, double weightCurrent)
        {
            // go by next done weight
            var item = weightsDone.OrderBy(x => x).FirstOrDefault(x => weightCurrent > 0 && x > weightCurrent);
            if (item != 0)
            {
                return item;
            }
            
            // go up a plateWeight
            if (weightCurrent > 0 && plateWeight > 0) 
            {
                return weightCurrent + plateWeight;
            }

            return 0;
        }
        
        private static double GetPreviousWeight(List<float> weightsDone, double plateWeight, double weightCurrent)
        {
            // no data yet
            if (!(weightCurrent > 0) || !(plateWeight > 0)) return 0;

            // go by previous done weight
            var item = weightsDone.OrderByDescending(x => x).FirstOrDefault(x => weightCurrent > 0 && x < weightCurrent);
            if (item != 0)
            {
                return item;
            }
            
            // go down a plateWeight
            var weight = weightCurrent - plateWeight;
            return weight > 0 ? weight : weightCurrent;
        }

        private async Task<bool> CanCalculateTarget(int exerciseId)
        {
			var workoutCount = App.Settings.RepsIncrement.WorkoutCount; 

            // target is calculated in every workout
            if (workoutCount == 0) return true;

            // target is calculated in every Nth workout

            // how many workouts for this exercise?
            var count = await _workoutsRepository.GetWorkoutsCount(exerciseId);
            //if (workout == null) count++;

            return IsDivisible(count, workoutCount);
        }
        
        public async Task<KeyValuePair<int, double>> GetTargetWorkout(Exercise exercise, Workout previousWorkout)
        {
            int targetReps;
            double targetWeight;

			var minReps = App.Settings.MinReps; 
			var maxReps = App.Settings.MaxReps;

			var startingReps = minReps;
            var weightsDone = await _workoutsRepository.GetWeightsDone(exercise.ExerciseId);

            // no previous workout exist, this is the first workout for this exercise
            if (previousWorkout == null)
            {
				targetReps = startingReps;
                targetWeight = 0;
            }
            else
            if (await CanCalculateTarget(exercise.ExerciseId))
            {
                // go back to previous Weight
                if (previousWorkout.Reps < minReps)
                {
                    targetReps = startingReps;
                    targetWeight = GetPreviousWeight(weightsDone, exercise.PlateWeight, previousWorkout.Weight);
                }
                // advance to next Weight
                else if (previousWorkout.Reps >= maxReps)
                {
                    targetReps = startingReps;
                    targetWeight = GetNextWeight(weightsDone, exercise.PlateWeight, previousWorkout.Weight);
                }
                // stay in same weight but increase Reps
                else
                {
                    targetReps = previousWorkout.Reps + App.Settings.RepsIncrement.Increment;
                    targetWeight = previousWorkout.Weight;
                }
            }
            else
            {
                return new KeyValuePair<int, double>(previousWorkout.Reps, previousWorkout.Weight);
            }

            return new KeyValuePair<int, double>(targetReps, targetWeight);
        }

		public int GetTrophies(Workout workout)
		{
			var minReps = App.Settings.MinReps;

		    // we need at least one previous workout to start collecting trophies
			if (workout.PreviousReps == 0)
			{
				return 0;
			}

			// still on same weight
			if (Math.Abs (workout.TargetWeight - workout.Weight) < _units.WeightTolerance)
			{
			    // calc how many more reps since last time
				if (Math.Abs (workout.TargetWeight - workout.PreviousWeight) < _units.WeightTolerance)
				{
					return workout.Reps - workout.TargetReps + (workout.TargetReps - workout.PreviousReps);
				}
			    return 0;
			}

            // down to less weight
            if (workout.TargetWeight > workout.Weight)
            {
                return -1;
            }

            // level up
            if (workout.TargetWeight < workout.Weight)
            {
                return workout.Reps - minReps;
            }

            return 0;
		}
    }
}

