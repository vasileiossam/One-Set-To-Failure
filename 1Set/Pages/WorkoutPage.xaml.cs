using System;
using System.Collections.Generic;
using Set.ViewModels;
using Xamarin.Forms;
using System.Linq;

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
					_viewModel =  new WorkoutViewModel(Navigation);
					_viewModel.Navigation = Navigation;
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
            BindingContext = ViewModel;

			// Bug in Android where this list is not scrollable.
					 
//			string[] options = new string[50];
//			for (var i = 0; i < 50; i++)
//			{
//				options[i] = i.ToString();
//			}
//
//			WeightUpButton.Clicked += async (sender, e) => {
//							var action = await DisplayActionSheet ("ActionSheet: Save Photo?", "Cancel", "Delete",
//
//											
//					options);
//							 
//};
//		 

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

	}
}

