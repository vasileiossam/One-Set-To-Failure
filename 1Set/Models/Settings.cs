using System;
using System.Globalization;

namespace Set.Models
{
	public class Settings
    {
        #region Defaults
        const int DefaultIsMetric = RegionInfo.CurrentRegion.IsMetric ? 1 : 0;
        const int DefaultRepsToAdvance = 12;
        const int DefaultStartingReps = 5;
        
        // 1 min 30secs
        const int DefaultRestTimerId = 4;
        
        // +1
        const int DefaultWorkoutGoalId = 1;
        #endregion

        public int? _isMetric;
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
        
        // total reps to advance to next weight
        // values 0, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18...50
        public int? _repsToAdvance;
        public int RepsToAdvance
        {
            get
            {
                if (_repsToAdvance == null)
                {
                    _repsToAdvance = DefaultRepsToAdvance;
                }
                return (int)_repsToAdvance;
            }
            set
            {
                _repsToAdvance = value;
            }
        }
        
        // reps when starting training on a weight for the first time
        // values 0..50
        public int? _startingReps;
        public int StartingReps
        {
            get
            {
                if (_startingReps == null)
                {
                    _startingReps = DefaultStartingReps;
                }
                return (int)_startingReps;
            }
            set
            {
                _startingReps = value;
            }
        }
        

        public RestTimer _restTimer;
        public RestTimer RestTimer
        {
            get
            {
                if (_restTimer == null)
                {
                    _restTimer = new RestTimer() { Seconds = 60 }; <- iid
                }
                return _restTimer;
            }
            set
            {
                _restTimer = value;
            }
        }
        #endregion

        public Settings ()
		{
		}
	}
}

