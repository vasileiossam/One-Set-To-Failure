using AutoMapper;
using OneSet.Entities;
using Xamarin.Forms;

namespace OneSet.ViewModels
{
	public class RoutineDayViewModel
    {
        public int RoutineDayId { get; set; }
        public int ExerciseId { get; set; }        
		public int DayOfWeek { get; set; }
        public int RowNumber { get; set; }
		public int IsActive {get; set;}

        [IgnoreMap]
        public Exercise Exercise { get; set; }

        [IgnoreMap]
        public Workout Workout {get; set;}

		public string StateImage {
			get
			{
                if (Workout != null && (Workout.Reps == 0 || Workout.Weight == 0))
					return "ic_fa_play_circle_action";
				return "ic_fa_check_circle_o";
			}
		}

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
                    return (Workout.TargetWeight > 0) && (Workout.PreviousWeight > 0) && (Workout.Weight > Workout.PreviousWeight);
			    return false;
			}
		}
			
		// TODO move to viewmodel or page when this works https://forums.xamarin.com/discussion/25677/does-xamarin-forms-support-relativesource-on-a-binding
		protected StackOrientation _cellLayoutOrientation;
        [IgnoreMap]
        public StackOrientation CellLayoutOrientation
		{
			get
			{
				return _cellLayoutOrientation;
			}
			set
			{
				if (_cellLayoutOrientation != value)
				{
					_cellLayoutOrientation = value;
					//OnPropertyChanged ("CellLayoutOrientation");
				}
			}
		}

        public int Reps
        {
            get
            {
                if (Workout == null) return 0;
                return Workout.WorkoutId > 0 ? Workout.Reps : Workout.TargetReps;
            }
        }
        
        public double Weight
        {
            get
            {
                if (Workout == null) return 0;
                return Workout.WorkoutId > 0 ? Workout.Weight : Workout.TargetWeight;
            }
        }

        public Color RepsWeightColor
        {
            get
            {
                if (Workout == null) return ColorPalette.SecondaryText;
                return Workout.WorkoutId > 0 ? ColorPalette.SecondaryText : ColorPalette.Accent;
            }
        }
	}
}

