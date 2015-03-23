using System;
using SQLite.Net.Attributes;
using Set.Models;
using System.Collections.Generic;
using Set.Resx;
using Xamarin.Forms;
using Toasts.Forms.Plugin.Abstractions;
using Set.Localization;

namespace Set.ViewModels
{
	public class WorkoutCommentViewModel : BaseViewModel
	{
		public WorkoutCommentViewModel (): base()
		{
			Title = AppResources.WorkoutNotesTitle;
		}
	}
}

