using System;
using Xamarin.Forms;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Xamarin.Forms;
using System.Threading.Tasks;

namespace Set.ViewModels
{

	public class ChartsViewModel : BaseViewModel
	{
		protected PlotModel _plotModel;
		public PlotModel PlotModel
		{
			get
			{
				return _plotModel;
			}
			set
			{
				if (_plotModel != value)
				{
					_plotModel = value;
					OnPropertyChanged("PlotModel");
				}
			}
		}

		public ChartsViewModel ()
		{
			
		}

		public async Task Load()
		{
			await Task.Run (() =>
			{
				PlotModel = new PlotModel {
					Title = "OxyPlot in Xamarin.Forms",
					Subtitle = string.Format ("OS: {0}, Idiom: {1}", Device.OS, Device.Idiom),
					Background = OxyColors.LightYellow,
					PlotAreaBackground = OxyColors.LightGray
				};
				var categoryAxis = new CategoryAxis { Position = AxisPosition.Bottom };
				var valueAxis = new LinearAxis { Position = AxisPosition.Left, MinimumPadding = 0 };
				PlotModel.Axes.Add (categoryAxis);
				PlotModel.Axes.Add (valueAxis);
				var series = new ColumnSeries ();
				series.Items.Add (new ColumnItem { Value = 3 });
				series.Items.Add (new ColumnItem { Value = 14 });
				series.Items.Add (new ColumnItem { Value = 11 });
				series.Items.Add (new ColumnItem { Value = 12 });
				series.Items.Add (new ColumnItem { Value = 7 });
				PlotModel.Series.Add (series);
			});
		}
	}
}

