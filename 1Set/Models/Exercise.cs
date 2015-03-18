using System;
using SQLite.Net.Attributes;
using System.Collections.Generic;
using SQLiteNetExtensions.Attributes;
using System.Linq;

namespace Set.Models
{
	[Table("Exercises")]
	public class Exercise
	{
		public Exercise ()
		{

		}

		[PrimaryKey, AutoIncrement]
		public int ExerciseId { get; set; }
		
        public string Name { get; set; }
	    public string Notes { get; set; }
		public float PlateWeight {get; set; }
        
        #region settings with global defaults
        
		protected int? _maxReps;
		public int? MaxReps 
        {
            get 
            {
				if (_maxReps == null)
                {
					_maxReps = App.Settings.MaxReps;
                }
				return _maxReps;
            } 
            set
            {
				_maxReps = value;
            }
        }
        
		protected int? _minReps;
		public int? MinReps
        {
            get 
            {
				if (_minReps == null)
                {
					_minReps = App.Settings.MinReps;
                }
				return _minReps;
            } 
            set
            {
				_minReps = value;
            }
        }
        
		protected int? _restTimerId;
        public int? RestTimerId 
        {
            get 
            {
                if (_restTimerId == null)
                {
					_restTimerId = 0; // App.Settings.RestTimer.RestTimerId;
                }
                return _restTimerId;
            } 
            set
            {
                _restTimerId = value;
            }
        }
        
		protected int? _repsIncrementId;
        public int? RepsIncrementId 
        {
            get 
            {
                if (_repsIncrementId == null)
                {
					_repsIncrementId = 0; //App.Settings.RepsIncrement.RepsIncrementId;
                }
                return _repsIncrementId;
            } 
            set
            {
                _repsIncrementId = value;
            }
        }
        #endregion

        [Ignore]
        public RestTimer RestTimer
        {
            get
            {
				return App.Database.RestTimers.FirstOrDefault(x => x.RestTimerId == RestTimerId);
            }
        }

        [Ignore]
        public RepsIncrement RepsIncrement
        {
            get
            {
				return App.Database.RepsIncrements.FirstOrDefault(x=> x.RepsIncrementId == RepsIncrementId);
            }
        } 

        public byte[] Image { get; set; }


	}
}

