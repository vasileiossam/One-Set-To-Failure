using SQLite;

namespace OneSet.Entities
{
	[Table("RoutineDays")]
	public class RoutineDay
	{
		[PrimaryKey, AutoIncrement]
		public int RoutineDayId { get; set; }
        [Indexed]
        public int ExerciseId { get; set; }        
		public int DayOfWeek { get; set; }
        public int RowNumber { get; set; }
		public int IsActive {get; set;}
	}
}