using System;
using System.Collections.Generic;
using Set.ViewModels;
using Xamarin.Forms;

namespace Set
{
	public partial class WorkoutPage : ContentPage
	{
        private WorkoutViewModel _viewModel;
        public WorkoutViewModel ViewModel
        {
			get
			{
				if (_viewModel == null)
				{
					_viewModel =  new WorkoutViewModel();
				}
				return _viewModel;
			}
			set
			{
				_viewModel = value;
			}
        }


		public WorkoutPage ()
		{
			InitializeComponent ();
		}

        protected async override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
	}
}

