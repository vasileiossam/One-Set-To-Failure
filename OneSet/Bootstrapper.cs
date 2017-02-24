using System;
using Autofac;
using AutoMapper;
using OneSet.Abstract;
using OneSet.Models;
using OneSet.Services;
using OneSet.ViewModels;
using System.Reflection;
using OneSet.Data;
using OneSet.Views;
using Xamarin.Forms;
using SQLite;

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
            // register database
            builder.Register(c => DependencyService.Get<ISQLite>().GetConnection()).As<SQLiteAsyncConnection>().SingleInstance();
            builder.RegisterType<Database>().SingleInstance();

            // register services
            builder.RegisterType<UnitsService>().As<IUnitsService>().SingleInstance();
            builder.RegisterType<WorkoutRules>().As<IWorkoutRules>().InstancePerLifetimeScope();
            builder.RegisterType<Statistics>().As<ΙStatistics>().InstancePerLifetimeScope();
            builder.RegisterType<Exporter>().As<IExporter>().InstancePerLifetimeScope();
            builder.RegisterType<NavigationService>().As<INavigationService>().InstancePerLifetimeScope();

            // register views
            builder.RegisterAssemblyTypes(typeof(AboutPage).GetTypeInfo().Assembly)
               .Where(t => t.Namespace == "OneSet.Views")
               .InstancePerLifetimeScope();

            // register view models
            builder.RegisterAssemblyTypes(typeof(WorkoutListViewModel).GetTypeInfo().Assembly)
               .Where(t => t.Namespace == "OneSet.ViewModels")
               .InstancePerLifetimeScope();

            // register repositories
            builder.RegisterAssemblyTypes(typeof(CalendarRepository).GetTypeInfo().Assembly)
               .Where(t => t.Namespace == "OneSet.Data" && t.Name.EndsWith("Repository"))
               .AsImplementedInterfaces()
               .SingleInstance();
            
        }

        public void Automapper()
		{
            Mapper.CreateMap<RoutineDay, RoutineDayViewModel> ();
            Mapper.CreateMap<Exercise, ExerciseDetailsViewModel> ();
            Mapper.CreateMap<ExerciseDetailsViewModel, Exercise> ();
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

