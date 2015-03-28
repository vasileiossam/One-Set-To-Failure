﻿using System;
using SQLite.Net.Attributes;
using Set.Models;
using System.Collections.Generic;
using Set.Resx;
using Xamarin.Forms;
using Toasts.Forms.Plugin.Abstractions;
using Set.Localization;
using AutoMapper;
using System.Linq;

namespace Set.ViewModels
{
	public class CalendarNotesViewModel : BaseViewModel
	{
        public int CalendarId { get; set; }
        public string Notes { get; set; }

        protected DateTime _date;
        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                if (_date != value)
                {
                    _date = value;
                    OnPropertyChanged("Date");
					LoadNotes ();
                }
            }
        }

        public CalendarNotesViewModel()
            : base()
		{
			Title = AppResources.CommentTitle;
		}

		protected void LoadNotes()
		{
			var repository = App.Database.CalendarRepository;
			var calendar = repository.All.FirstOrDefault(x => x.Date == _date);

			if (calendar == null)
			{
				CalendarId = 0;
				Notes = string.Empty;
			} else
			{
				CalendarId = calendar.CalendarId;
				Notes = calendar.Notes;
			}
		}

		private bool Validate ()
		{
			return true;
		}

		protected override void OnSave () 
		{
			if (Validate ())
			{
				var calendar = new Calendar (){ CalendarId = CalendarId, Date = Date, Notes = Notes };
				App.Database.CalendarRepository.Save(calendar);
				Navigation.PopAsync();
			}
		}
	}
}
