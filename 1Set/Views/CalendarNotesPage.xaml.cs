using System;
using System.Collections.Generic;
using Set.ViewModels;
using Set.Models;
using Xamarin.Forms;
using Set.Resx;

namespace Set
{
	public partial class CalendarNotesPage : ContentPage
	{
        public CalendarNotesViewModel ViewModel { get; set; }

		public CalendarNotesPage(DateTime date, INavigation navigation)
		{
			InitializeComponent ();

			ViewModel = new CalendarNotesViewModel() { Date = date };
			ViewModel.Navigation = navigation;
		}

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = ViewModel;
        }

	}
}

