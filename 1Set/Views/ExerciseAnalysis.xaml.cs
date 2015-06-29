using System;
using System.Collections.Generic;
using Set.ViewModels;
using Xamarin.Forms;
using Set.Abstract;

namespace Set
{
	public partial class ExerciseAnalysis : ContentPage, IScreenSizeHandler
	{
		private ScreenSizeHandler _screenSizeHandler;
		private StackOrientation _stackOrientation;

		private ExerciseAnalysisViewModel _viewModel;
		public ExerciseAnalysisViewModel ViewModel
		{
			get
			{
				if (_viewModel == null)
				{
					_viewModel = new ExerciseAnalysisViewModel
					{
						Navigation = Navigation,
					};
				}
				return _viewModel;
			}
			set
			{
				_viewModel = value;
			}
		}

		public ExerciseAnalysis ()
		{
			InitializeComponent ();
			InitScreenSizeHandler ();
		}

		protected async override void OnAppearing()
		{
			base.OnAppearing ();
			ViewModel.ExercisesPicker = ExercisesPicker;
			await ViewModel.Load ();

			ExercisesPicker.SelectedIndex = 0;
		}

		private async void OnSelectedIndexChanged(object sender, EventArgs eventArgs)
		{
			var selectedIndex = (sender as Picker).SelectedIndex;
			if (selectedIndex > -1)
			{
				ViewModel.Stats = await ViewModel.GetStats(selectedIndex);
				ChangeOrientation ();
			}
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
			if ((_screenSizeHandler.GetStartingOrientation () == Orientations.Portrait) 
				&& (_screenSizeHandler.GetScreenSize() == ScreenSizes.Small) )
			{
				_stackOrientation = StackOrientation.Vertical;
			}
		}

		protected override void OnSizeAllocated(double width, double height)
		{
			base.OnSizeAllocated (width, height);

			if (_screenSizeHandler.GetScreenSize () == ScreenSizes.Small)
			{
				var orientation = _screenSizeHandler.OnSizeAllocated(width, height);

				if (orientation == Orientations.Landscape)
				{
					_stackOrientation = StackOrientation.Horizontal;
					ChangeOrientation ();				
				}
				if (orientation == Orientations.Portrait)
				{
					_stackOrientation = StackOrientation.Vertical;
					ChangeOrientation ();
				}
			}
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
					stat.CellLayoutOrientation = _stackOrientation;
				}
			}
			StatsList.EndRefresh ();
			Refresh ();
		}

		#endregion
	}
}

