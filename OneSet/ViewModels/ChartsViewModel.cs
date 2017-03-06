using Xamarin.Forms;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Xamarin.Forms;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using OneSet.Abstract;
using OneSet.Converters;
using OneSet.Localization;
using OneSet.Models;

namespace OneSet.ViewModels
{
	public class ChartsViewModel : BaseViewModel
    {
        #region properties
		private bool _noChartsDataVisible;
		public bool NoChartsDataVisible
        {
            get { return _noChartsDataVisible; }
            set { SetProperty(ref _noChartsDataVisible, value); }
        }

        private bool _chartsPinchToZoomVisible;
		public bool ChartsPinchToZoomVisible
        {
            get { return _chartsPinchToZoomVisible; }
            set { SetProperty(ref _chartsPinchToZoomVisible, value); }
        }

        private int _pickerSelectedIndex;
        public int PickerSelectedIndex
        {
            get { return _pickerSelectedIndex; }
            set
            {
                SetProperty(ref _pickerSelectedIndex, value);

                if (_pickerSelectedIndex == 0)
                {
                    BuildWeightPerWorkout();
                }
                if (_pickerSelectedIndex == 1)
                {
                    BuildRepsPerWorkout();
                }
            }
        }

        public List<Exercise> Exercises { get; set; }
        public List<Workout> Workouts { get; set; }

        #endregion

        private readonly IMessagingService _messagingService;

        public ChartsViewModel (IMessagingService messagingService)
        {
            _messagingService = messagingService;

            NoChartsDataVisible = false;
			ChartsPinchToZoomVisible = false;
            PickerSelectedIndex = -1;
        }

        #region private methods
        private DateTimeAxis GetDateTimeAxis(int count)
		{
		    var dateAxis = new DateTimeAxis
		    {
		        Position = AxisPosition.Bottom,
		        StringFormat = "MMM dd",
		        IsPanEnabled = true,
		        IsZoomEnabled = true,
		        IntervalType = DateTimeIntervalType.Days,
		        MinorIntervalType = DateTimeIntervalType.Days,
		        MinorStep = 1
		    };
            
		    // when one workout it display a black mark in the begining of the axis; like minor and major to print in the same area
			if (count == 1)
			{
				dateAxis.IsAxisVisible = false;
			}
			else
			// in the begining when there aren't many workouts, this will avoid displaying the same date more than once in the axis
			if (count <= 7)
			{
				dateAxis.MajorStep = 1;
			}

			return dateAxis;
		}

        private PlotModel GetWeightPerWorkoutModel(KeyValuePair<Exercise, List<DataPoint>> item)
        {
            var plotModel = new PlotModel
            {
                Title = item.Key.Name,
                Background = OxyColors.LightYellow,
                PlotAreaBackground = OxyColors.LightGray,
            };

            var series = new LineSeries { ItemsSource = item.Value };
            plotModel.Series.Add(series);

            var dateAxis = GetDateTimeAxis(item.Value.Count);
            plotModel.Axes.Add(dateAxis);

            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                MinimumPadding = 0,
                Title = "Weight"
            };
            plotModel.Axes.Add(valueAxis);

            return plotModel;
        }
        
        private void BuildWeightPerWorkout ()
		{
			var data = new Dictionary<Exercise, List<DataPoint>> ();

			// prepare data
			foreach (var workout in Workouts)
			{
				var exercise = Exercises.FirstOrDefault (x => x.ExerciseId == workout.ExerciseId);
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
            var list = new List<PlotView>();
            foreach (var item in data)
			{
				var plotModel = GetWeightPerWorkoutModel (item);
				var plotView = new PlotView
				{
					HeightRequest = 300,
					Model = plotModel,
					VerticalOptions = LayoutOptions.Fill,
					HorizontalOptions = LayoutOptions.Fill
				};
			    list.Add(plotView);
			}

            _messagingService.Send(this, Messages.ChartBuilt, list);
        }

        private void BuildRepsPerWorkout ()
		{
			var data = 
				from w in Workouts
				group w by w.ExerciseId into workoutsPerExercise
				from nestedGroup in
					(
						from a in workoutsPerExercise
						group a by a.Weight
					)
				group nestedGroup by workoutsPerExercise.Key;

            var list = new List<PlotView>();
            foreach (var exerciseGroup in data)
			{
				var exercise = Exercises.FirstOrDefault (x => x.ExerciseId == exerciseGroup.Key);
				var plotModel = new PlotModel {
					Title = exercise.Name,
					Background = OxyColors.LightYellow,
					PlotAreaBackground = OxyColors.LightGray,
				};

				var count = 0;
				foreach (var weightGroup in exerciseGroup)
				{
				    var series = new LineSeries
				    {
				        Title = $"{WeightMetricToImperialConverter.GetWeightAsDouble(weightGroup.Key)} {L10n.GetWeightUnit()}"
				    };
				    plotModel.Series.Add (series);

					var weightData =  new List<DataPoint>();
					foreach (var workout in weightGroup)
					{
						weightData.Add(new DataPoint (DateTimeAxis.ToDouble (workout.Created), workout.Reps));						
						count++;
					}

					series.ItemsSource = weightData;
				}

				var dateAxis = GetDateTimeAxis (count);
				plotModel.Axes.Add (dateAxis);

			    var valueAxis = new LinearAxis
			    {
			        Position = AxisPosition.Left,
			        MinimumPadding = 0,
			        Title = "Resps"
			    };
			    plotModel.Axes.Add (valueAxis);		

				var plotView = new PlotView
				{
					HeightRequest = 300,
					Model = plotModel,
					VerticalOptions = LayoutOptions.Fill,
					HorizontalOptions = LayoutOptions.Fill
						
				};

                list.Add(plotView); 
            }

            _messagingService.Send(this, Messages.ChartBuilt, list);
        }
        #endregion

        public void Load()
        {
            NoChartsDataVisible = Workouts.Count == 0;
            ChartsPinchToZoomVisible = !NoChartsDataVisible;

            PickerSelectedIndex = 0;
        }
    }
}

