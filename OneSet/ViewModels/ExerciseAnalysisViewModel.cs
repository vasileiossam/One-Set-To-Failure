using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using OneSet.Abstract;
using OneSet.Converters;
using OneSet.Localization;
using OneSet.Models;

namespace OneSet.ViewModels
{
	public class ExerciseStat
	{
		public string Title {get; set;}
		public string Value {get; set;}

		// TODO move to viewmodel or page when this works https://forums.xamarin.com/discussion/25677/does-xamarin-forms-support-relativesource-on-a-binding
		protected StackOrientation _cellLayoutOrientation;
		public StackOrientation CellLayoutOrientation
		{
			get
			{
				return _cellLayoutOrientation;
			}
			set
			{
				if (_cellLayoutOrientation != value)
				{
					_cellLayoutOrientation = value;
				}
			}
		}
	}

	public class ExerciseAnalysisViewModel : BaseViewModel, INavigationAware
    {
		public Picker ExercisesPicker { get; set; }
		public List<ExerciseStat> Stats {get; set;}
        
        #region private variables
        private List<Exercise> _exercises;
		private List<Workout> _workouts;
		private List<Exercise> _exercisesInWorkouts { get; set;}
		private WeightMetricToImperialConverter _weightConverter { get; set;}
        private readonly IWorkoutsRepository _workoutsRepository;
        private readonly IExercisesRepository _exercisesRepository;
        #endregion

        public ExerciseAnalysisViewModel(IWorkoutsRepository workoutsRepository, IExercisesRepository exercisesRepository)
        {
            _workoutsRepository = workoutsRepository;
            _exercisesRepository = exercisesRepository;
            _weightConverter = new WeightMetricToImperialConverter ();
		}

        #region private methods
        private string GetStarted(Exercise exercise)
		{
			var workout = _workouts.Where(x=>x.ExerciseId == exercise.ExerciseId).OrderBy (x => x.Created).FirstOrDefault ();
			if (workout == null) return string.Empty;
				
			return
			    $"{workout.Created:d}, {workout.Reps} Reps {WeightMetricToImperialConverter.GetWeight(workout.Weight)} {L10n.GetWeightUnit()}";
		}

		private string GetLastWorkout(Exercise exercise)
		{
			var workout = _workouts.Where(x=>x.ExerciseId == exercise.ExerciseId).OrderBy (x => x.Created).LastOrDefault ();
			if (workout == null) return string.Empty;

			return
			    $"{workout.Created:d}, {workout.Reps} Reps {WeightMetricToImperialConverter.GetWeight(workout.Weight)} {L10n.GetWeightUnit()}";
		}

		private string GetCurrentWeight(Exercise exercise)
		{
			var lastWorkout = _workouts.Where(x=>x.ExerciseId == exercise.ExerciseId).OrderBy (x => x.Created).LastOrDefault ();
			if (lastWorkout == null)
				return string.Empty;
			
			return $"{WeightMetricToImperialConverter.GetWeight(lastWorkout.Weight)} {L10n.GetWeightUnit()}";			
		}

		private string GetLastTargetWorkout(Exercise exercise)
		{
			var workout = _workouts.Where(x=>x.ExerciseId == exercise.ExerciseId).OrderBy (x => x.Created).LastOrDefault ();
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
			var weights = _workouts.Where(x=>x.ExerciseId == exercise.ExerciseId)
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
			var lastWorkout = _workouts.Where(x=>x.ExerciseId == exercise.ExerciseId).OrderBy (x => x.Created).LastOrDefault ();
			if (lastWorkout == null)
				return string.Empty;

			var currentWeight = lastWorkout.Weight;
			var workouts = _workouts.Where(x=>x.ExerciseId == exercise.ExerciseId).OrderByDescending(x => x.Created);
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
			var firstWorkout = _workouts.Where(x=>x.ExerciseId == exercise.ExerciseId).OrderBy (x => x.Created).FirstOrDefault ();
			return firstWorkout == null ? string.Empty : Math.Truncate((DateTime.Now - firstWorkout.Created).TotalDays).ToString();
		}

