using System;
using System.Threading.Tasks;
using OneSet.Abstract;
using OneSet.Models;

namespace OneSet.Services
{
    public class WorkoutRules : IWorkoutRules
    {
        private readonly IUnitsService _units;
        private readonly IWorkoutsRepository _workoutsRepository;
        private readonly IExercisesRepository _exercisesRepository;

        public WorkoutRules(IUnitsService units, IWorkoutsRepository workoutsRepository, IExercisesRepository exercisesRepository)
        {
            _units = units;
            _workoutsRepository = workoutsRepository;
            _exercisesRepository = exercisesRepository;
        }

        private static bool IsDivisible(int x, int n)
        {
           return x % n == 0;
        }        

        private static double GetNextWeight(double plateWeight, double previousWorkoutWeight)
        {
            if (previousWorkoutWeight > 0 && plateWeight > 0) 
            {
                return previousWorkoutWeight + plateWeight;
            }
            return 0;
        }
        
        private static double GetPreviousWeight(double plateWeight, double previousWorkoutWeight)
        {
            if (!(previousWorkoutWeight > 0) || !(plateWeight > 0)) return 0;
            var weight = previousWorkoutWeight - plateWeight;
            return weight < 0 ? 0 : weight;
        }

        private async Task<bool> CanCalculateTargetAsync(Workout workout)
        {
			// TODO when I'll implement the settings in the exercise level I have to replace the WorkoutCount with workout.Exercise.RepsIncrement.WorkoutCount 
			var workoutCount = App.Settings.RepsIncrement.WorkoutCount; 

            // target is calculated in every workout
            if (workoutCount == 0) return true;

            // target is calculated in every Nth workout

            // how many workouts for this exercise?
            var count = await _workoutsRepository.GetWorkoutsCount(workout.ExerciseId);
            if (workout.WorkoutId == 0) count++;

            return IsDivisible(count, workoutCount);
        }
        
        public async Task<object> GetTargetWorkoutAsync(Workout workout)
        {
            var previousWorkout = await _workoutsRepository.GetPreviousWorkout(workout);
            var exercise = await _exercisesRepository.FindAsync(workout.ExerciseId);

            int targetReps;
            double targetWeight;

			// TODO when I'll implement the settings in the exercise level I have to replace the MinReps with (int) workout.Exercise.MinReps; 
			var minReps = App.Settings.MinReps; 
			var maxReps = App.Settings.MaxReps;

			var startingReps = minReps;
				
            if (previousWorkout == null)
            {
				targetReps = startingReps;
                targetWeight = 0;
            }
            else
            if (await CanCalculateTargetAsync(workout))
            {
                // no previous workout exist, this is the first workout for this exercise
                if (previousWorkout == null)
                {
					targetReps = startingReps;
                    targetWeight = 0;
                }
                else
                {
                    // go back to previous Weight
					if (previousWorkout.Reps < minReps)
                    {
						targetReps = startingReps;
                        targetWeight = GetPreviousWeight(exercise.PlateWeight, previousWorkout.Weight);
                    }
                    // advance to next Weight
					else if (previousWorkout.Reps >= maxReps)
                    {
                        targetReps = startingReps;
                        targetWeight = GetNextWeight(exercise.PlateWeight, previousWorkout.Weight);
                    }
                    // stay in same weight but increase Reps
                    else
                    {
						// TODO when I'll implement the settings in the exercise level I have to replace the increment with workout.Exercise.RepsIncrement.Increment
						targetReps = previousWorkout.Reps + App.Settings.RepsIncrement.Increment;
					    
                        targetWeight = previousWorkout.Weight;
                    }
                }
            }
            else
            {
                // suggest previous workout Reps and Weight
                targetReps = previousWorkout.Reps;
                targetWeight = previousWorkout.Weight;
            }

            return new {TargetReps = targetReps, TargetWeight = targetWeight};
        }

		public int GetTrophies(Workout workout)
		{
			// TODO when I'll implement the settings in the exercise level I have to replace the MinReps with (int) workout.Exercise.MinReps; 
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

