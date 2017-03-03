using OneSet.Models;

namespace OneSet.ViewModels
{
    public class ExerciseItemViewModel : BaseViewModel
    {
        protected Exercise _exercise;
        public Exercise Exercise
        {
            get
            {
                return _exercise;
            }
            set
            {
                if (_exercise == value) return;
                _exercise = value;
                OnPropertyChanged("Exercise");
                OnPropertyChanged("NotesVisible");
            }
        }

        protected string _trainingDays;
        public string TrainingDays
        {
            get
            {
                return _trainingDays;
            }
            set
            {
                if (_trainingDays == value) return;
                _trainingDays = value;
                OnPropertyChanged("TrainingDays");
                OnPropertyChanged("TrainingDaysVisible");
            }
        }
        
        public bool TrainingDaysVisible => !string.IsNullOrEmpty(TrainingDays);
        public bool NotesVisible => !string.IsNullOrEmpty(Exercise.Notes);
    }
}