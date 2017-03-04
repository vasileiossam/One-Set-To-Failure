using System;
using System.Threading.Tasks;
using Autofac;
using OneSet.Abstract;
using OneSet.Models;
using OneSet.ViewModels;
using Xamarin.Forms;

namespace OneSet.Views
{
	public partial class MainPage : MainPageXaml
    {
		private readonly ScreenSizeHandler _screenSizeHandler;
		private StackOrientation _stackOrientation;
        private readonly IMessagingService _messagingService;

        public MainPage(IMessagingService messagingService)
		{

            InitializeComponent ();
            _messagingService = messagingService;
            _screenSizeHandler = new ScreenSizeHandler ();

			_stackOrientation = StackOrientation.Horizontal;
			if (_screenSizeHandler.GetStartingOrientation () == Orientations.Portrait && _screenSizeHandler.GetScreenSize() == ScreenSizes.Small )
			{
				_stackOrientation = StackOrientation.Vertical;
			}

            _messagingService = messagingService;

            _messagingService.Subscribe<MainViewModel>(this, Messages.WorkoutsReloaded, sender =>
            {
                Refresh();
                ChangeOrientation();
            });
        }

        ~MainPage()
        {
            _messagingService.Unsubscribe<MainViewModel>(this, Messages.WorkoutsReloaded);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

			BindingContext = ViewModel;
            list.SelectedItem = null;
            ChangeOrientation();

			MainFrame.SwipeLeft += OnLeftChevronTapCommand;
			MainFrame.SwipeRight += OnRightChevronTapCommand;
        }

		private void OnLeftChevronTapCommand(object sender, EventArgs args)
		{
			ViewModel.ChevronTapCommand.Execute("Left");
		}

		private void OnRightChevronTapCommand(object sender, EventArgs args)
		{
			ViewModel.ChevronTapCommand.Execute("Right");
		}

        protected override void OnDisappearing()
        {
			MainFrame.SwipeLeft -= OnLeftChevronTapCommand;
			MainFrame.SwipeRight -= OnRightChevronTapCommand;			
            base.OnDisappearing();
        }

		public void Refresh()
		{
            list.ItemsSource = null;
            list.ItemsSource = ViewModel.Routine;
		}

		protected override void OnSizeAllocated(double width, double height)
		{
			base.OnSizeAllocated (width, height);


			if (_screenSizeHandler.GetScreenSize () == ScreenSizes.Small)
			{
				var orientation = _screenSizeHandler.OnSizeAllocated(width, height);

				if (orientation == Orientations.Landscape)
				{
					CurrentDate.FontSize = Device.GetNamedSize (NamedSize.Large, typeof(Label));					
					CalendarNotesButton.FontSize = Device.GetNamedSize (NamedSize.Large, typeof(Button));		
					NoDataImage.IsVisible = false;
					_stackOrientation = StackOrientation.Horizontal;
					ChangeOrientation();				
				}
				if (orientation == Orientations.Portrait)
				{
					CurrentDate.FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label));					
					CalendarNotesButton.FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Button));					
					NoDataImage.IsVisible = true;		
					_stackOrientation = StackOrientation.Vertical;
					ChangeOrientation();
				}
			}
		}

    	public void ChangeOrientation()
		{
            list.BeginRefresh ();
			if (ViewModel.Routine != null)
			{
				foreach (var item in ViewModel.Routine)
				{
					item.CellLayoutOrientation = _stackOrientation;
				}
			}
            list.EndRefresh ();
		}
	}

    public class MainPageXaml : BasePage<MainViewModel>
    {
    }
}

