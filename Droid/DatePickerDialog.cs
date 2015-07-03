using System;
using Set.Models;
using System.Threading.Tasks;
using Set.Droid;
using Xamarin.Forms;
using System.IO;
using Android.App;

[assembly: Dependency (typeof (Set.Droid.DatePickerDialog))]

namespace Set.Droid
{
	public class DatePickerDialog : IDatePickerDialog
	{
		private EventHandler _callback;

		public DatePickerDialog ()
		{

		}

		public void Show(EventHandler callback)		
		{
			_callback = callback;
			var today = DateTime.Today;
			var dialog = new Android.App.DatePickerDialog(Forms.Context, OnDateSet, today.Year, today.Month - 1, today.Day);
			dialog.Show ();
		}

		private async void OnDateSet(object sender, Android.App.DatePickerDialog.DateSetEventArgs e)
		{
			await Task.Run (() => _callback (e.Date, null));
		}
	}
}

