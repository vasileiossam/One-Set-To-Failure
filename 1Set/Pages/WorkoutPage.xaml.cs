using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Set
{
	public partial class WorkoutPage : ContentPage
	{
        private WorkoutViewModel _viewModel;
        public WorkoutViewModel ViewModel
        {
            set
            {
                if (_viewModel == null)
                {
                    _viewModel = new WorkoutViewModel();
                }
            }
            get
            {
                return _viewModel;
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

