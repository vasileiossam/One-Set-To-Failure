using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using OneSet.Abstract;
using OneSet.Entities;
using SQLite;

namespace OneSet.Concrete
{
	public class RoutineDaysRepository : BaseRepository<RoutineDay>, IRoutineDaysRepository
	{
		public RoutineDaysRepository(SQLiteAsyncConnection connection)
			: base(connection)
		{

		}

		/// <summary>
		/// Returns the routine of the day
		/// </summary>
		/// <returns>The routine.</returns>
		/// <param name="date">Date.</param>
		public async Task<List<RoutineDay>> GetRoutine(DateTime date)
		{
//				var sql = @"SELECT *  
//                            FROM RoutineDays
//                            INNER JOIN Exercises ON Exercises.ExerciseId = RoutineDays.ExerciseId
//                            WHERE (RoutineDays.DayOfWeek = ?) AND (RoutineDays.IsActive = 1)
//                            ORDER BY RoutineDays.RowNumber, RoutineDays.ExerciseId";
//
//				return _connection.Query<RoutineDay> (sql, (int)date.DayOfWeek);

		//var k = await _connection.QueryAsync<RoutineDay> ("SELECT * FROM RoutineDays");
			try
			{
				var list = new List<RoutineDay> ();
				using (await Mutex.LockAsync ().ConfigureAwait (false))
				{
					list = await _connection.Table<RoutineDay> ()
						.Where (x => (x.DayOfWeek == (int)date.DayOfWeek) && (x.IsActive == 1)).ToListAsync ();
				}

				//await LoadExercisesAsync (list);
				//await LoadRelations(list, date);
				return list;
			}
			catch(Exception ex)
			{
			  Debug.WriteLine (ex.Message);
			}

			return null;
		}

		private async Task LoadRelations(List<RoutineDay> list, DateTime date)
		{	
			
		}

		public async Task<List<RoutineDay>> GetRoutine(int exerciseId)
		{
			var list = new List<RoutineDay> ();
			using (await Mutex.LockAsync ().ConfigureAwait (false))
			{			
				var sql = @"SELECT *  
                        FROM RoutineDays
                        WHERE (RoutineDays.ExerciseId = ?)
                        ORDER BY RoutineDays.DayOfWeek";

				list = await _connection.QueryAsync<RoutineDay> (sql, exerciseId);
				return list;
			}
		}

	}
}
