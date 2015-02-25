using System;
using Set.Models;
using SQLite.Net;
using Set.Abstract;

namespace Set.Concrete
{
	public class ExerciseRepository : BaseRepository<Exercise>, IExercisesRepository
	{
        public ExerciseRepository(SQLiteConnection connection)
			: base(connection)
		{

		}

        public IEnumerable<Exercise> GetDayRoutine(DateTime date)
        {
            lock (locker)
            {
                string dayField;

                switch (date.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        dayField = "DoOnMon";
                        break;
                    case DayOfWeek.Tuesday:
                        dayField = "DoOnTue";
                        break;
                    case DayOfWeek.Wednesday:
                        dayField = "DoOnWed";
                        break;
                    case DayOfWeek.Thursday:
                        dayField = "DoOnThu";
                        break;
                    case DayOfWeek.Friday:
                        dayField = "DoOnFri";
                        break;
                    case DayOfWeek.Saturday:
                        dayField = "DoOnSat";
                        break;
                    case DayOfWeek.Sunday:
                        dayField = "DoOnSun";
                        break;
                }

                day | exercise  | RowNumber | cando <-- create this every time we create an exersise
                      <- when adding a rownumber should be + 1 for the day

                - get routine for day

                - get workouts for day

                    - merge them or link them?
                    


                var sql = string.Format(
                    @"SELECT *  
                            FROM Routines
                            INNER JOIN Exercises ON Exercises.ExerciseID = Routines.ExerciseID
                            WHERE Routines.DayOfWeek = {0}
                            ORDER BY Routines.RowNumber, Routines.ExerciseID
                    ", dayField); 

                    <--- or create another table with setings for a day -->
                          Exersiceid, sort numb and date and give the option to move exercises on a day.

                    <--- sort numb is not going to workig because an exercice is part of multiple days 
                    <--- maybe we need the routine after all

                - idea: ability to print a routine or weekly program. call it old school printout or export in a txt file.

                var routines = _connection.Query<Routine>(sql);


                
//				need to create workouts when not there?
//
//				get workouts for routine xxxx
//
//					http://forums.xamarin.com/discussion/20117/sqlite-net-extensions-and-sqliteconnection
//					http://www.codeproject.com/Articles/792883/Using-Sqlite-in-a-Xamarin-Android-Application-Deve
//					http://stackoverflow.com/questions/24665304/sqlite-net-extensions-example-on-xamarin-forms
//
//
//					///
//
//					Routine 
//					-> Workout 
//					-> Excerice 
//					-> Set
//
//					------
//
//					have a systemic routine? 
//
//					show workouts everything available and then the program
//
//					if previous date - 7 then show prgram else show what is saved?
//
//
//						- find active routines for the day
//							-if one then display extersises in routine and dont display routine header
//							- if more than one display first a header for routnine and then display extersies
//
//								- find workouts for the day
//									- merge them with routine
//									- what is left
//
//
//									- what i want
//									- display the program (what I have to do)
//									- display what I DID
//									- allow workout without routine
//
								//}
            }
        }
	}
}
