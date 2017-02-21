using System.Collections.ObjectModel;

namespace OneSet.ViewModels
{
	public class PreferenceGroup : ObservableCollection<Preference>
	{
		public string Title { get; set; }
		public string Hint { get; set; }

		public bool IsHintVisible => !string.IsNullOrEmpty (Hint);
	}
}

