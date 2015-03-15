using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;

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

