using System.Threading.Tasks;
using OneSet.ViewModels;
using Xamarin.Forms;

namespace OneSet.Views
{
	public partial class RestTimerPage : ContentPage
	{
		private readonly ScreenSizeHandler _screenSizeHandler;
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

        protected override async void OnAppearing()
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
		    ViewModel?.StopTimer ();
		    base.OnDisappearing();
		}

		void OnTextChanged(object sender, TextChangedEventArgs  e)
		{
			var entry = sender as Entry;
		    if (entry == null) return;
		    var value = entry.Text;

		    // max length = 3
		    if (value.Length <= 3) return;
		    // Remove Last character 
		    value = value.Remove(value.Length - 1);
		    // Set the Old value
		    entry.Text = value;
		}

		protected override void OnSizeAllocated(double width, double height)
		{
			base.OnSizeAllocated (width, height);

			var orientation = _screenSizeHandler.OnSizeAllocated(width, height);

			switch (orientation)
			{
			    case Orientations.Landscape:
			        MotivationalQuoteImage.Padding = new Thickness (10, 10, 0, 10);				
			        _stackOrientation = StackOrientation.Horizontal;
			        MainStackLayout.Orientation = _stackOrientation;
			        break;
			    case Orientations.Portrait:
			        MotivationalQuoteImage.Padding = new Thickness (0, 0, 0, 10);					
			        _stackOrientation = StackOrientation.Vertical;
			        MainStackLayout.Orientation = _stackOrientation;
			        break;
			}
		}
	}
}

