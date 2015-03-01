using System;
using System.Collections.Generic;
using Set.Models;
using System.Linq;
using System.Collections.ObjectModel;
using Set.Resx;
using Set.ViewModels;

namespace Set
{
	public class ExerciseListViewModel : BaseViewModel
	{
		public ObservableCollection<Exercise> Exercises 
		{
			get
			{
				return App.Database.ExercisesRepository.All;
			}
		}

		public ExerciseListViewModel ()
		{
			Title = AppResources.ExercisesTitle;
		}
	}
}

