using System;
using AutoMapper;
using OneSet.Entities;
using OneSet.ViewModels;
using OneSet.Models;

namespace OneSet
{
	public class Bootstrapper
	{
		public void Automapper()
		{
            	Mapper.CreateMap<RoutineDay, RoutineDayViewModel> ();
            //	Mapper.CreateMap<Exercise, ExerciseViewModel> ();
            //Mapper.CreateMap<ExerciseViewModel, Exercise> ();
        }

        public void CheckMapper()
		{
			try
			{
				Mapper.AssertConfigurationIsValid ();
			} catch (Exception ex)
			{
				App.ShowErrorPage (this, ex);
			}
		}
	}
}

