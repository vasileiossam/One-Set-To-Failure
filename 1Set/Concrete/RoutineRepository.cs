using System;
using Set.Models;
using SQLite.Net;
using Set.Abstract;

namespace Set.Concrete
{
	public class RoutineRepository : BaseRepository<Routine>, IRoutineRepository
	{
        public RoutineRepository(SQLiteConnection connection)
			: base(connection)
		{

		}

        public IEnumerable<Routine> GetActiveRoutines(DateTime date)
        {
            lock (locker)
            {
                string activeDayFieldName;
                
                switch (date.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        activeDayFieldName = "IsActiveOnMon";
                        break;
                    case DayOfWeek.Tuesday:
                        activeDayFieldName = "IsActiveOnTue";
                        break;
                    case DayOfWeek.Wednesday:
                        activeDayFieldName = "IsActiveOnWed";
                        break;
                    case DayOfWeek.Thursday:
                        activeDayFieldName = "IsActiveOnThu";
                        break;
                    case DayOfWeek.Friday:
                        activeDayFieldName = "IsActiveOnFri";
                        break;
                    case DayOfWeek.Saturday:
                        activeDayFieldName = "IsActiveOnSat";
                        break;
                    case DayOfWeek.Sunday:
                        activeDayFieldName = "IsActiveOnSun";
                        break;
                }

                var sql = string.Format("SELECT * FROM [Routines] WHERE ([IsActive] = 1) AND ({0} = 1) ORDER BY RoutineID", activeDayFieldName);
                var routines = _connection.Query<Routine>(sql);
                

                need to create workouts when not there?

                get workouts for routine xxxx

                    http://forums.xamarin.com/discussion/20117/sqlite-net-extensions-and-sqliteconnection
                    http://www.codeproject.com/Articles/792883/Using-Sqlite-in-a-Xamarin-Android-Application-Deve
                    http://stackoverflow.com/questions/24665304/sqlite-net-extensions-example-on-xamarin-forms

                    
        ///

        Routine 
            -> Workout 
               -> Excerice 
                  -> Set
        
                      ------

        have a systemic routine? 

        show workouts everything available and then the program

            if previous date - 7 then show prgram else show what is saved?


                - find active routines for the day
                    -if one then display extersises in routine and dont display routine header
                    - if more than one display first a header for routnine and then display extersies
                
                - find workouts for the day
                    - merge them with routine
                    - what is left


    - what i want
         - display the program (what I have to do)
         - display what I DID
         - allow workout without routine

            }
        }


	}
}
