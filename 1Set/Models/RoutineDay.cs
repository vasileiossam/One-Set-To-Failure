using System;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;
using System.Threading.Tasks;

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
	}
}

