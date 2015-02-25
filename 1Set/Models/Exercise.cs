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
		public string Title { get; set; }
		public string Notes { get; set; }
		public byte[] Image { get; set; }
		public float Step {get; set; }

		public int DoOnMon { get; set; }
		public int DoOnTue { get; set; }
		public int DoOnWed { get; set; }
		public int DoOnThu { get; set; }
		public int DoOnFri { get; set; }
		public int DoOnSat { get; set; }
		public int DoOnSun { get; set; }
	}
}

