using System;
using SQLite.Net.Attributes;
using System.Collections.Generic;
using SQLiteNetExtensions.Attributes;

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
        
        private int? _repsForWeightUp;
        public int? RepsForWeightUp 
        {
            get 
            {
                if (_repsForWeightUp == null)
                {
                    _repsForWeightUp = App.Settings.RepsForWeightUp;
                }
                return _repsForWeightUp;
            } 
            set
            {
                _repsForWeightUp = value;
            }
        }
        
        private int? _repsForWeightDn;
        public int? RepsForWeightDn
        {
            get 
            {
                if (_repsForWeightDn == null)
                {
                    _repsForWeightDn = App.Settings.RepsForWeightDn;
                }
                return _repsForWeightDn;
            } 
            set
            {
                _repsForWeightDn = value;
            }
        }
        
        private int? _startingReps;
        public int? StartingReps 
        {
            get 
            {
                if (_startingReps == null)
                {
                    _startingReps = App.Settings.StartingReps;
                }
                return _startingReps;
            } 
            set
            {
                _startingReps = value;
            }
        }
        
        private int? _restTimerId;
        public int? RestTimerId 
        {
            get 
            {
                if (_restTimerId == null)
                {
                    _restTimerId = App.Settings.RestTimer.RestTimerId;
                }
                return _restTimerId;
            } 
            set
            {
                _restTimerId = value;
            }
        }
        
        private int? _repsIncrementId;
        public int? RepsIncrementId 
        {
            get 
            {
                if (_repsIncrementId == null)
                {
                    _repsIncrementId = App.Settings.RepsIncrement.RepsIncrementId;
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
                return App.Database.RestTimer.FirstOrDefault(RestTimerId);
            }
        }

        [Ignore]
        public RepsIncrement RepsIncrement
        {
            get
            {
                return App.Database.RepsIncrements.FirstOrDefault(RepsIncrementId);
            }
        } 

        public byte[] Image { get; set; }
	}
}

