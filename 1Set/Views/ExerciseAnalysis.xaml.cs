using System;
using System.Collections.Generic;
using Set.ViewModels;
using Xamarin.Forms;

namespace Set
{
	public partial class ExerciseAnalysis : ContentPage
	{
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
				StatsList.ItemsSource = null;
				StatsList.ItemsSource = await ViewModel.GetStats(selectedIndex);
			}
		}
	}
}

