using System.Linq;
using AutoMapper;
using SQLite;

namespace Set.Models1
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
		public double PlateWeight {get; set; }
        
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



		[IgnoreMap]
        public byte[] Image { get; set; }

		[Ignore]
		[IgnoreMap]
		public double ConvertedPlateWeight 
		{
			get
			{
				return Units.GetWeight(PlateWeight);
			}
		}

	}
}

