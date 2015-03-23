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

                    var repository = App.Database.CalendarRepository;
                    var calendar = repository.Find(x => x == _date);
                    if (calendar != null)
                    {

                    }
                }
            }
        }

        public CalendarNotesViewModel()
            : base()
		{
            Title = AppResources.CalendarNotesTitle;
		}
	}
}

