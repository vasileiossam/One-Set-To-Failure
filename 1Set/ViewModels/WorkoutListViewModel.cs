using System;
using SQLite.Net.Attributes;
using Set.Models;
using System.Collections.Generic;

namespace Set.ViewModels
{
    public class WorkoutListViewModel : BaseViewModel
    {
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

    }
}

