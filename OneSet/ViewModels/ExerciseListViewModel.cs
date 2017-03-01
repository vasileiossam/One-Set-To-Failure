using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac;
using OneSet.Abstract;
using OneSet.Models;
using OneSet.Resx;
using Xamarin.Forms;

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

        private readonly INavigationService _navigationService;

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

        public ICommand SelectItemCommand { get; }
        public ICommand AddExerciseCommand { get; }

        public ExerciseListViewModel (IComponentContext componentContext, INavigationService navigationService, IExercisesRepository exercisesRepository)
        {
            _componentContext = componentContext;
            _navigationService = navigationService;
            _exercisesRepository = exercisesRepository;
            Title = AppResources.ExercisesTitle;

            SelectItemCommand = new Command(async (item) => { await OnItemSelected(item); });
            AddExerciseCommand = new Command(async () => { await _navigationService.NavigateTo<ExerciseDetailsViewModel>(); });
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

        private async Task OnItemSelected(object selectedItem)
        {
            var item = selectedItem as ExerciseItemViewModel;
            if (item == null) return;

            var parameters = new NavigationParameters()
            {
                {"Title", AppResources.EditExerciseTitle},
                {"Exercise", item.Exercise }
            };
            await _navigationService.NavigateTo<ExerciseDetailsViewModel>(parameters);
        }
    }
}

