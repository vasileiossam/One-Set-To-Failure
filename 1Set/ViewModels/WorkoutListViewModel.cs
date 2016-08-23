using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Set.Models;
using Set.Entities;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using AutoMapper;

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

		public RestTimerToolbarItem RestTimerToolbarItem { get; set; }

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

		private readonly ICommand _gotoDateCommand;
		public ICommand GotoDateCommand
		{
			get
			{
				return _gotoDateCommand;
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

        private ObservableCollection<RoutineDayViewModel> _routineDays;
		public ObservableCollection<RoutineDayViewModel> RoutineDays
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
			_analysisCommand = new Command (async() => { await OnAnalysisCommand(); });
			_gotoDateCommand = new Command (async() => { await OnGotoDateCommand(); });

		}

		public async Task Load(DateTime date)
		{
			RestTimerToolbarItem.Update();

			CurrentDate = date;

			CalendarNotes = await App.Database.CalendarRepository.GetCalendarNotes (_currentDate);
			if (CalendarNotes != null)
			{
				CalendarNotes = CalendarNotes.Trim ();

				// trim to two lines
				var countLines = CalendarNotes.ToCharArray ().Count (c => c == '\n');
				if (countLines > 2)
				{
					var lines = CalendarNotes.Split (new[]{ '\n' });
					if (lines.Count () >= 2)
					{
						CalendarNotes = string.Format ("{0}\n{1}...", lines [0].Trim (), lines [1].Trim ());
					}
				}
			}

			CalendarNotesVisible = !string.IsNullOrEmpty (CalendarNotes);

			var list = await App.Database.RoutineDaysRepository.GetRoutine (_currentDate);
            RoutineDays = Mapper.Map<ObservableCollection<RoutineDayViewModel>>(list);

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

		private async Task OnGotoDateCommand()
		{
			DependencyService.Get<IDatePickerDialog>().Show(OnGetDate);
		}

		private async void OnGetDate(object sender, EventArgs args)
		{
			if (sender is DateTime)
			{
				await Load ((DateTime) sender);

				Device.BeginInvokeOnMainThread (() =>
				{
					Page.ChangeOrientation (false);
					Page.Refresh ();
				});
			}
		}

    }
}

