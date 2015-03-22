using System;
using Set.Models;
using SQLite.Net;
using Set.Abstract;
using System.Collections.Generic;
using System.Linq;

namespace Set.Concrete
{
	public class RoutineDaysRepository : BaseRepository<RoutineDay>, IRoutineDaysRepository
	{
		public RoutineDaysRepository(SQLiteConnection connection)
			: base(connection)
		{

		}

		/// <summary>
		/// Returns the routine of the day
		/// </summary>
		/// <returns>The routine.</returns>
		/// <param name="date">Date.</param>
		public List<RoutineDay> GetRoutine(DateTime date)
		{
			lock (_locker)
			{
//				var sql = @"SELECT *  
//                            FROM RoutineDays
//                            INNER JOIN Exercises ON Exercises.ExerciseId = RoutineDays.ExerciseId
//                            WHERE (RoutineDays.DayOfWeek = ?) AND (RoutineDays.IsActive = 1)
//                            ORDER BY RoutineDays.RowNumber, RoutineDays.ExerciseId";
//
//				return _connection.Query<RoutineDay> (sql, (int)date.DayOfWeek);

				return All.Where (x => (x.DayOfWeek == (int)date.DayOfWeek) && (x.IsActive == 1)).ToList ();
			}
		}

		public List<RoutineDay> GetRoutine(int exerciseId)
		{
			lock (_locker)
			{
				var sql = @"SELECT *  
                            FROM RoutineDays
                            WHERE (RoutineDays.ExerciseId = ?)
                            ORDER BY RoutineDays.DayOfWeek";

				return _connection.Query<RoutineDay> (sql, exerciseId);
			}
		}
	}
}
