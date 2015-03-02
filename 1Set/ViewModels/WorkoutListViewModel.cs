using System;
using SQLite.Net.Attributes;
using Set.Models;
using System.Windows.Input;
using Xamarin.Forms;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Collections.Generic;

namespace Set.ViewModels
{
	public class WorkoutListViewModel : BaseViewModel
    {
		private ICommand _chevronTapCommand;
		public ICommand ChevronTapCommand
		{
			get
			{
				return _chevronTapCommand;
			}
		}

        private DateTime _currentDate;
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
                    OnPropertyChanged("CurrentDate");
                    LoadRoutineDays();
                }
            }
        }

		public List<RoutineDay> RoutineDays {get; set; }

		public WorkoutListViewModel ()
		{
			Title = "One Set To Exhaustion";

			_chevronTapCommand = new Command (OnChevronTapCommand);
		}

        private void LoadRoutineDays()
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
					workout.Exercise = day.Exercise;
				} 

				day.Workout = workout;
			}
        }

		private void OnChevronTapCommand (object s) 
		{
			if ((string)s == "Left")
			{
				CurrentDate = CurrentDate.AddDays (-1);
			} 
			else
			{
				CurrentDate = CurrentDate.AddDays (1);
			}
		}
    }
}

