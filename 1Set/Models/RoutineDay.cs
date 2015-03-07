using System;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

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

		protected Exercise _exercise;
		[Ignore]
		public Exercise Exercise
		{
			get
			{
				_exercise = App.Database.ExercisesRepository.Find(ExerciseId);
				return _exercise;
			}
			set
			{
				_exercise = value;
			}
		}

		[Ignore]
		public Workout Workout {get; set;}

		[Ignore]
		public string RepsImage {
			get
			{
				if (Workout.Reps == 0)
					return "ic_fa_chevron_circle_right";
				return "ic_fa_circle";
			}
		}

		[Ignore]
		public string WeightImage {
			get
			{
				if (Workout.Weight == 0)
					return "ic_fa_chevron_circle_right";
				return "ic_fa_circle";
			}
		}

	}
}

