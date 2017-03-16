using OneSet.Services;
using Xamarin.Forms;

namespace OneSet.Models
{
	public class RoutineItem : ObservableObject
    {
        #region properties
        public RoutineDay RoutineDay { get; set; }
        public Exercise Exercise { get; set; }

        protected Workout _workout;
        public Workout Workout
        {
            get { return _workout; }
            set
            {
                SetProperty(ref _workout, value);
                OnPropertyChanged("StateImage");
                OnPropertyChanged("TrophyVisible");
                OnPropertyChanged("LevelUpVisible");
                OnPropertyChanged("Reps");
                OnPropertyChanged("Weight");
                OnPropertyChanged("RepsWeightColor");
            }
        }

        public string StateImage => Workout == null ? "ic_fa_play_circle_action" : "ic_fa_check_circle_o";

        public bool TrophyVisible {
			get
			{
                if (Workout != null)
                    return Workout.Trophies != 0;
			    return false;
			}
		}

		public bool LevelUpVisible {
			get
			{
                if (Workout != null)
                    return Workout.TargetWeight > 0 && Workout.PreviousWeight > 0 && Workout.Weight > Workout.PreviousWeight;
			    return false;
			}
		}
			
        public int Reps => Workout?.Reps ?? TargetReps;
        public double Weight => Workout?.Weight ?? TargetWeight;
        public Color RepsWeightColor => Workout != null ? ColorPalette.SecondaryText : ColorPalette.Accent;
        public int PreviousReps { get; set; }
        public double PreviousWeight { get; set; }
        public int TargetReps { get; set; }
        public double TargetWeight { get; set; }

        // TODO: move all CellLayoutOrientation to their view model and use RelativeSource in the binding of the DataTemplate
        // RelativeSource is not supported yet: https://xamarin.uservoice.com/forums/258559-xamarin-forms-suggestions/suggestions/6451625-support-for-relativesource-in-xaml-binding
        protected StackOrientation _cellLayoutOrientation;
        public StackOrientation CellLayoutOrientation
        {
            get { return _cellLayoutOrientation; }
            set { SetProperty(ref _cellLayoutOrientation, value); }
        }
        #endregion
    }
}

