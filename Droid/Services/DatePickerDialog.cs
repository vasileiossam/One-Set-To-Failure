using System;
using System.Threading.Tasks;
using OneSet.Abstract;
using OneSet.Droid.Services;
using Xamarin.Forms;

[assembly: Dependency (typeof (DatePickerDialog))]

namespace OneSet.Droid.Services
{
	public class DatePickerDialog : IDatePickerDialog
	{
		private EventHandler _callback;

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

