﻿using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Set.Models;
using Xamarin.Forms;

namespace Set.ViewModels
{
	public class WorkoutListViewModel : BaseViewModel
    {
		// http://forums.xamarin.com/discussion/18631/listview-binding-to-observablecollection-does-not-update-gui
		// very bad to reference the view here but I need a way to refresh 
		// ListView which doesn't gets update when LoadRoutineDays()
		// try again in the next xamarin forms update
		public WorkoutListPage Page { get; set; }

        private string _trophies;
		public string Trophies
		{
			get
			{
				return _trophies;
			}
			set
			{
			    if (_trophies == value) return;
			    _trophies = value;
			    OnPropertyChanged("Trophies");
			}
		}

        private string _calendarNotes;
		public string CalendarNotes
		{
			get
			{
				return _calendarNotes;
			}
			set
			{
				if (_calendarNotes != value)
				{
					_calendarNotes = value;
					OnPropertyChanged("CalendarNotes");
				}
			}
		}

        private bool _calendarNotesVisible;
		public bool CalendarNotesVisible 
		{ 
			get
			{
				return _calendarNotesVisible;
			}
			set
			{
				if (_calendarNotesVisible != value)
				{
					_calendarNotesVisible = value;
					OnPropertyChanged("CalendarNotesVisible");
				}
			}
		}

        private bool _workoutsListVisible;
		public bool WorkoutsListVisible
		{
			get
			{
				return _workoutsListVisible;
			}
			set
			{
				if (_workoutsListVisible != value)
				{
					_workoutsListVisible = value;
					OnPropertyChanged("WorkoutsListVisible");
				}
			}
		}

		private bool _noWorkoutDataVisible;
		public bool NoWorkoutDataVisible
		{
			get
			{
				return _noWorkoutDataVisible;
			}
			set
			{
				if (_noWorkoutDataVisible != value)
				{
					_noWorkoutDataVisible = value;
					OnPropertyChanged("NoWorkoutDataVisible");
				}
			}
		}

		private readonly ICommand _chevronTapCommand;
		public ICommand ChevronTapCommand
		{
			get
			{
				return _chevronTapCommand;
			}
		}

		private readonly ICommand _calendarNotesCommand;
		public ICommand CalendarNotesCommand
		{
			get
			{
				return _calendarNotesCommand;
			}
		}

		private readonly ICommand _analysisCommand;
		public ICommand AnalysisCommand
		{
			get
			{
				return _analysisCommand;
			}
		}

		private readonly ICommand _restTimerCommand;
		public ICommand RestTimerCommand
		{
			get
			{
				return _restTimerCommand;
			}
		}

        private DateTime _currentDate;
        public DateTime CurrentDate
        {
			get
			{
				return _currentDate;
			}
			set
            {
                if (_currentDate == value) return;
                _currentDate = value;
                App.CurrentDate = value;
                OnPropertyChanged("CurrentDate");
            }
        }

        private ObservableCollection<RoutineDay> _routineDays;
		public ObservableCollection<RoutineDay> RoutineDays
		{
			get
			{
				return _routineDays;
			}
			set
			{ 
				if (_routineDays != value)
				{				
					_routineDays = value;
					OnPropertyChanged ("RoutineDays");
				}
			}
		}

		public WorkoutListViewModel ()
		{
			Title = "One Set To Fatigue";

			_chevronTapCommand = new Command (async(object s) => { await OnChevronTapCommand(s); });
			_calendarNotesCommand = new Command (async() => { await OnCalendarNotesCommand(); });
			_restTimerCommand = new Command (async() => { await OnRestTimerCommand(); });
			_analysisCommand = new Command (async() => { await OnAnalysisCommand(); });
		}

		public async Task Load(DateTime date)
		{
			CurrentDate = date;
			CalendarNotes = await App.Database.CalendarRepository.GetCalendarNotes (_currentDate);
			CalendarNotesVisible = !string.IsNullOrEmpty (CalendarNotes);

			var list = await App.Database.RoutineDaysRepository.GetRoutine (_currentDate);
			RoutineDays = new ObservableCollection<RoutineDay>(list);

			if (RoutineDays == null)
			{
				WorkoutsListVisible = false;
			}
			else
			{
				WorkoutsListVisible = RoutineDays.Count > 0;
			}
			NoWorkoutDataVisible = !WorkoutsListVisible;

			if (App.TotalTrophies == null)
			{
  				App.TotalTrophies = await App.Database.WorkoutsRepository.GetTotalTrophies ();
			}
			var dayTrophies = await App.Database.WorkoutsRepository.GetTrophies (CurrentDate);
			Trophies = string.Format("{0} / {1}", dayTrophies, (int) App.TotalTrophies);
		}

		private async Task OnChevronTapCommand (object s) 
		{
			if ((string)s == "Left")
			{
				await Load(CurrentDate.AddDays (-1));
				Page.ChangeOrientation (false);
				Page.Refresh ();
			} 
			else
			{
				await Load(CurrentDate.AddDays (1));
				Page.ChangeOrientation (false);
				Page.Refresh ();
			}
		}

		private async Task OnCalendarNotesCommand()
		{
			var viewModel = new CalendarNotesViewModel() {Navigation = Page.Navigation, Date = CurrentDate };
			await viewModel.Load ();

			var page = new CalendarNotesPage () {ViewModel = viewModel};

			await Navigation.PushAsync(page); 	
		}

		private async Task OnAnalysisCommand()
		{
			var viewModel = new AnalysisViewModel() {Navigation = Page.Navigation};
			var page = new AnalysisPage () {ViewModel = viewModel};
			await Navigation.PushAsync(page); 	
		}

		private async Task OnRestTimerCommand()
		{
			var viewModel = new RestTimerViewModel() {Navigation = Page.Navigation};
			var page = new RestTimerPage () {ViewModel = viewModel};
			await Navigation.PushAsync(page); 	
		}

    }
}

