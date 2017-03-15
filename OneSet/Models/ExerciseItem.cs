namespace OneSet.Models
{
    public class ExerciseItem : ObservableObject
    {
        protected Exercise _exercise;
        public Exercise Exercise
        {
            get { return _exercise; }
            set
            {
                SetProperty(ref _exercise, value);
                OnPropertyChanged("NotesVisible");
            }
        }

        protected string _trainingDays;
        public string TrainingDays
        {
            get { return _trainingDays; }
            set
            {
                SetProperty(ref _trainingDays, value);
                OnPropertyChanged("TrainingDaysVisible");
            }
        }
        
        public bool TrainingDaysVisible => !string.IsNullOrEmpty(TrainingDays);
        public bool NotesVisible => !string.IsNullOrEmpty(Exercise?.Notes);
    }
}