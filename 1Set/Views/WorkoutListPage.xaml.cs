using System;
using System.Collections.Generic;
using Set.ViewModels;
using Set.Models;
using Xamarin.Forms;

namespace Set
{
	public partial class WorkoutListPage : ContentPage
	{
		private double _width = 0.0;
		private double _height = 0.0;
		private StackOrientation _stackOrientation;

        private WorkoutListViewModel _viewModel;
        public WorkoutListViewModel ViewModel
        {
			get
            {
                if (_viewModel == null)
                {
					_viewModel =  new WorkoutListViewModel(){Navigation = this.Navigation};
					_viewModel.Page = this;
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
			this.InitializeComponent ();

			_stackOrientation = StackOrientation.Horizontal;
			if ((App.ScreenWidth <= 320) || (App.ScreenHeight <= 320))
			{
				// landscape
				if (App.ScreenWidth > App.ScreenHeight)
				{

				} 
				// portrait
				else
				{
					_stackOrientation = StackOrientation.Vertical;
				}
			}
		}

        protected async override void OnAppearing()
        {
            base.OnAppearing();

			this.BindingContext = ViewModel;
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
				Navigation = this.Navigation
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

			if (width != _width || height != _height)
			{
				_width = width;
				_height = height;

				// small screens
				if ((width <= 320) || (height <= 320))
				{
					// landscape
					if (width > height)
					{
						CurrentDate.FontSize = Device.GetNamedSize (NamedSize.Large, typeof(Label));					
						CalendarNotesButton.FontSize = Device.GetNamedSize (NamedSize.Large, typeof(Button));		
						NoDataImage.IsVisible = false;
						_stackOrientation = StackOrientation.Horizontal;
						ChangeOrientation (true);
					} 
					// portrait
					else
					{
						CurrentDate.FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label));					
						CalendarNotesButton.FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Button));					
						NoDataImage.IsVisible = true;		
						_stackOrientation = StackOrientation.Vertical;
						ChangeOrientation (true);
					}
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

