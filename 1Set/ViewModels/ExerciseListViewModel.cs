using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AutoMapper;
using OneSet.Resx;
using OneSet.ViewModels;

namespace OneSet
{
	public class ExerciseListViewModel : BaseViewModel
	{
		protected ObservableCollection<ExerciseViewModel> _exercises;
		public ObservableCollection<ExerciseViewModel> Exercises
		{
			get
			{
				return _exercises;
			}
			set
			{ 
				if (_exercises != value)
				{				
					_exercises = value;
					OnPropertyChanged ("Exercises");
				}
			}
		}

		protected bool _listVisible;
		public bool ListVisible
		{
			get
			{
				return _listVisible;
			}
			set
			{
				if (_listVisible != value)
				{
					_listVisible = value;
					OnPropertyChanged("ListVisible");
				}
			}
		}

		protected bool _noDataVisible;
		public bool NoDataVisible
		{
			get
			{
				return _noDataVisible;
			}
			set
			{
				if (_noDataVisible != value)
				{
					_noDataVisible = value;
					OnPropertyChanged("NoDataVisible");
				}
			}
		}

		public ExerciseListViewModel () : base()
		{
			Title = AppResources.ExercisesTitle;
		}

		public async Task Load()
		{
			try
			{
				var exercisesList = await App.Database.ExercisesRepository.AllAsync();
				var exerciseViewModelsList = Mapper.Map<ObservableCollection<ExerciseViewModel>>(exercisesList);
				foreach(var item in exerciseViewModelsList)
				{
					await item.Load();
				}

				if (exerciseViewModelsList ==  null)
				{
					ListVisible = false;
				}
				else
				{
					ListVisible = exerciseViewModelsList.Count > 0;
				}
				NoDataVisible = !ListVisible;

				Exercises = exerciseViewModelsList;
			}
			catch(Exception  ex)
			{
				App.ShowErrorPage (this, ex);
			}
		}
	}
}

