using System;
using System.Globalization;
using System.Linq;

namespace Set.Models
{
	public class Settings
    {
        #region Defaults
        int DefaultIsMetric = RegionInfo.CurrentRegion.IsMetric ? 1 : 0;
		const int DefaultMaxReps = 12;
		const int DefaultMinReps = 5;
        
        // 1 min 30secs
        const int DefaultRestTimerId = 4;
        
        // +1
        const int DefaultWorkoutGoalId = 1;
        #endregion

        protected int? _isMetric;
		public int IsMetric 
		{ 
			get 
			{ 
				if (_isMetric == null) 
				{
                    _isMetric = DefaultIsMetric;
				}
				return (int) _isMetric;
			} 
			set {
				_isMetric = value;
			}
		}

        #region Settings that can be overriden in Exercises 
        
        // if you do  more than this Reps you will advance to next weight
        // values 0, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18...50
		protected int? _maxReps;
		public int MaxReps
        {
            get
            {
				if (_maxReps == null)
                {
					_maxReps = DefaultMaxReps;
                }
				return (int)_maxReps;
            }
            set
            {
				_maxReps = value;
            }
        } 

        // if you do less than this Reps you will go to previous weight
        // values 0, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18...50
        protected int? _minReps;
        public int MinReps
        {
            get
            {
				if (_minReps == null)
                {
					_minReps = DefaultMinReps;
                }
				return (int)_minReps;
            }
            set
            {
				_minReps = value;
            }
        }

        protected RestTimer _restTimer;
        public RestTimer RestTimer
        {
            get
            {
                if (_restTimer == null)
                {
					_restTimer = App.Database.RestTimers.FirstOrDefault (x => x.Seconds == 60);
                }
                return _restTimer;
            }
            set
            {
                _restTimer = value;
            }
        }

		// workout goal: how many reps to do more in every workout
        protected RepsIncrement _repsIncrement;
		public RepsIncrement RepsIncrement
		{
			get
			{
				if (_repsIncrement == null)
				{
                    _repsIncrement = App.Database.RepsIncrements.FirstOrDefault(x => (x.WorkoutCount == 0) && (x.Increment == 1));
				}
				return _repsIncrement;
			}
			set
			{
				_repsIncrement = value;
			}
		}
        #endregion

        public Settings ()
		{
		}
	}
}

