using System;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Set.Models
{
	[Table("RoutineDays")]
	public class RoutineDay
	{
		public RoutineDay ()
		{
		}

		[PrimaryKey, AutoIncrement]
		public int RoutineDayId { get; set; }

		[ForeignKey(typeof(Exercise))]
        public int ExerciseId { get; set; }        
	      
		public int DayOfWeek { get; set; }
        public int RowNumber { get; set; }
		public int IsActive {get; set;}

		[Ignore]
		public Exercise Exercise { get; set; }

		[Ignore]
		public Workout Workout {get; set;}

		[Ignore]
		public string StateImage {
			get
			{
				if ((Workout.Reps == 0) || (Workout.Weight == 0))
					return "ic_fa_play_circle_action";
				return "ic_fa_check_circle_o";
			}
		}

		[Ignore]
		public bool TrophyVisible {
			get
			{
				return Workout.Trophies != 0;
			}
		}

		// TODO move to viewmodel or page when this works https://forums.xamarin.com/discussion/25677/does-xamarin-forms-support-relativesource-on-a-binding
		protected StackOrientation _cellLayoutOrientation;
		[Ignore]
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

        // TODO move this to a new RoutineDayViewModel
        [Ignore]
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
        
        // TODO move this to a new RoutineDayViewModel
        [Ignore]
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
        
        // TODO move this to a new RoutineDayViewModel
        [Ignore]
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

