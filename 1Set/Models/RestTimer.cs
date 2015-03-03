using System;
using System.Globalization;
using System.Collections.Generic;
using Set.Resx;

namespace Set.Models
{
    [Table("RestTimers")]                     xx xx xx xx xx xx   <----------------- table or not? need an id
	public class RestTimer
	{
        [PrimaryKey, AutoIncrement]
        public int RestTimerId { get; set; }
        public int Seconds { get; set; }

        public string Description { get; set; }

        /// <summary>
        //  hide rest timer
        //  30 secs
        //  45 secs
        //  1 min
        //  1 min 30 secs
        //  1 min 45 secs
        //  2 min
        //  2 min 30 secs
        /// </summary>
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

