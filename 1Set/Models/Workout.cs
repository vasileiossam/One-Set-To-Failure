using System;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using Set;

namespace Set.Models
{
	[Table("Workouts")]
	public class Workout
	{
		public Workout ()
		{
            Created = DateTime.Today;
		}

		[PrimaryKey, AutoIncrement]
		public int WorkoutId { get; set; }

		[Indexed]
		[ForeignKey(typeof(Exercise))]
        public int ExerciseId { get; set; }

		public DateTime Created { get; set; }
        public string Notes { get; set; }

        public int Reps { get; set; }
        public double Weight { get; set; }

        protected Workout _previousWorkout;
        [Ignore]
        public Workout PreviousWorkout
        {
            get
            {
                _previousWorkout = App.Database.WorkoutsRepository.GetPreviousWorkout(ExerciseId, Created);
                return _previousWorkout;
            }
            set
            {
                _previousWorkout = value;
            }
        }

		protected int _previousReps;
        public int PreviousReps
        {
            get
            {
                if (PreviousWorkout != null)
                {
                    _previousReps = PreviousWorkout.Reps;
                }
                else
                {
                    _previousReps = 0;
                }
                return _previousReps;
            }
            set
            {
                _previousReps = value;
            }
        }

		protected double _previousWeight;
        public double PreviousWeight
        {
            get
            {
                if (PreviousWorkout != null)
                {
                    _previousWeight = PreviousWorkout.Weight;
                }
                else
                {
                    _previousWeight = 0;
                }
                return _previousWeight;
            }
            set
            {
                _previousWeight = value;
            }
        }

		protected int? _targetReps;
		public int? TargetReps
		{
			get
			{
				if (_targetReps == null)
				{
					dynamic result = WorkoutRules.GetTargetWorkout (this);
					_targetReps = result.TargetReps;
				}
				return _targetReps;
			}
			set
			{
				_targetReps = value;
			}
		}

		protected double? _targetWeight;
		public double? TargetWeight
		{
			get
			{
				if (_targetWeight == null)
				{
					dynamic result = WorkoutRules.GetTargetWorkout (this);
					_targetWeight = result.TargetWeight;
				}
				return _targetWeight;
			}
			set
			{
				_targetWeight = value;
			}
		}

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
		public string RepsImage {
			get
			{
				if (Reps == 0)
					return "ic_fa_circle";
				return "ic_fa_circle";
			}
		}

		[Ignore]
		public string WeightImage {
			get
			{
				if (Weight == 0)
					return "ic_fa_circle_o";
				return "ic_fa_circle";
			}
		}
	}
}

