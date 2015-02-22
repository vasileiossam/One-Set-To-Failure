using System;
using SQLite.Net.Attributes;

namespace Set.Models
{
	public class Set
	{
		public Set ()
		{
		}

		[PrimaryKey, AutoIncrement]
		public int SetID { get; set; }

		public int WorkoutID { get; set; }

		public int Weight {get; set; }
		public int Reps {get; set; }
	}
}

