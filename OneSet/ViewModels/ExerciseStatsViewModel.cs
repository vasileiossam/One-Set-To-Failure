using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OneSet.Converters;
using OneSet.Localization;
using OneSet.Models;

namespace OneSet.ViewModels
{
	public class ExerciseStatsViewModel : BaseViewModel
    {
        #region properties

        private ObservableCollection<ExerciseAnalysisItem> _stats;
        public ObservableCollection<ExerciseAnalysisItem> Stats
        {
            get { return _stats; }
            set { SetProperty(ref _stats, value); }
        }

        private int _pickerSelectedIndex;
        public int PickerSelectedIndex
        {
            get { return _pickerSelectedIndex; }
            set
            {
                SetProperty(ref _pickerSelectedIndex, value);
                if (_pickerSelectedIndex > -1)
                {
                    Stats = GetItems(_pickerSelectedIndex);
                }
            }
        }

        public List<Exercise> Exercises { get; set; }
        public List<Workout> Workouts { get; set; }
        public List<Exercise> ExercisesInWorkouts { get; set; }
        #endregion

        private WeightMetricToImperialConverter _weightConverter { get; set;}

        public ExerciseStatsViewModel()
        {
            _weightConverter = new WeightMetricToImperialConverter ();
		}

        #region private methods
        private ObservableCollection<ExerciseAnalysisItem> GetItems(int exerciseIndex)
        {
            var exercise = ExercisesInWorkouts[exerciseIndex];

            var list = new ObservableCollection<ExerciseAnalysisItem>
            {
                new ExerciseAnalysisItem {Title = "Started", Value = GetStarted(exercise)},
                new ExerciseAnalysisItem {Title = "Last workout", Value = GetLastWorkout(exercise)},
                new ExerciseAnalysisItem {Title = "Last target workout", Value = GetLastTargetWorkout(exercise)},
                new ExerciseAnalysisItem {Title = "Current Weight", Value = GetCurrentWeight(exercise)},
                new ExerciseAnalysisItem {Title = "Successive workouts in current weight", Value = GetSuccesiveDays(exercise)},
                new ExerciseAnalysisItem {Title = "Weight increases", Value = GetWeightIncreases(exercise)},
                new ExerciseAnalysisItem {Title = "Days since started", Value = GetDaysSinceStarted(exercise)},
                new ExerciseAnalysisItem {Title = "Days since last workout", Value = GetDaysSinceLastWorkout(exercise)},
                new ExerciseAnalysisItem {Title = "Total workouts", Value = GetTotalWorkouts(exercise)},
                new ExerciseAnalysisItem {Title = "Total trophies", Value = GetTotalTrophies(exercise)},
                new ExerciseAnalysisItem {Title = "Trophies won", Value = GetTrophiesWon(exercise)},
                new ExerciseAnalysisItem {Title = "Trophies lost", Value = GetTrophiesLost(exercise)},
                new ExerciseAnalysisItem {Title = "Total Reps", Value = GetTotalReps(exercise)},
                new ExerciseAnalysisItem {Title = "Total Weight", Value = GetTotalWeight(exercise)}
            };

            return list;
        }
        
        private string GetStarted(Exercise exercise)
		{
			var workout = Workouts.Where(x=>x.ExerciseId == exercise.ExerciseId).OrderBy (x => x.Created).FirstOrDefault ();
			if (workout == null) return string.Empty;
				
			return
			    $"{workout.Created:d}, {workout.Reps} Reps {WeightMetricToImperialConverter.GetWeight(workout.Weight)} {L10n.GetWeightUnit()}";
		}

		private string GetLastWorkout(Exercise exercise)
		{
			var workout = Workouts.Where(x=>x.ExerciseId == exercise.ExerciseId).OrderBy (x => x.Created).LastOrDefault ();
			if (workout == null) return string.Empty;

			return
			    $"{workout.Created:d}, {workout.Reps} Reps {WeightMetricToImperialConverter.GetWeight(workout.Weight)} {L10n.GetWeightUnit()}";
		}

		private string GetCurrentWeight(Exercise exercise)
		{
			var lastWorkout = Workouts.Where(x=>x.ExerciseId == exercise.ExerciseId).OrderBy (x => x.Created).LastOrDefault ();
			if (lastWorkout == null)
				return string.Empty;
			
			return $"{WeightMetricToImperialConverter.GetWeight(lastWorkout.Weight)} {L10n.GetWeightUnit()}";			
		}

