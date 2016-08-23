using System;
using AutoMapper;
using Set.Models;
using Set.ViewModels;
using Set.Entities;

namespace Set
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

