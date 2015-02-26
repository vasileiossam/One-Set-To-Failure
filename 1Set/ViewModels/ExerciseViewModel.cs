using System;
using System.Collections.Generic;
using Set.Models;

namespace Set.ViewModels
{
	public class ExerciseViewModel : BaseViewModel
	{
		public Exercise Exercise {get; set; }
        
        public bool DoOnMon { get; set; }
        public bool DoOnTue { get; set; }
        public bool DoOnWed { get; set; }
        public bool DoOnThu { get; set; }
        public bool DoOnFri { get; set; }
        public bool DoOnSat { get; set; }
        public bool DoOnSun { get; set; }

		public ExerciseViewModel ()
		{
		}
	}
}

