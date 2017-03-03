using Autofac;
using OneSet.Abstract;
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
            builder.RegisterType<DialogService>().As<IDialogService>().InstancePerLifetimeScope();
            builder.RegisterType<MessagingService>().As<IMessagingService>().InstancePerLifetimeScope();
            builder.Register(c => DependencyService.Get<ISettingsStorage>()).As<ISettingsStorage>().InstancePerLifetimeScope();
            builder.Register(c => DependencyService.Get<ITextStorage>()).As<ITextStorage>().InstancePerLifetimeScope();
            builder.Register(c => DependencyService.Get<IDatePickerDialog>()).As<IDatePickerDialog>().InstancePerLifetimeScope();
            builder.Register(c => DependencyService.Get<ISoundService>()).As<ISoundService>().InstancePerLifetimeScope();
            builder.Register(c => DependencyService.Get<IBackupRestoreService>()).As<IBackupRestoreService>().InstancePerLifetimeScope();
            
            // register views
            builder.RegisterAssemblyTypes(typeof(AboutPage).GetTypeInfo().Assembly)
               .Where(t => t.Namespace == "OneSet.Views")
               .InstancePerDependency();

            // register view models
            builder.RegisterAssemblyTypes(typeof(MainViewModel).GetTypeInfo().Assembly)
               .Where(t => t.Namespace == "OneSet.ViewModels")
               .InstancePerDependency();

            // register repositories
            builder.RegisterAssemblyTypes(typeof(CalendarRepository).GetTypeInfo().Assembly)
               .Where(t => t.Namespace == "OneSet.Data" && t.Name.EndsWith("Repository"))
               .AsImplementedInterfaces()
               .SingleInstance();
            
        }
	}
}

