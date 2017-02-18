﻿using System;
using OneSet.Models;
using System.Threading.Tasks;
using OneSet.Droid;
using Xamarin.Forms;
using System.IO;
using Android.App;

[assembly: Dependency (typeof (OneSet.Droid.DatePickerDialog))]

namespace OneSet.Droid
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
