using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Set.Abstract;
using Set.Models;
using SQLite.Net.Async;

namespace Set.Concrete
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

				await LoadExercisesAsync (list);
				await LoadRelations(list, date);
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
			var workouts = await App.Database.WorkoutsRepository.GetWorkouts(date);
				
			var canCalculateTarget = false;

			foreach (var day in list)
			{
				var workout = workouts.Find (x => x.ExerciseId == day.ExerciseId);

				// workout hasn't performed for this exercise
				if (workout == null)
				{
				    workout = new Workout
				    {
				        ExerciseId = day.ExerciseId,
				        Created = date
				    };
				    canCalculateTarget = true;
				} 

				day.Workout = workout;
				day.Workout.Exercise = day.Exercise;

				// to calculate target reps/weight
				if (canCalculateTarget) 
				{
					await workout.LoadAsync ();
				}

			}
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

		private async Task LoadExercisesAsync(List<RoutineDay> list)
		{
			if (list.Count == 0)
				return;
			
			using (await Mutex.LockAsync ().ConfigureAwait (false))
			{	
				var condition = string.Empty;

				foreach (var day in list)
				{
					condition = condition + day.ExerciseId + ",";
				}
				condition = condition.TrimEnd (',');

				var sql = string.Format(@"SELECT *  
                        FROM Exercises
                        WHERE ExerciseId IN ({0})", condition);
				var exercises = await _connection.QueryAsync<Exercise> (sql);

				foreach (var day in list)
				{
					day.Exercise = exercises.FirstOrDefault (x => x.ExerciseId == day.ExerciseId);
				}
			}
		}
	}
}
