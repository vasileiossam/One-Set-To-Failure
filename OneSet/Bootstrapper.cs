using System;
using Autofac;
using AutoMapper;
using OneSet.Abstract;
using OneSet.Models;
using OneSet.Services;
using OneSet.ViewModels;
using System.Reflection;
using OneSet.Data;

namespace OneSet
{
	public class Bootstrapper
	{
        public IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();
            RegisterDependencies(builder);
            return builder.Build();
        }

        protected virtual void RegisterDependencies(ContainerBuilder builder)
        {
            // register services
            builder.RegisterType<UnitsService>().As<IUnitsService>().SingleInstance();
            builder.RegisterType<WorkoutRules>().As<IWorkoutRules>().SingleInstance();
            builder.RegisterType<Statistics>().As<ΙStatistics>().SingleInstance();
            builder.RegisterType<Exporter>().As<IExporter>().SingleInstance();
            builder.RegisterType<Database>().SingleInstance();

            // register view views
            builder.RegisterAssemblyTypes(typeof(Database).GetTypeInfo().Assembly)
               .Where(t => t.Namespace == " OneSet.Views")
               .InstancePerRequest();

            // register view models
            builder.RegisterAssemblyTypes(typeof(Database).GetTypeInfo().Assembly)
               .Where(t => t.Namespace == " OneSet.ViewModels")
               .InstancePerRequest();

            // register repositories
            builder.RegisterAssemblyTypes(typeof(Database).GetTypeInfo().Assembly)
               .Where(t => t.Namespace == " OneSet.Data" && t.Name.EndsWith("Repository"))
               .AsImplementedInterfaces()
               .SingleInstance();
            
        }

        public void Automapper()
		{
            Mapper.CreateMap<RoutineDay, RoutineDayViewModel> ();
            Mapper.CreateMap<Exercise, ExerciseViewModel> ();
            Mapper.CreateMap<ExerciseViewModel, Exercise> ();
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

