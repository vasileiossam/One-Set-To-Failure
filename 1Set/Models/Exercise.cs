using System;
using SQLite.Net.Attributes;

namespace Set.Models
{
	public class Exercise
	{
		public Exercise ()
		{
		}

		[PrimaryKey, AutoIncrement]
		public int ExerciseID { get; set; }
		public string Name { get; set; }
		public string Notes { get; set; }
		public byte[] Image { get; set; }
		public int Step {get; set; }
	}
}

