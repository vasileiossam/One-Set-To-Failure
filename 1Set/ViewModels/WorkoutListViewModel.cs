using System;
using SQLite.Net.Attributes;
using Set.Models;
using System.Windows.Input;
using Xamarin.Forms;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Set.ViewModels
{
	public class WorkoutListViewModel : BaseViewModel
    {
		// http://forums.xamarin.com/discussion/18631/listview-binding-to-observablecollection-does-not-update-gui
		// very bad to reference the view here but I need a way to refresh 
		// ListView which doesn't gets update when LoadRoutineDays()
		// try again in the next xamarin forms update
		public WorkoutListPage Page { get; set; }

		public bool WorkoutsListVisible
		{
			get
			{
				if (RoutineDays == null)
				{
					return false;
				}

				return RoutineDays.Count > 0;
			}
		}
		public bool NoWorkoutDataVisible
		{
			get
			{
				return !WorkoutsListVisible;
			}
		}

		private ICommand _chevronTapCommand;
		public ICommand ChevronTapCommand
		{
			get
			{
				return _chevronTapCommand;
			}
		}

		protected DateTime _currentDate;
        public DateTime CurrentDate
        {
			get
			{
				return _currentDate;
			}
			set
            {
                if (_currentDate != value)
                {
                    _currentDate = value;
					App.CurrentDate = value;
                    OnPropertyChanged("CurrentDate");
					RoutineDays = LoadRoutineDays();
                }
            }
        }

		protected ObservableCollection<RoutineDay> _routineDays;
		public ObservableCollection<RoutineDay> RoutineDays
		{
			get
			{
				_routineDays = LoadRoutineDays ();
				return _routineDays;
			}
			set
			{ 
				_routineDays = value;
				OnPropertyChanged("RoutineDays");
				OnPropertyChanged("NoWorkoutDataVisible");
				OnPropertyChanged("WorkoutsListVisible");
			}
		}

		public WorkoutListViewModel (INavigation navigation) : base(navigation)
		{
			Title = "One Set To Exhaustion";

			_chevronTapCommand = new Command (OnChevronTapCommand);
		}

		protected ObservableCollection<RoutineDay> LoadRoutineDays()
        {
			var routineDays = App.Database.RoutineDaysRepository.GetRoutine (_currentDate);
			var workouts = App.Database.WorkoutsRepository.GetWorkouts(_currentDate);

			foreach (var day in routineDays)
			{
				var workout = workouts.Find(x => x.ExerciseId == day.ExerciseId);

				// workout hasn't performed for this exercise
				if (workout == null)
				{
					workout = new Workout ();
					workout.ExerciseId = day.ExerciseId;
				} 

				day.Workout = workout;
			}

			return new ObservableCollection<RoutineDay>(routineDays);
        }

		private void OnChevronTapCommand (object s) 
		{
			if ((string)s == "Left")
			{
				CurrentDate = CurrentDate.AddDays (-1);
				Page.Refresh ();
			} 
			else
			{
				CurrentDate = CurrentDate.AddDays (1);
				Page.Refresh ();
			}
		}
    }
}

