using System;
using SQLite.Net.Attributes;

namespace Set.ViewModels
{
    public class WorkoutViewModel : BaseViewModel
    {
        public IEnumerable<Workout> Workouts {get; set; }
    }
}

