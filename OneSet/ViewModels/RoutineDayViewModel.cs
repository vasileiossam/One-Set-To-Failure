using OneSet.Entities;
using Xamarin.Forms;

namespace OneSet.ViewModels
{
	public class RoutineDayViewModel
    {
		public RoutineDayViewModel()
		{
		}

		public int RoutineDayId { get; set; }
        public int ExerciseId { get; set; }        
		public int DayOfWeek { get; set; }
        public int RowNumber { get; set; }
		public int IsActive {get; set;}
 
		public Exercise Exercise { get; set; }
		public Workout Workout {get; set;}

		public string StateImage {
			get
			{
				if ((Workout.Reps == 0) || (Workout.Weight == 0))
					return "ic_fa_play_circle_action";
				return "ic_fa_check_circle_o";
			}
		}

		public bool TrophyVisible {
			get
			{
				return Workout.Trophies != 0;
			}
		}

		public bool LevelUpVisible {
			get
			{
				return (Workout.TargetWeight > 0) && (Workout.PreviousWeight > 0) && (Workout.Weight > Workout.PreviousWeight);
			}
		}
			
		// TODO move to viewmodel or page when this works https://forums.xamarin.com/discussion/25677/does-xamarin-forms-support-relativesource-on-a-binding
		protected StackOrientation _cellLayoutOrientation;
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
                if (Workout != null)
                {
                    if (Workout.WorkoutId > 0)
                    {
                        return Workout.Reps;
                    }
                    else
                    {
                        return Workout.TargetReps;
                    }
                }
                else
                {
                    return 0;
                }
            }
        }
        
        public double Weight
        {
            get
            {
                if (Workout != null)
                {
                    if (Workout.WorkoutId > 0)
                    {
                        return Workout.Weight;
                    }
                    else
                    {
                        return Workout.TargetWeight;
                    }
                }
                else
                {
                    return 0;
                }
            }
        }

        public Color RepsWeightColor
        {
            get
            {
                if (Workout != null)
                {
                    if (Workout.WorkoutId > 0)
                    {
                        return ColorPalette.SecondaryText; 
                    }
                    else
                    {
                        return ColorPalette.Accent;
                    }
                }
                else
                {
                    return ColorPalette.SecondaryText;
                }
            }
        }
	}
}

