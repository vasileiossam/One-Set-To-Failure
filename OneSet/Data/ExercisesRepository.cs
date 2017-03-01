using System;
using System.Threading.Tasks;
using OneSet.Abstract;
using OneSet.Models;
using SQLite;
using OneSet.Resx;
using System.Collections.Generic;
using System.Linq;

namespace OneSet.Data
{
    public class ExercisesRepository : BaseRepository<Exercise>, IExercisesRepository
    {
        public ExercisesRepository(SQLiteAsyncConnection connection)
            : base(connection)
        {

        }

        public override async Task<int> DeleteAsync(int id)
        {
            using (await Mutex.LockAsync().ConfigureAwait(false))
            {
                await _connection.ExecuteAsync("DELETE FROM RoutineDays WHERE ExerciseId = ?", id);
                await _connection.ExecuteAsync("DELETE FROM Workouts WHERE ExerciseId = ?", id);
                await _connection.ExecuteAsync("DELETE FROM Exercises WHERE ExerciseId = ?", id);
                return 0;
            }
        }

        public async Task<string> GetTrainingDays(Exercise exercise)
        {
            // get routine
            using (await Mutex.LockAsync().ConfigureAwait(false))
            {
                const string sql = @"SELECT *  
                        FROM RoutineDays
                        WHERE (RoutineDays.ExerciseId = ?)
                        ORDER BY RoutineDays.DayOfWeek";

                var routine = await _connection.QueryAsync<RoutineDay>(sql, exercise.ExerciseId);
                var dayNames = AppResources.Culture.DateTimeFormat.AbbreviatedDayNames;
                var list = new List<string>();

                // add active days
                foreach (var day in routine)
                {
                    if (day.IsActive != 1) continue;
                    var dayName = dayNames[day.DayOfWeek];
                    if (list.All(x => x != dayName))
                    {
                        list.Add(dayName);
                    }
                }

                if (list.Count == 7)
                {
                    return AppResources.AllWeekTraining;
                }

                var s = string.Join(", ", list);

                // replace last , with 'and'
                if (s == string.Empty) return s;
                var place = s.LastIndexOf(",", StringComparison.Ordinal);
                if (place >= 0)
                {
                    s = s.Remove(place, 1).Insert(place, " " + AppResources.And);
                }

                return s;
            }
        }
    }
}
