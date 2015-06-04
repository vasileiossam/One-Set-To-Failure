using System;
using System.Linq;
using System.Threading.Tasks;
using Set.Models;
using Set.Resx;

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
                }
            }
        }

        public CalendarNotesViewModel()
            : base()
		{
			Title = AppResources.CommentTitle;
		}

		public async Task Load()
		{
			var repository = App.Database.CalendarRepository;
			var all = await repository.AllAsync ();
			var calendar = all.FirstOrDefault(x => x.Date == _date);

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

		protected override async Task OnSave () 
		{
			if (Validate ())
			{
				var calendar = new Calendar (){ CalendarId = CalendarId, Date = Date, Notes = Notes };
				await App.Database.CalendarRepository.SaveAsync(calendar);
				await Navigation.PopAsync();
			}
		}
	}
}

