using System;
using SQLite.Net.Attributes;
using Set.Models;
using System.Collections.Generic;

namespace Set.ViewModels
{
    public class WorkoutViewModel : BaseViewModel
    {
		public IEnumerable<Workout> Workouts {get; set; }
    }
}

