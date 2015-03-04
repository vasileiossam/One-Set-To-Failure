using System;
using Set.Models;
using SQLite.Net;
using Set.Abstract;
using System.Collections.Generic;
using System.Linq;

namespace Set.Concrete
{
	public class WorkoutsRepository : BaseRepository<Workout>, IWorkoutsRepository
	{
		public WorkoutsRepository(SQLiteConnection connection)
			: base(connection)
		{

		}

		public List<Workout> GetWorkouts(DateTime date)
		{
			lock (_locker)
			{
				lock (_locker)
				{
					var sql = @"SELECT *  
                            FROM Workouts
                            WHERE Created = ?";

					return _connection.Query<Workout> (sql, date);
				}
			}
		}

        public Workout GetPreviousWorkout(int exerciseId, DateTime created)
        {
            var results = All.Where(x => (x.ExericeId == exerciseId) && (x.Created < created)).OrderDescBy(x => x.created); <-- TOP or SKIP
        }
    }
}