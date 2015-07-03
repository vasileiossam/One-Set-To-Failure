using System;
using Xamarin.Forms;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Xamarin.Forms;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Set.Models;
using Set.Localization;

namespace Set.ViewModels
{
	public class ChartsViewModel : BaseViewModel
	{
		public StackLayout OxyPlotsLayout { get; set; }

		private List<Exercise> _exercises;
		private List<Workout> _workouts;

		private bool _noChartsDataVisible;
		public bool NoChartsDataVisible 
		{ 
			get
			{
				return _noChartsDataVisible;
			}
			set
			{
				if (_noChartsDataVisible != value)
				{
					_noChartsDataVisible = value;
					OnPropertyChanged("NoChartsDataVisible");
				}
			}
		}

		private bool _chartsPinchToZoomVisible;
		public bool ChartsPinchToZoomVisible 
		{ 
			get
			{
				return _chartsPinchToZoomVisible;
			}
			set
			{
				if (_chartsPinchToZoomVisible != value)
				{
					_chartsPinchToZoomVisible = value;
					OnPropertyChanged("ChartsPinchToZoomVisible");
				}
			}
		}

		public ChartsViewModel ()
		{
			NoChartsDataVisible = false;
			ChartsPinchToZoomVisible = false;			
		}

		public async Task Load()
		{
			_workouts = await App.Database.WorkoutsRepository.AllAsync ();
			_exercises = await App.Database.ExercisesRepository.AllAsync ();

			NoChartsDataVisible = _workouts.Count == 0;
			ChartsPinchToZoomVisible = !NoChartsDataVisible;

			foreach (var workout in _workouts)
			{
				var exercise = _exercises.FirstOrDefault (x => x.ExerciseId == workout.ExerciseId);
				workout.Exercise = exercise;
			}
		}

		public async Task SelectChart(int selectedChartIndex)
		{
			switch(selectedChartIndex)
			{
				case 0:
				BuildWeightPerWorkout ();
				break;

				case 1:
				BuildRepsPerWorkout ();
				break;
			}
		}

		public PlotModel GetWeightPerWorkoutModel(KeyValuePair<Exercise, List<DataPoint>> item)
		{
			//await Task.Run (() =>
			//{
				// create plot model
				var plotModel = new PlotModel {
					Title = item.Key.Name,
					Background = OxyColors.LightYellow,
					PlotAreaBackground = OxyColors.LightGray,
				};

				var series = new LineSeries ();
				series.ItemsSource = item.Value;
				plotModel.Series.Add (series);

				// setup axis
				var dateAxis = new DateTimeAxis { Position = AxisPosition.Bottom };
				dateAxis.StringFormat = "MMM dd";

				var valueAxis = new LinearAxis { Position = AxisPosition.Left, MinimumPadding = 0 };
				valueAxis.Title = "Weight";

				plotModel.Axes.Add (dateAxis);
				plotModel.Axes.Add (valueAxis);

				return plotModel; 
			//});
		}

		private void BuildWeightPerWorkout ()
		{
			var data = new Dictionary<Exercise, List<DataPoint>> ();

			// prepare data
			foreach (var workout in _workouts)
			{
				var exercise = _exercises.FirstOrDefault (x => x.ExerciseId == workout.ExerciseId);
				if (exercise == null)
					continue;

				var defaultValue = default(KeyValuePair<Exercise, List<DataPoint>>);
				var item = data.FirstOrDefault (x => x.Key == exercise);
				if (item.Equals(defaultValue))
				{
					data.Add(exercise, new List<DataPoint> ());
					item = data.FirstOrDefault (x => x.Key == exercise);
				} 

				item.Value.Add(
					new DataPoint (DateTimeAxis.ToDouble (workout.Created), WeightMetricToImperialConverter.GetWeightAsDouble(workout.Weight))
				);
			}

			// create charts
			foreach(var item in data)
			{
				var plotModel = GetWeightPerWorkoutModel (item);
				var plotView = new PlotView ()
				{
					HeightRequest = 300,
					Model = plotModel,
					VerticalOptions = LayoutOptions.Fill,
					HorizontalOptions = LayoutOptions.Fill
				};

				OxyPlotsLayout.Children.Add(plotView);		
			}			
		}

		private void BuildRepsPerWorkout ()
		{
			var data = 
				from w in _workouts
				group w by w.ExerciseId into workoutsPerExercise
				from nestedGroup in
					(
						from a in workoutsPerExercise
						group a by a.Weight
					)
				group nestedGroup by workoutsPerExercise.Key;

			foreach (var exerciseGroup in data)
			{
				var exercise = _exercises.FirstOrDefault (x => x.ExerciseId == exerciseGroup.Key);
				var plotModel = new PlotModel {
					Title = exercise.Name,
					Background = OxyColors.LightYellow,
					PlotAreaBackground = OxyColors.LightGray,
				};

				foreach (var weightGroup in exerciseGroup)
				{
					var series = new LineSeries ();
					series.Title = string.Format("{0} {1}", WeightMetricToImperialConverter.GetWeightAsDouble(weightGroup.Key), L10n.GetWeightUnit());
					plotModel.Series.Add (series);

					var weightData =  new List<DataPoint>();
					foreach (var workout in weightGroup)
					{
						weightData.Add(new DataPoint (DateTimeAxis.ToDouble (workout.Created), workout.Reps));						
					}

					series.ItemsSource = weightData;
				}

				var dateAxis = new DateTimeAxis { Position = AxisPosition.Bottom };
				dateAxis.StringFormat = "MMM dd";
				var valueAxis = new LinearAxis { Position = AxisPosition.Left, MinimumPadding = 0 };
				valueAxis.Title = "Resps";

				plotModel.Axes.Add (dateAxis);
				plotModel.Axes.Add (valueAxis);		

				var plotView = new PlotView ()
				{
					HeightRequest = 300,
					Model = plotModel,
					VerticalOptions = LayoutOptions.Fill,
					HorizontalOptions = LayoutOptions.Fill
				};

				OxyPlotsLayout.Children.Add(plotView);		
			}
		}
	}
}

