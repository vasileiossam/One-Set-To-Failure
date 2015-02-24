using System;
using System.Globalization;

namespace Set.Models
{
	public class Settings
	{
		public int? _isMetric;
		public int? IsMetric 
		{ 
			get 
			{ 
				if (_isMetric == null) 
				{
					_isMetric = RegionInfo.CurrentRegion.IsMetric ? 1 : 0;
				}
				return _isMetric;
			} 
			set {
				_isMetric = value;
			}
		}

		public Settings ()
		{

		}
	}
}

