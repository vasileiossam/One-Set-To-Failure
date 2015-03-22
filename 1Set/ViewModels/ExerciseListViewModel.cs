using System;
using System.Collections.Generic;
using Set.Models;
using System.Linq;
using System.Collections.ObjectModel;
using Set.Resx;
using Set.ViewModels;
using Xamarin.Forms;
using AutoMapper;
using System.Diagnostics;

namespace Set
{
	public class ExerciseListViewModel : BaseViewModel
	{
        public ObservableCollection<ExerciseViewModel> Exercises 
		{
			get
			{
				try
				{
					var exercisesList = App.Database.ExercisesRepository.All;
					var exerciseViewModelsList = Mapper.Map<ObservableCollection<ExerciseViewModel>>(exercisesList);
					foreach(var item in exerciseViewModelsList)
					{
						item.LoadRoutine();
					}
					return exerciseViewModelsList;
				}
				catch(Exception  ex)
				{
					App.ShowErrorPage (this, ex);
				}

				return null;
			}
		}

		public ExerciseListViewModel () : base()
		{
			Title = AppResources.ExercisesTitle;
		}
	}
}

