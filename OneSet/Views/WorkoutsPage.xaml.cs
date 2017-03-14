using System;
using OneSet.Abstract;
using OneSet.Models;
using OneSet.Services;
using OneSet.ViewModels;
using Xamarin.Forms;

namespace OneSet.Views
{
	public partial class WorkoutsPage : WorkoutsPageXaml, IScreenRotationAware
    {
        private readonly IMessagingService _messagingService;
        private readonly IScreenSizeHandler _screenSizeHandler;
        private StackOrientation _stackOrientation;

        public WorkoutsPage(IMessagingService messagingService, IScreenSizeHandler screenSizeHandler)
		{

            InitializeComponent ();
            _messagingService = messagingService;
		    _screenSizeHandler = screenSizeHandler;

            _messagingService.Subscribe<WorkoutsViewModel>(this, Messages.WorkoutsReloaded, sender =>
            {
                Refresh();
            });
        }

        ~WorkoutsPage()
        {
            _messagingService.Unsubscribe<WorkoutsViewModel>(this, Messages.WorkoutsReloaded);
        }

        #region private methods
        private void Refresh()
        {
            list.ItemsSource = null;
            list.ItemsSource = ViewModel.Routine;
            ChangeOrientation();
        }

        private void OnLeftChevronTapCommand(object sender, EventArgs args)
        {
            // Used by the gesture frame
            ViewModel.ChevronTapCommand.Execute("Left");
        }

        private void OnRightChevronTapCommand(object sender, EventArgs args)
        {
            // Used by the gesture frame
            ViewModel.ChevronTapCommand.Execute("Right");
        }

        #endregion

        #region IScreenRotationAware
        public void InitScreenSize()
        {
            _stackOrientation = StackOrientation.Horizontal;
            if (_screenSizeHandler.GetStartingOrientation() == Orientations.Portrait && _screenSizeHandler.GetScreenSize() == ScreenSizes.Small)
            {
                _stackOrientation = StackOrientation.Vertical;
            }
        }

        public void ChangeOrientation()
        {
            list.BeginRefresh();
            if (ViewModel.Routine != null)
            {
                foreach (var item in ViewModel.Routine)
                {
                    item.CellLayoutOrientation = _stackOrientation;
                }
            }
            list.EndRefresh();
        }
        #endregion

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            
            if (_screenSizeHandler.GetScreenSize() == ScreenSizes.Small)
            {
                var orientation = _screenSizeHandler.OnSizeAllocated(width, height);

                if (orientation == Orientations.Landscape)
                {
                    CurrentDate.FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label));
                    CalendarNotesButton.FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Button));
                    NoDataImage.IsVisible = false;
                    _stackOrientation = StackOrientation.Horizontal;
                    ChangeOrientation();
                }
                if (orientation == Orientations.Portrait)
                {
                    CurrentDate.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
                    CalendarNotesButton.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button));
                    NoDataImage.IsVisible = true;
                    _stackOrientation = StackOrientation.Vertical;
                    ChangeOrientation();
                }
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            InitScreenSize();
            ChangeOrientation();
            
            BindingContext = ViewModel;
            list.SelectedItem = null;

            MainFrame.SwipeLeft += OnLeftChevronTapCommand;
			MainFrame.SwipeRight += OnRightChevronTapCommand;

            ViewModel.RestTimerItem.Update();
        }

        protected override void OnDisappearing()
        {
            MainFrame.SwipeLeft -= OnLeftChevronTapCommand;
            MainFrame.SwipeRight -= OnRightChevronTapCommand;
            base.OnDisappearing();
        }
	}

    public class WorkoutsPageXaml : BasePage<WorkoutsViewModel>
    {
    }
}

