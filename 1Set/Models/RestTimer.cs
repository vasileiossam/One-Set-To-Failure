using System;
using System.Globalization;
using System.Collections.Generic;
using Set.Resx;

namespace Set.Models
{
	public class RestTimer
	{
		public string Description {get; set;}
        public int Seconds { get; set; }

        public static List<RestTimer> All
        {
            get
            {
                var list = new List<RestTimer>();
				list.Add(new RestTimer() { Description = AppResources.ZeroSecsRestTimerDescription, Seconds = 0 });
                
                list.Add(new RestTimer() { Description = "30 " + AppResources.SecondsRestTimerDescription, Seconds = 30 });
                list.Add(new RestTimer() { Description = "45 " + AppResources.SecondsRestTimerDescription, Seconds = 45 });
                list.Add(new RestTimer() { Description = "1 " + AppResources.MinuteRestTimerDescription, Seconds = 60 });
                list.Add(new RestTimer() { Description = "1 " + AppResources.MinuteRestTimerDescription + " 30 " + AppResources.SecondsRestTimerDescription, Seconds = 60 + 30 });
                list.Add(new RestTimer() { Description = "1 " + AppResources.MinuteRestTimerDescription + " 45 " + AppResources.SecondsRestTimerDescription, Seconds = 60 + 45 });
                list.Add(new RestTimer() { Description = "2 " + AppResources.MinutesRestTimerDescription, Seconds = 2 * 60 });
                list.Add(new RestTimer() { Description = "2 " + AppResources.MinutesRestTimerDescription + " 30 " + AppResources.SecondsRestTimerDescription, Seconds = 2 * 60 + 30 });                

                return list;
            }
        }
	}
}

