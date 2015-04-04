using System;
using System.Collections.Generic;
using Set.ViewModels;
using Xamarin.Forms;
using System.Linq;

namespace Set
{
	public partial class WorkoutPage : ContentPage
	{
		public WorkoutViewModel ViewModel { get; set; }

		public WorkoutPage ()
		{
			InitializeComponent ();
		}

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = ViewModel;
			await ViewModel.Load ();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

	}
}

