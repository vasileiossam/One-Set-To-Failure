using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using OneSet.Abstract;
using OneSet.Models;
using OneSet.Resx;

namespace OneSet.ViewModels
{
	public class ExerciseDetailsViewModel : BaseViewModel, INavigationAware
    {
        #region properties

        public int ExerciseId { get; set; }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private double _plateWeight;
        public double PlateWeight
        {
            get { return _plateWeight; }
            set { SetProperty(ref _plateWeight, value); }
        }

        private string _notes;
        public string Notes
        {
            get { return _notes; }
            set { SetProperty(ref _notes, value); }
        }

        public List<RoutineDay> Days{ get; set; }
        public RoutineDay Mon { get { return Days?.FirstOrDefault(x => x.DayOfWeek == 1); } }
        public RoutineDay Tue { get { return Days?.FirstOrDefault(x => x.DayOfWeek == 2); } }
        public RoutineDay Wed { get { return Days?.FirstOrDefault(x => x.DayOfWeek == 3); } }
        public RoutineDay Thu { get { return Days?.FirstOrDefault(x => x.DayOfWeek == 4); } }
        public RoutineDay Fri { get { return Days?.FirstOrDefault(x => x.DayOfWeek == 5); } }
        public RoutineDay Sat { get { return Days?.FirstOrDefault(x => x.DayOfWeek == 6); } }
        public RoutineDay Sun { get { return Days?.FirstOrDefault(x => x.DayOfWeek == 0); } }

        public ICommand DeleteCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        #endregion

        #region private variables
        private readonly IMasterDetailNavigation _navigationService;
        private readonly IDialogService _dialogService;
        private readonly IMessagingService _messagingService;
        private readonly IUnitsService _units;
        private readonly IExercisesRepository _exercisesRepository;
        private readonly IRoutineDaysRepository _routineDaysRepository;
        #endregion

        public ExerciseDetailsViewModel(
            IMasterDetailNavigation navigationService, IDialogService dialogService, IMessagingService messagingService, IUnitsService units, 
            IExercisesRepository exercisesRepository, IRoutineDaysRepository routineDaysRepository)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _messagingService = messagingService;
            _units = units;
            _exercisesRepository = exercisesRepository;
            _routineDaysRepository = routineDaysRepository;

            Title = AppResources.AddExerciseTitle;

            DeleteCommand = new Command(async () => { await OnDelete(); });
            SaveCommand = new Command(async () => await OnSave());
        }
        
        #region private methods
        private async Task OnDelete () 
		{
            try
            {
                var answer = await _dialogService.DisplayAlert (AppResources.ExerciseDeleteQuestionTitle, AppResources.ExerciseDeleteQuestion, AppResources.Yes, AppResources.No);

                if (answer)
                {
                    await _exercisesRepository.DeleteAsync(ExerciseId);

                    _messagingService.Send(this, Messages.ItemDeleted);

                    await _navigationService.PopAsync();
                }
            }
            catch (Exception ex)
            {
                App.ShowErrorPage(this, ex);
            }
        }

        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                App.ShowWarning(AppResources.ExerciseNameIsRequired);
                return false;
            }

            if (PlateWeight < 0 || PlateWeight > 999)
            {
                App.ShowWarning(AppResources.ExerciseInvalidPlateWeight);
                PlateWeight = 0;
                return false;
            }

            return true;
        }

        private async Task OnSave()
        {
            if (Validate())
            {
                var exercise = new Exercise()
                {
                    ExerciseId = this.ExerciseId,
                    Name = this.Name,
                    PlateWeight = _units.GetMetric(App.Settings.IsMetric, this.PlateWeight),
                    Notes = this.Notes
                };

                var message = Messages.ItemAdded;
                if (exercise.ExerciseId > 0)
                {
                    message = Messages.ItemChanged;
                }

                ExerciseId = await _exercisesRepository.SaveAsync(exercise);
                
                // save routine
                foreach (var day in Days)
                {
                    day.ExerciseId = ExerciseId;
                    await _routineDaysRepository.SaveAsync(day);
                }

                _messagingService.Send(this, message, exercise);
                await App.ShowSuccess(AppResources.ExerciseSaved);
                await _navigationService.PopAsync();
            }
        }
        #endregion

        #region INavigationAware
        public async Task OnNavigatedFrom(NavigationParameters parameters)
        {
            await Task.FromResult(0);
        }

        public async Task OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("Title"))
            {
                Title = (string) parameters["Title"];
            }
            if (parameters.ContainsKey("Exercise"))
            {
                var exercise = parameters["Exercise"] as Exercise;

                if (exercise != null)
                {
                    ExerciseId = exercise.ExerciseId;
                    Name = exercise.Name;
                    Notes = exercise.Notes;
                    PlateWeight = _units.GetWeight(App.Settings.IsMetric, exercise.PlateWeight);
                }
            }

            // make sure we have all days
            Days = await _routineDaysRepository.GetRoutine(ExerciseId);
            for (var i = 0; i <= 6; i++)
            {
                var day = Days.FirstOrDefault(x => x.DayOfWeek == i);
                if (day == null)
                {
                    day = new RoutineDay
                    {
                        DayOfWeek = i,
                        ExerciseId = ExerciseId,
                        IsActive = 0,
                        RowNumber = 1
                    };
                    Days.Add(day);
                }
            }
        }
        #endregion
    }
}


