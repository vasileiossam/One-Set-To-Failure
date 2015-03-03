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
					return "+" + Increment.ToString ();
				} 
				else
				{
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

