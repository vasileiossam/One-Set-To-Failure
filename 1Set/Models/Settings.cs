using System;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;

namespace Set.Models
{
	public class Settings
    {
		public bool IsMetric { get; set; }
		public int MaxReps { get; set; }
		public int MinReps { get; set; }
		public int RestTimerId { get; set; }
		public int RepsIncrementId { get; set; }
		public bool PreviousRepsWeightVisible { get; set; }
		public bool TargetRepsWeightVisible { get; set; }


		public Settings()
		{
			// defaults
			IsMetric = RegionInfo.CurrentRegion.IsMetric;
			MaxReps = 12;
			MinReps = 5;
			RestTimerId = 4; // 1 min 30secs
			RepsIncrementId = 1; // +1
			PreviousRepsWeightVisible = true;
			TargetRepsWeightVisible = true;
		}

		[JsonIgnore]
        public RestTimer RestTimer
        {
            get
            {
				return App.Database.RestTimers.FirstOrDefault (x => x.RestTimerId == RestTimerId);
            }
        }

		// exercise goal: how many reps to do more in every workout
		[JsonIgnore]
		public RepsIncrement RepsIncrement
		{
			get
			{
				return App.Database.RepsIncrements.FirstOrDefault (x => x.RepsIncrementId == RepsIncrementId);
			}
		}

  

	}
}

