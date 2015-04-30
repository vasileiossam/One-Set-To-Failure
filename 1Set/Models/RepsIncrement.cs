using System;
using Set.Resx;

namespace Set.Models
{
	public class RepsIncrement
	{
		public int RepsIncrementId { get; set;}
		public int Increment { get; set; }
		public int WorkoutCount { get; set; }

		public string Description 
		{ 
			get 
			{
                if (WorkoutCount == 0)
                {
                    return AppResources.RepsIncrementDisabledDescr;
                }
                else
				if (WorkoutCount == 1)
				{
                    return string.Format(AppResources.RepsIncrementZeroWorkCountDescr, Increment);
				} 
				else
				{
                    if (WorkoutCount == 2)
                    {
                        return string.Format(AppResources.RepsIncrementEveryOtherTimeDescription, Increment);
                    }
                        
					return string.Format (AppResources.RepsIncrementDescription, Increment, WorkoutCount);
				}
			} 
		}

		public RepsIncrement (int repsIncrementId, int increment, int workoutCount )
		{
			RepsIncrementId = repsIncrementId;
			Increment = increment;
			WorkoutCount = workoutCount;
		}
	}
}

