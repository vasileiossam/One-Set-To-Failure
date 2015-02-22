using System;
using Set.Abstract;
using Xamarin.Forms;
using System.Globalization;

namespace Set
{
	public class App : Application
	{
//		static Database database;
//		public static TodoItemDatabase Database {
//			get { 
//				if (database == null) {
//					database = new TodoItemDatabase ();
//				} 
//				return database; 
//			}
//		}

		public App ()
		{
			L10n.SetLocale ();

			var netLanguage = DependencyService.Get<ILocale>().GetCurrent();
		//--\\	AppResources.Culture = new CultureInfo (netLanguage);

//--			var mainNav = new NavigationPage (new TodoListPage ());

	//--		MainPage = mainNav;
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}