using System;
using SQLite.Net.Attributes;
using System.Collections.Generic;
using SQLiteNetExtensions.Attributes;
using System.Linq;
using AutoMapper;

namespace Set.Models
{
	[Table("Exercises")]
	public class Exercise
	{
		public Exercise ()
		{
			Name = string.Empty;
			Notes = string.Empty;
		}

		[PrimaryKey, AutoIncrement]
		public int ExerciseId { get; set; }
        public string Name { get; set; }
	    public string Notes { get; set; }
		public float PlateWeight {get; set; }
        
        #region settings with global defaults
        
		protected int? _maxReps;
		[IgnoreMap]
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
		[IgnoreMap]
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
		[IgnoreMap]
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
		[IgnoreMap]
        public int? RepsIncrementId 
        {
            get 
            {
				if (_repsIncrementId == null)
				{
					_repsIncrementId = 1; 
				} else if (_repsIncrementId == 0)
				{
					_repsIncrementId = 1;
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
		[IgnoreMap]
        public RestTimer RestTimer
        {
            get
            {
				return App.Database.RestTimers.FirstOrDefault(x => x.RestTimerId == RestTimerId);
            }
        }

        [Ignore]
		[IgnoreMap]
		public RepsIncrement RepsIncrement
        {
            get
            {
				var repsIncrement = App.Database.RepsIncrements.FirstOrDefault(x=> x.RepsIncrementId == RepsIncrementId);
				return repsIncrement;
            }
        } 

		[IgnoreMap]
        public byte[] Image { get; set; }


	}
}

