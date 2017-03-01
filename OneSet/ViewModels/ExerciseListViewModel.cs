using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Autofac;
using OneSet.Abstract;
using OneSet.Models;
using OneSet.Resx;

namespace OneSet.ViewModels
{
	public class ExerciseListViewModel : BaseViewModel, INavigationAware
    {
		protected ObservableCollection<ExerciseItemViewModel> _exercises;
		public ObservableCollection<ExerciseItemViewModel> Exercises
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

        private readonly IComponentContext _componentContext;
        private readonly IExercisesRepository _exercisesRepository;

        public ExerciseListViewModel (IComponentContext componentContext, IExercisesRepository exercisesRepository)
        {
            _componentContext = componentContext;
            _exercisesRepository = exercisesRepository;
            Title = AppResources.ExercisesTitle;
		}

        private async Task<ObservableCollection<ExerciseItemViewModel>> GetExercises()
        {
            var collection = new ObservableCollection<ExerciseItemViewModel>();
            var list = await _exercisesRepository.AllAsync();

            foreach (var item in list)
            {
                var vm = _componentContext.Resolve<ExerciseItemViewModel>();
                vm.Exercise = item;
                vm.TrainingDays = await _exercisesRepository.GetTrainingDays(item);
                collection.Add(vm);
            }

            return collection;
        }

        public async Task OnNavigatedFrom(NavigationParameters parameters)
        {
            await Task.FromResult(0);
        }

        public async Task OnNavigatedTo(NavigationParameters parameters)
        {
            Exercises = await GetExercises();
            ListVisible = Exercises.Count > 0;
            NoDataVisible = !ListVisible;
        }
    }
}

