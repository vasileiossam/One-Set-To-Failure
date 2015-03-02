using System;
using System.Globalization;

namespace Set.Models
{
	public class Settings
	{
		public int? _isMetric;
		public int IsMetric 
		{ 
			get 
			{ 
				if (_isMetric == null) 
				{
					_isMetric = RegionInfo.CurrentRegion.IsMetric ? 1 : 0;
				}
				return (int) _isMetric;
			} 
			set {
				_isMetric = value;
			}
		}


        public RestTimer _restTimer;
        public RestTimer RestTimer
        {
            get
            {
                if (_restTimer == null)
                {
                    _restTimer = new RestTimer() { Seconds = 60 };
                }
                return _restTimer;
            }
            set
            {
                _restTimer = value;
            }
        }

		public Settings ()
		{

		}
	}
}

