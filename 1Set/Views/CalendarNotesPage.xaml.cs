using System;
using System.Collections.Generic;
using Set.ViewModels;
using Set.Models;
using Xamarin.Forms;
using Set.Resx;
using System.Threading.Tasks;

namespace Set
{
	public partial class CalendarNotesPage : ContentPage
	{
        public CalendarNotesViewModel ViewModel { get; set; }

		public CalendarNotesPage()
		{
			InitializeComponent ();
		}

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = ViewModel;

			// following statement will prevent a compiler warning about async method lacking await
			await Task.FromResult(0);
        }
	}
}

