using System;
using System.Collections.Generic;
using Set.ViewModels;
using Xamarin.Forms;

namespace Set
{
	public partial class ExercisePage : ContentPage
	{
		private ExerciseViewModel _viewModel;
		public ExerciseViewModel ViewModel
		{
			get
			{
				if (_viewModel == null)
				{
					_viewModel =  new ExerciseViewModel();
				}
				return _viewModel;
			}
			set
			{
				_viewModel = value;
			}
		}

		public ExercisePage ()
		{
			InitializeComponent ();
		}

		protected async override void OnAppearing()
		{
			base.OnAppearing();
			BindingContext = ViewModel;
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
		}
	}
}