		private string GetDaysSinceLastWorkout(Exercise exercise) 
		{
			var lastWorkout = _workouts.Where(x=>x.ExerciseId == exercise.ExerciseId).OrderBy (x => x.Created).LastOrDefault ();
			return lastWorkout == null ? string.Empty : Math.Truncate((DateTime.Now - lastWorkout.Created).TotalDays).ToString();
		}

		private string GetTotalWorkouts(Exercise exercise) 
		{
			return _workouts.Count (x => x.ExerciseId == exercise.ExerciseId).ToString ();
		}

		private string GetTotalTrophies(Exercise exercise) 
		{
			return _workouts.Where(x => x.ExerciseId == exercise.ExerciseId).Sum(x=>x.Trophies).ToString ();
		}

		private string GetTrophiesWon(Exercise exercise) 
		{
			return _workouts.Where(x => x.ExerciseId == exercise.ExerciseId && x.Trophies > 0).Sum(x=>x.Trophies).ToString ();
		}

		private string GetTrophiesLost(Exercise exercise) 
		{
			return _workouts.Where(x => x.ExerciseId == exercise.ExerciseId && x.Trophies < 0).Sum(x=>x.Trophies).ToString ();
		}

		private string GetTotalReps(Exercise exercise) 
		{
			return _workouts.Where(x => x.ExerciseId == exercise.ExerciseId).Sum(x=>x.Reps).ToString ();
		}

		private string GetTotalWeight(Exercise exercise) 
		{
			var total = _workouts.Where(x => x.ExerciseId == exercise.ExerciseId).Sum(x=>x.Weight);
			return $"{WeightMetricToImperialConverter.GetWeight(total)} {L10n.GetWeightUnit()}";
		}
        #endregion

        #region INavigationAware
        public async Task OnNavigatedFrom(NavigationParameters parameters)
        {
            await Task.FromResult(0);
        }

        public async Task OnNavigatedTo(NavigationParameters parameters)
        {
            _workouts = await _workoutsRepository.AllAsync();
            _exercises = await _exercisesRepository.AllAsync();

            foreach (var workout in _workouts)
            {
                var exercise = _exercises.FirstOrDefault(x => x.ExerciseId == workout.ExerciseId);
                //	workout.Exercise = exercise;
            }

            //	_exercisesInWorkouts = _workouts.GroupBy (x => x.Exercise).Select (x => x.First ().Exercise).ToList ();
            //ExercisesPicker.Items.Clear();
            //foreach(var item in _exercisesInWorkouts)
            //{
            //	ExercisesPicker.Items.Add (item.Name);
            //}
        }
        #endregion

        public async Task<List<ExerciseStat>> GetStats(int exerciseIndex)
        {
            var exercise = _exercisesInWorkouts[exerciseIndex];

            var list = new List<ExerciseStat>
            {
                new ExerciseStat {Title = "Started", Value = GetStarted(exercise)},
                new ExerciseStat {Title = "Last workout", Value = GetLastWorkout(exercise)},
                new ExerciseStat {Title = "Last target workout", Value = GetLastTargetWorkout(exercise)},
                new ExerciseStat {Title = "Current Weight", Value = GetCurrentWeight(exercise)},
                new ExerciseStat {Title = "Successive workouts in current weight", Value = GetSuccesiveDays(exercise)},
                new ExerciseStat {Title = "Weight increases", Value = GetWeightIncreases(exercise)},
                new ExerciseStat {Title = "Days since started", Value = GetDaysSinceStarted(exercise)},
                new ExerciseStat {Title = "Days since last workout", Value = GetDaysSinceLastWorkout(exercise)},
                new ExerciseStat {Title = "Total workouts", Value = GetTotalWorkouts(exercise)},
                new ExerciseStat {Title = "Total trophies", Value = GetTotalTrophies(exercise)},
                new ExerciseStat {Title = "Trophies won", Value = GetTrophiesWon(exercise)},
                new ExerciseStat {Title = "Trophies lost", Value = GetTrophiesLost(exercise)},
                new ExerciseStat {Title = "Total Reps", Value = GetTotalReps(exercise)},
                new ExerciseStat {Title = "Total Weight", Value = GetTotalWeight(exercise)}
            };

            return list;
        }
    }
}

