using System;
using SQLite.Net.Attributes;

namespace Set.ViewModels
{
    public class WorkoutListViewModel : BaseViewModel
    {
        private DateTime _currentDate;
        public DateTime CurrentDate
        {
            set
            {
                if (_currentDate != value)
                {
                    _currentDate = value;
                    OnPropertyChanged("CurrentDate");
                    LoadRoutines();
                }
            }
            get
            {
                return _currentDate;
            }
        }

       // public IEnumerable<Routine> Routines {get; set; }

        private void LoadRoutines()
        {
         //   Routines = App.RoutineRepository.GetActiveRoutines(_currentDate);
        }
    }
}

