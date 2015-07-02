using System;
using Set.Models;
using Set.ViewModels;
using Xamarin.Forms;

namespace Set
{
	public partial class WorkoutListPage : ContentPage
	{
		private ScreenSizeHandler _screenSizeHandler;
		private StackOrientation _stackOrientation;

        private WorkoutListViewModel _viewModel;
        public WorkoutListViewModel ViewModel
        {
			get
            {
                if (_viewModel == null)
                {
                    _viewModel = new WorkoutListViewModel
                    {
                        Navigation = Navigation,
                        Page = this,
						RestTimerToolbarItem = new RestTimerToolbarItem() {Navigation = Navigation}
                    };
                }
				return _viewModel;
            }
			set
            {
				_viewModel = value;
            }
        }

        public WorkoutListPage()
		{
			InitializeComponent ();
			_screenSizeHandler = new ScreenSizeHandler ();

			_stackOrientation = StackOrientation.Horizontal;
			if ((_screenSizeHandler.GetStartingOrientation () == Orientations.Portrait) 
				&& (_screenSizeHandler.GetScreenSize() == ScreenSizes.Small) )
			{
				_stackOrientation = StackOrientation.Vertical;
			}
		}

        protected async override void OnAppearing()
        {
            base.OnAppearing();

			BindingContext = ViewModel;
			await ViewModel.Load (App.CurrentDate);
			workoutsList.ItemsSource = ViewModel.RoutineDays;
			ChangeOrientation (true);

			MainFrame.SwipeLeft += OnLeftChevronTapCommand;
			MainFrame.SwipeRight += OnRightChevronTapCommand;
        }

		private async void OnLeftChevronTapCommand(object sender, EventArgs args)
		{
			ViewModel.ChevronTapCommand.Execute("Left");
		}

		private async void OnRightChevronTapCommand(object sender, EventArgs args)
		{
			ViewModel.ChevronTapCommand.Execute("Right");
		}

        protected override void OnDisappearing()
        {
			MainFrame.SwipeLeft -= OnLeftChevronTapCommand;
			MainFrame.SwipeRight -= OnRightChevronTapCommand;			
            base.OnDisappearing();
        }

        public void OnWorkoutSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (((ListView)sender).SelectedItem == null) return;

			var viewModel = new WorkoutViewModel()
            {
                Workout = (e.SelectedItem as RoutineDay).Workout,
				Navigation = Navigation,
				RestTimerToolbarItem = ViewModel.RestTimerToolbarItem
            };

            var workoutPage = new WorkoutPage
            {
                ViewModel = viewModel
            };

            Navigation.PushAsync(workoutPage);

			// deselect row
			((ListView)sender).SelectedItem = null;
        }

		public void OnExercisesButtonClicked(object sender, EventArgs args)
		{
			Navigation.PushAsync( new ExerciseListPage());
		}

		public void OnSettingsButtonClicked(object sender, EventArgs args)
		{
			Navigation.PushAsync( new SettingsPage());
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
					ChangeOrientation (true);				
				}
				if (orientation == Orientations.Portrait)
				{
					CurrentDate.FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label));					
					CalendarNotesButton.FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Button));					
					NoDataImage.IsVisible = true;		
					_stackOrientation = StackOrientation.Vertical;
					ChangeOrientation (true);
				}
			}
		}

		// TODO remove this when xamarin forms supports source/relative binding inside a datatemplate
		public void ChangeOrientation(bool canRefresh)
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
			Refresh ();
		}
	}
}

