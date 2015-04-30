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
		public int RepsIncrementId { get; set; }
		public bool PreviousRepsWeightVisible { get; set; }
		public bool TargetRepsWeightVisible { get; set; }

		#region Rest Timer
		public bool RestTimerAutoStart { get; set; }
		public bool RestTimerPlaySounds { get; set; }
		public int RestTimerTotalSeconds { get; set; }
		#endregion

		public Settings()
		{
			// defaults
			IsMetric = RegionInfo.CurrentRegion.IsMetric;
			MaxReps = 12;
			MinReps = 5;
			RepsIncrementId = 1; // +1
			PreviousRepsWeightVisible = true;
			TargetRepsWeightVisible = true;

			#region Rest Timer defaults
			RestTimerAutoStart = true;
			RestTimerPlaySounds = true;
			RestTimerTotalSeconds = 90;
			#endregion
		}

		// exercise goal: how many reps to do more in every workout
		[JsonIgnore]
		public RepsIncrement RepsIncrement
		{
			get
			{
				var repsIncrement = App.Database.RepsIncrements.FirstOrDefault (x => x.RepsIncrementId == RepsIncrementId);
				if (repsIncrement == null)
				{
					repsIncrement = App.Database.RepsIncrements.FirstOrDefault (x => x.RepsIncrementId == 1);
				}
				return repsIncrement;
			}
		}

  

	}
}

