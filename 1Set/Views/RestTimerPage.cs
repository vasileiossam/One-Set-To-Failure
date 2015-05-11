using System;
using System.Collections.Generic;
using Set.ViewModels;
using Set.Models;
using Xamarin.Forms;
using Set.Resx;
using System.Threading.Tasks;

namespace Set
{
	public partial class RestTimerPage : ContentPage
	{
		private ScreenSizeHandler _screenSizeHandler;
		private StackOrientation _stackOrientation;

		public RestTimerViewModel ViewModel { get; set; }

		public RestTimerPage()
		{
			InitializeComponent ();

			_screenSizeHandler = new ScreenSizeHandler ();

			_stackOrientation = StackOrientation.Horizontal;
			if (_screenSizeHandler.GetStartingOrientation () == Orientations.Portrait) 
			{
				_stackOrientation = StackOrientation.Vertical;
			}
		}

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = ViewModel;
			ViewModel.ProgressBar = ProgressBar;
			await ViewModel.Load ();

			// following statement will prevent a compiler warning about async method lacking await
			await Task.FromResult(0);
        }

		protected override void OnDisappearing()
		{
			if (ViewModel != null)
			{
				ViewModel.StopTimers ();
			}
			base.OnDisappearing();
		}

		protected override void OnSizeAllocated(double width, double height)
		{
			base.OnSizeAllocated (width, height);

			var orientation = _screenSizeHandler.OnSizeAllocated(width, height);

			if (orientation == Orientations.Landscape)
			{
				MotivationalQuoteImage.Padding = new Thickness (10, 10, 0, 10);				
				_stackOrientation = StackOrientation.Horizontal;
				MainStackLayout.Orientation = _stackOrientation;
			}
			else
			if (orientation == Orientations.Portrait)
			{
				MotivationalQuoteImage.Padding = new Thickness (0, 0, 0, 10);					
				_stackOrientation = StackOrientation.Vertical;
				MainStackLayout.Orientation = _stackOrientation;
			}
		}
	}
}

