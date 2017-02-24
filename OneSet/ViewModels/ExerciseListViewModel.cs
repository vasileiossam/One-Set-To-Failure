using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AutoMapper;
using OneSet.Abstract;
using OneSet.Resx;

namespace OneSet.ViewModels
{
	public class ExerciseListViewModel : BaseViewModel
	{
		protected ObservableCollection<ExerciseDetailsViewModel> _exercises;
		public ObservableCollection<ExerciseDetailsViewModel> Exercises
		{
			get
			{
				return _exercises;
			}
			set
			{
			    if (_exercises == value) return;
			    _exercises = value;
			    OnPropertyChanged ("Exercises");
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
			    if (_listVisible == value) return;
			    _listVisible = value;
			    OnPropertyChanged("ListVisible");
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
			    if (_noDataVisible == value) return;
			    _noDataVisible = value;
			    OnPropertyChanged("NoDataVisible");
			}
		}

        private readonly IExercisesRepository _exercisesRepository;

        public ExerciseListViewModel (IExercisesRepository exercisesRepository) : base()
		{
            _exercisesRepository = exercisesRepository;
            Title = AppResources.ExercisesTitle;
		}

        public override async Task OnLoad(object parameter = null)
		{
			try
			{
				var exercisesList = await _exercisesRepository.AllAsync();
				var exerciseViewModelsList = Mapper.Map<ObservableCollection<ExerciseDetailsViewModel>>(exercisesList);
				foreach(var item in exerciseViewModelsList)
				{
				    await item.OnLoad(parameter);
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

        public override Task OnSave()
        {
            throw new NotImplementedException();
        }
    }
}

