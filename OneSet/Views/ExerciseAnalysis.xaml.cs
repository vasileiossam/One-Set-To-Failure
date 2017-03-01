using System;
using Autofac;
using OneSet.Abstract;
using OneSet.ViewModels;
using Xamarin.Forms;

namespace OneSet.Views
{
    public partial class ExerciseAnalysis : ExerciseAnalysisXaml, IScreenSizeHandler
	{
		private ScreenSizeHandler _screenSizeHandler;
		private StackOrientation _stackOrientation;

		public ExerciseAnalysis ()
		{
			InitializeComponent ();
			InitScreenSizeHandler ();
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing ();
			ViewModel.ExercisesPicker = ExercisesPicker;

            BindingContext = ViewModel;
			ExercisesPicker.SelectedIndex = 0;
		}

		private async void OnSelectedIndexChanged(object sender, EventArgs eventArgs)
		{
		    var picker = sender as Picker;
		    if (picker == null) return;
		    var selectedIndex = picker.SelectedIndex;
		    if (selectedIndex <= -1) return;
		    ViewModel.Stats = await ViewModel.GetStats(selectedIndex);
		    ChangeOrientation ();
		}

		public void Refresh()
		{
			StatsList.ItemsSource = null;
			StatsList.ItemsSource = ViewModel.Stats;
		}

		#region IScreenSizeHandler

		public void InitScreenSizeHandler()
		{
			_screenSizeHandler = new ScreenSizeHandler ();

			_stackOrientation = StackOrientation.Horizontal;
			if (_screenSizeHandler.GetStartingOrientation () == Orientations.Portrait 
				&& _screenSizeHandler.GetScreenSize() == ScreenSizes.Small )
			{
				_stackOrientation = StackOrientation.Vertical;
			}
		}

		protected override void OnSizeAllocated(double width, double height)
		{
			base.OnSizeAllocated (width, height);

		    if (_screenSizeHandler.GetScreenSize() != ScreenSizes.Small) return;
		    var orientation = _screenSizeHandler.OnSizeAllocated(width, height);

		    if (orientation == Orientations.Landscape)
		    {
		        _stackOrientation = StackOrientation.Horizontal;
		        ChangeOrientation ();				
		    }
		    if (orientation != Orientations.Portrait) return;
		    _stackOrientation = StackOrientation.Vertical;
		    ChangeOrientation ();
		}

		// TODO remove this when xamarin forms supports source/relative binding inside a datatemplate
		public void ChangeOrientation()
		{
			StatsList.BeginRefresh ();
			if (StatsList.ItemsSource != null)
			{
				foreach (var item in StatsList.ItemsSource)
				{
				    var stat = item as ExerciseStat;
				    if (stat != null) stat.CellLayoutOrientation = _stackOrientation;
				}
			}
			StatsList.EndRefresh ();
			Refresh ();
		}

		#endregion
	}

    public class ExerciseAnalysisXaml : BasePage<ExerciseAnalysisViewModel>
    {
    }
}

