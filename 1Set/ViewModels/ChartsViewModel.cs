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

namespace Set.ViewModels
{

	public class ChartsViewModel : BaseViewModel
	{
		public StackLayout OxyPlotsLayout { get; set; }

		public ChartsViewModel ()
		{
			
		}

		public async Task Load()
		{
			var workouts = await App.Database.WorkoutsRepository.AllAsync ();
			var exercises = await App.Database.ExercisesRepository.AllAsync ();
			var data = new Dictionary<Exercise, List<DataPoint>> ();

			// prepare data
			foreach (var workout in workouts)
			{
				var exercise = exercises.FirstOrDefault (x => x.ExerciseId == workout.ExerciseId);
				if (exercise == null)
					continue;

				var defaultValue = default(KeyValuePair<Exercise, List<DataPoint>>);
				var item = data.FirstOrDefault (x => x.Key == exercise);
				if (item.Equals(defaultValue))
				{
					data.Add(exercise, new List<DataPoint> ());
					item = data.FirstOrDefault (x => x.Key == exercise);
				} 

				item.Value.Add(new DataPoint (DateTimeAxis.ToDouble (workout.Created), workout.Weight));
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

		public PlotModel GetWeightPerWorkoutModel(KeyValuePair<Exercise, List<DataPoint>> item)
		{
			//await Task.Run (() =>
			//{
				// create plot model
				var plotModel = new PlotModel {
					Title = "Weight per workout",
					Background = OxyColors.LightYellow,
					PlotAreaBackground = OxyColors.LightGray,
				};

				var series = new LineSeries ();
				series.Title = item.Key.Name;
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

	}
}

