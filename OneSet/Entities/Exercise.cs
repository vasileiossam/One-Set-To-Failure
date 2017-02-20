using AutoMapper;
using SQLite;

namespace OneSet.Entities
{
	[Table("Exercises")]
	public class Exercise
	{
		[PrimaryKey, AutoIncrement]
		public int ExerciseId { get; set; }
        public string Name { get; set; }
	    public string Notes { get; set; }
		public double PlateWeight {get; set; }
		public int? MaxReps { get; set; }
        public int? MinReps { get; set; }
        public int? RestTimerId { get; set; }
        public int? RepsIncrementId { get; set; }

        [IgnoreMap]
        public byte[] Image { get; set; }
	}
}

