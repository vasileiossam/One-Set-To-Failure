using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Set
{
	public partial class CalendarNotesPage : ContentPage
	{
        public CalendarNotesViewModel ViewModel { get; set; }

        public CalendarNotesPage(DateTime date)
		{
			InitializeComponent ();


            ViewModel = new CalendarNotesViewModel() { Date = date };
		}

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = ViewModel;
        }
	}
}

