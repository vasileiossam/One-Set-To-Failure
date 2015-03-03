using System;
using System.Globalization;
using System.Collections.Generic;
using Set.Resx;

namespace Set.Models
{
	public class RestTimer
	{
        public int RestTimerId { get; set; }
        public int Seconds { get; set; }
        public string Description { get; set; }

		public RestTimer(int restTimerId, string description, int seconds)
		{
			RestTimerId = restTimerId;
			Seconds = seconds;
			Description = description;
		}
	}
}

