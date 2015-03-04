using System;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

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

        public Workout _previousWorkout;
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

        public int _previousReps;
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

        public double _previousWeight;
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

        TargetReps
        TargetWeight

		[OneToOne]		      
		public Exercise Exercise { get; set; }
	}
}

