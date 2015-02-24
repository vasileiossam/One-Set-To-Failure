using System;
using SQLite.Net.Attributes;

namespace Set.Models
{
	public class Routine
	{
        [PrimaryKey, AutoIncrement]
        public int RoutineID { get; set; }
        public string Title {get; set; }
        public string Notes { get; set; }
        
        public int IsActive { get; set; }
        public int IsActiveOnMon { get; set; }
        public int IsActiveOnTue { get; set; }
        public int IsActiveOnWed { get; set; }
        public int IsActiveOnThu { get; set; }
        public int IsActiveOnFri { get; set; }
        public int IsActiveOnSat { get; set; }
        public int IsActiveOnSun { get; set; }

        public Workouts

        public Routine ()
		{

		}
	}
}

