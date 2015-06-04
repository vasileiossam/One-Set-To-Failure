using System.Collections.ObjectModel;

namespace Set.ViewModels
{
	public class PreferenceGroup : ObservableCollection<Preference>
	{
		public string Title { get; set; }
		public string Hint { get; set; }

		public bool IsHintVisible
		{
			get
			{
				return !string.IsNullOrEmpty (Hint);
			}
		}
	}
}

