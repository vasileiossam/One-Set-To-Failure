using System.Collections.ObjectModel;
using System.Linq;
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
        #region properties
        private ObservableCollection<ExerciseItem> _exercises;
		public ObservableCollection<ExerciseItem> Exercises
        {
            get { return _exercises; }
		    set
		    {
		        SetProperty(ref _exercises, value);
		        UpdateVisible();
		    }
        }

		public bool ListVisible => Exercises.Count > 0;
        public bool NoDataVisible => !ListVisible;
        #endregion

        #region private variables
        private readonly INavigationService _navigationService;
        private readonly IExercisesRepository _exercisesRepository;
        private readonly IMessagingService _messagingService;
        #endregion

        #region commands
        public ICommand SelectItemCommand { get; }
        public ICommand AddExerciseCommand { get; }
        #endregion

        public ExerciseListViewModel (INavigationService navigationService, IMessagingService messagingService, IExercisesRepository exercisesRepository)
        {
            _navigationService = navigationService;
            _messagingService = messagingService;
            _exercisesRepository = exercisesRepository;
            Title = AppResources.ExercisesTitle;

            SelectItemCommand = new Command(async (item) => { await OnItemSelected(item); });
            AddExerciseCommand = new Command(async () => { await _navigationService.NavigateTo<ExerciseDetailsViewModel>(); });

            _messagingService.Subscribe<ExerciseDetailsViewModel, Exercise>(this, Messages.ItemAdded, async (sender, e) =>
            {
                var item = new ExerciseItem
                {
                    Exercise = e,
                    TrainingDays = await _exercisesRepository.GetTrainingDays(e)
                };
                Exercises.Add(item);
                UpdateVisible();
            });
            _messagingService.Subscribe<ExerciseDetailsViewModel, Exercise>(this, Messages.ItemChanged, async (sender, e) =>
            {
                var item = Exercises.FirstOrDefault(x => x.Exercise.ExerciseId == e.ExerciseId);
                if (item == null) return;
                item.Exercise = null;
                item.Exercise = e;
                item.TrainingDays = await _exercisesRepository.GetTrainingDays(e);
            });
            _messagingService.Subscribe<ExerciseDetailsViewModel>(this, Messages.ItemDeleted, sender =>
            {
                var item = Exercises.FirstOrDefault(x => x.Exercise.ExerciseId == sender.ExerciseId);
                Exercises.Remove(item);
                UpdateVisible();
            });
        }

        ~ExerciseListViewModel()
        {
            _messagingService.Unsubscribe<ExerciseDetailsViewModel, Exercise>(this, Messages.ItemAdded);
            _messagingService.Unsubscribe<ExerciseDetailsViewModel, Exercise>(this, Messages.ItemChanged);
            _messagingService.Unsubscribe<ExerciseDetailsViewModel>(this, Messages.ItemDeleted);
        }
        
        #region private methods
        private async Task<ObservableCollection<ExerciseItem>> GetExercises()
        {
            var collection = new ObservableCollection<ExerciseItem>();
            var list = await _exercisesRepository.AllAsync();

            foreach (var item in list)
            {
                var vm = new ExerciseItem
                {
                    Exercise = item,
                    TrainingDays = await _exercisesRepository.GetTrainingDays(item)
                };
                collection.Add(vm);
            }

            return collection;
        }

        private async Task OnItemSelected(object selectedItem)
        {
            var item = selectedItem as ExerciseItem;
            if (item == null) return;

            var parameters = new NavigationParameters()
            {
                {"Title", AppResources.EditExerciseTitle},
                {"Exercise", item.Exercise }
            };
            await _navigationService.NavigateTo<ExerciseDetailsViewModel>(parameters);
        }

        private void UpdateVisible()
        {
            OnPropertyChanged("ListVisible");
            OnPropertyChanged("NoDataVisible");
        }
        #endregion

        #region INavigationAware
        public async Task OnNavigatedFrom(NavigationParameters parameters)
        {
            await Task.FromResult(0);
        }

        public async Task OnNavigatedTo(NavigationParameters parameters)
        {
            Exercises = await GetExercises();
        }
        #endregion
    }
}

