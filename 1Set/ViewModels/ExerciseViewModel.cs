using System;
using System.Collections.Generic;
using Set.Models;
using System.Collections.ObjectModel;

namespace Set.ViewModels
{
	public class ExerciseViewModel : BaseViewModel
	{
		private List<RoutineDay> _routineDays;

		private Exercise _exercise;
		public Exercise Exercise
		{
			get
			{
				if (_exercise == null)
				{
					_exercise = new Exercise();
				}
				return _exercise;
			}
			set
			{
				_exercise = value;
			}
		}

        public bool DoOnMon { get; set; }
        public bool DoOnTue { get; set; }
        public bool DoOnWed { get; set; }
        public bool DoOnThu { get; set; }
        public bool DoOnFri { get; set; }
        public bool DoOnSat { get; set; }
        public bool DoOnSun { get; set; }

		public ExerciseViewModel ()
		{
			_routineDays = new List<RoutineDay>();
		}

		public void LoadRoutine()
		{
			_routineDays = App.Database.RoutineDaysRepository.GetRoutine(Exercise);

			DoOnMon = _routineDays.Exists (x => (x.DayOfWeek == 1) && (x.IsActive == 1));
			DoOnTue = _routineDays.Exists (x => (x.DayOfWeek == 2) && (x.IsActive == 1));
			DoOnWed = _routineDays.Exists (x => (x.DayOfWeek == 3) && (x.IsActive == 1));
			DoOnThu = _routineDays.Exists (x => (x.DayOfWeek == 4) && (x.IsActive == 1));
			DoOnFri = _routineDays.Exists (x => (x.DayOfWeek == 5) && (x.IsActive == 1));
			DoOnSat = _routineDays.Exists (x => (x.DayOfWeek == 6) && (x.IsActive == 1));
			DoOnSun = _routineDays.Exists (x => (x.DayOfWeek == 0) && (x.IsActive == 1));
		}

		public void Save()
		{
			App.Database.ExercisesRepository.Save(Exercise);
			SaveRoutine ();
		}

		private void SaveRoutine()
		{
			// insert
			if (_routineDays.Count == 0)
			{
				CreateRoutineDay(Exercise.ExerciseId, 1, DoOnMon);
				CreateRoutineDay(Exercise.ExerciseId, 2, DoOnTue);
				CreateRoutineDay(Exercise.ExerciseId, 3, DoOnWed);
				CreateRoutineDay(Exercise.ExerciseId, 4, DoOnThu);
				CreateRoutineDay(Exercise.ExerciseId, 5, DoOnFri);
				CreateRoutineDay(Exercise.ExerciseId, 6, DoOnSat);
				CreateRoutineDay(Exercise.ExerciseId, 0, DoOnSun);
			} 
			// update
			else
			{
				foreach (var routineDay in _routineDays)
				{
					routineDay.IsActive = GetActive (routineDay.DayOfWeek);
					App.Database.RoutineDaysRepository.Save(routineDay);
				}
			}
		}

		public void CreateRoutineDay(int exerciseId, int dayOfWeek, bool isActive)
		{
			var routineDay = new RoutineDay()
			{
				ExerciseId = exerciseId,
				DayOfWeek = dayOfWeek,
				RowNumber = 1, 
				IsActive = isActive ? 1 : 0 
			};

			App.Database.RoutineDaysRepository.Save(routineDay);
		}

		public int GetActive(int dayOfWeek)
		{
			bool day = false;

			switch (dayOfWeek)
			{
				case 1: 
					day = DoOnMon;
					break;
				case 2: 
					day = DoOnTue;
					break;
				case 3: 
					day = DoOnWed;
					break;
				case 4: 
					day = DoOnThu;
					break;
				case 5: 
					day = DoOnFri;
					break;
				case 6: 
					day = DoOnSat;
					break;
				case 0: 
					day = DoOnSun;
					break;
			}

			if (day) return 1;
			return 0;
		}
	}
}

