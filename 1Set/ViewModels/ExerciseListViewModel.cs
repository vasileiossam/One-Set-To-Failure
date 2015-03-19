using System;
using System.Collections.Generic;
using Set.Models;
using System.Linq;
using System.Collections.ObjectModel;
using Set.Resx;
using Set.ViewModels;
using Xamarin.Forms;

namespace Set
{
	public class ExerciseListViewModel : BaseViewModel
	{
        public ObservableCollection<ExerciseViewModel> ExerciseViewModel 
		{
			get
			{
				var exercisesList = App.Database.ExercisesRepository.All;
              //  convert list to view models
				return null;
			}
		}

		public ExerciseListViewModel (INavigation navigation) : base(navigation)
		{
			Title = AppResources.ExercisesTitle;
		}
	}
}

