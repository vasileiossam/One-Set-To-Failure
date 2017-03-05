using SQLite;

namespace OneSet.Models
{
	[Table("Exercises")]
	public class Exercise
	{
		[PrimaryKey, AutoIncrement]
		public int ExerciseId { get; set; }
        public string Name { get; set; }
	    public string Notes { get; set; }
		public double PlateWeight {get; set; }
	}
}

