using OneSet.Models;

namespace OneSet.ViewModels
{
    public class ExerciseItemViewModel
    {
        public Exercise Exercise { get; set; }
        public string TrainingDays { get; set; }
        public bool TrainingDaysVisible => !string.IsNullOrEmpty(TrainingDays);
        public bool NotesVisible => !string.IsNullOrEmpty(Exercise.Notes);
    }
}