		private string GetLastTargetWorkout(Exercise exercise)
		{
			var workout = Workouts.Where(x=>x.ExerciseId == exercise.ExerciseId).OrderBy (x => x.Created).LastOrDefault ();
			if (workout == null) return string.Empty;

			if ((workout.TargetReps > 0) && (workout.TargetWeight > 0))
			{
				return
				    $"{workout.TargetReps} Reps {WeightMetricToImperialConverter.GetWeight(workout.TargetWeight)} {L10n.GetWeightUnit()}";
			}
			return string.Empty;
		}

		private string GetWeightIncreases(Exercise exercise)
		{
			var weights = Workouts.Where(x=>x.ExerciseId == exercise.ExerciseId)
				.GroupBy (x => x.Weight).Select (x => x.First ().Weight).ToList ();

			weights.Sort();

			if (weights.Count == 0)
			{
				return "0";
			}

			var convertedWeights = new List<string> ();
			foreach (var weight in weights)
			{
				convertedWeights.Add($"{WeightMetricToImperialConverter.GetWeight(weight)} {L10n.GetWeightUnit()}");
			}
			return $"{weights.Count} ({string.Join(", ", convertedWeights)})";
		}

		private string GetSuccesiveDays(Exercise exercise)
		{
			var lastWorkout = Workouts.Where(x=>x.ExerciseId == exercise.ExerciseId).OrderBy (x => x.Created).LastOrDefault ();
			if (lastWorkout == null)
				return string.Empty;

			var currentWeight = lastWorkout.Weight;
			var workouts = Workouts.Where(x=>x.ExerciseId == exercise.ExerciseId).OrderByDescending(x => x.Created);
			var count = 0;
			foreach(var workout in workouts)
			{
				count++;
				if (workout.Weight != currentWeight)
					break;
			}
			return count.ToString ();
		}

		private string GetDaysSinceStarted(Exercise exercise) 
		{
			var firstWorkout = Workouts.Where(x=>x.ExerciseId == exercise.ExerciseId).OrderBy (x => x.Created).FirstOrDefault ();
			return firstWorkout == null ? string.Empty : Math.Truncate((DateTime.Now - firstWorkout.Created).TotalDays).ToString();
		}

		private string GetDaysSinceLastWorkout(Exercise exercise) 
		{
			var lastWorkout = Workouts.Where(x=>x.ExerciseId == exercise.ExerciseId).OrderBy (x => x.Created).LastOrDefault ();
			return lastWorkout == null ? string.Empty : Math.Truncate((DateTime.Now - lastWorkout.Created).TotalDays).ToString();
		}

		private string GetTotalWorkouts(Exercise exercise) 
		{
			return Workouts.Count (x => x.ExerciseId == exercise.ExerciseId).ToString ();
		}

		private string GetTotalTrophies(Exercise exercise) 
		{
			return Workouts.Where(x => x.ExerciseId == exercise.ExerciseId).Sum(x=>x.Trophies).ToString ();
		}

		private string GetTrophiesWon(Exercise exercise) 
		{
			return Workouts.Where(x => x.ExerciseId == exercise.ExerciseId && x.Trophies > 0).Sum(x=>x.Trophies).ToString ();
		}

		private string GetTrophiesLost(Exercise exercise) 
		{
			return Workouts.Where(x => x.ExerciseId == exercise.ExerciseId && x.Trophies < 0).Sum(x=>x.Trophies).ToString ();
		}

		private string GetTotalReps(Exercise exercise) 
		{
			return Workouts.Where(x => x.ExerciseId == exercise.ExerciseId).Sum(x=>x.Reps).ToString ();
		}

		private string GetTotalWeight(Exercise exercise) 
		{
			var total = Workouts.Where(x => x.ExerciseId == exercise.ExerciseId).Sum(x=>x.Weight);
			return $"{WeightMetricToImperialConverter.GetWeight(total)} {L10n.GetWeightUnit()}";
		}
        #endregion

        public void Load()
        {
            PickerSelectedIndex = 0;
        }
    }
}

