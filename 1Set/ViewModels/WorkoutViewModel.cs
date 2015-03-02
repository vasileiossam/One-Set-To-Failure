using System;
using SQLite.Net.Attributes;
using Set.Models;
using System.Collections.Generic;
using Set.Resx;

namespace Set.ViewModels
{
    public class WorkoutViewModel : BaseViewModel
    {
		public Workout Workout {get; set; }

		public WorkoutViewModel ()
		{
			Title = AppResources.WorkoutTitle;
		}

        public void Save()
        {
            App.Database.WorkoutsRepository.Save(Workout);
        }
    }
}

