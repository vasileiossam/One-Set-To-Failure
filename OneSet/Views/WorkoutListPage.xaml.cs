using System;
using System.Threading.Tasks;
using Autofac;
using OneSet.Abstract;
using OneSet.Models;
using OneSet.ViewModels;
using Xamarin.Forms;

namespace OneSet.Views
{
	public partial class WorkoutListPage : WorkoutListPageXaml
    {
		private readonly ScreenSizeHandler _screenSizeHandler;
		private StackOrientation _stackOrientation;
        private readonly IComponentContext _componentContext;
        private readonly INavigationService _navigationService;
        private readonly IMessagingService _messagingService;

        public WorkoutListPage(IComponentContext componentContext, INavigationService navigationService, IMessagingService messagingService)
		{

            InitializeComponent ();
            _componentContext = componentContext;
            _navigationService = navigationService;
            _messagingService = messagingService;
            _screenSizeHandler = new ScreenSizeHandler ();

			_stackOrientation = StackOrientation.Horizontal;
			if (_screenSizeHandler.GetStartingOrientation () == Orientations.Portrait && _screenSizeHandler.GetScreenSize() == ScreenSizes.Small )
			{
				_stackOrientation = StackOrientation.Vertical;
			}

            _messagingService = messagingService;
            _messagingService.Subscribe<WorkoutListViewModel>(this, Messages.WorkoutsReloaded, sender =>
            {
                Refresh();
                ChangeOrientation();
            });
        }

        ~WorkoutListPage()
        {
            _messagingService.Unsubscribe<WorkoutListViewModel>(this, Messages.WorkoutsReloaded);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

			BindingContext = ViewModel;
			workoutsList.ItemsSource = ViewModel.RoutineDays;
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

        public async Task OnWorkoutSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as RoutineDayViewModel;
            if (item == null) return;

            var parameters = new NavigationParameters
            {
                {"Workout", item.Workout},
                {"Exercise", item.Exercise},
                {"RestTimerToolbarItem", ViewModel.RestTimerToolbarItem}
            };
            await _navigationService.NavigateTo<WorkoutDetailsViewModel>(parameters);

            // deselect row
			((ListView)sender).SelectedItem = null;
        }

		public async Task OnExercisesButtonClicked(object sender, EventArgs args)
		{
		    await _navigationService.NavigateTo<ExerciseListViewModel>();
		}

		public async Task OnSettingsButtonClicked(object sender, EventArgs args)
		{
            await _navigationService.NavigateTo<SettingsViewModel>();
		}

		public void Refresh()
		{
			workoutsList.ItemsSource = null;
			workoutsList.ItemsSource = ViewModel.RoutineDays;
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

		// TODO remove this when xamarin forms supports source/relative binding inside a datatemplate
		public void ChangeOrientation()
		{
			workoutsList.BeginRefresh ();
			if (ViewModel.RoutineDays != null)
			{
				foreach (var item in ViewModel.RoutineDays)
				{
					item.CellLayoutOrientation = _stackOrientation;
				}
			}
			workoutsList.EndRefresh ();
		}
	}

    public class WorkoutListPageXaml : BasePage<WorkoutListViewModel>
    {
    }
}

