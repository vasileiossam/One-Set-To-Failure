using System;
using System.Linq;
using System.Threading.Tasks;
using OneSet.Models;
using OneSet.Resx;
using OneSet.Abstract;

namespace OneSet.ViewModels
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
                if (_date == value) return;
                _date = value;
                OnPropertyChanged("Date");
            }
        }

	    private readonly ICalendarRepository _repo;

        public CalendarNotesViewModel(ICalendarRepository repo)
            : base()
        {
            _repo = repo;
            Title = AppResources.CommentTitle;
		}

		public async Task Load()
		{
			var all = await _repo.AllAsync ();
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
				var calendar = new Calendar { CalendarId = CalendarId, Date = Date, Notes = Notes.Trim() };
				await _repo.SaveAsync(calendar);
				await Navigation.PopAsync();
			}
		}
	}
}

