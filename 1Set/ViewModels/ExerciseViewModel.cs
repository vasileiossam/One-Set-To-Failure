using System;
using System.Collections.Generic;
using Set.Models;
using System.Collections.ObjectModel;

namespace Set.ViewModels
{
	public class ExerciseViewModel : BaseViewModel
	{
		private Exercise _exercise;
		public Exercise Exercise
		{
			get
			{
				if (_exercise == null)
				{
					_exercise = new Exercise();
				}
				return _exercise;
			}
			set
			{
				_exercise = value;
			}
		}

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

