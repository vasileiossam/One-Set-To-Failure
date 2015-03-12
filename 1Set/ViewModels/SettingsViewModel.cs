using System;
using Set.Resx;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Set.Models;
using System.Threading.Tasks;

namespace Set.ViewModels
{
	public class SettingItem
	{
		public string Title { get; set; }
		public string Hint { get; set; }
		public string Value { get; set; }
		public EventHandler Clicked {get; set;}
	}

	public class SettingsViewModel : BaseViewModel
	{
		public SettingsPage Page { get; set; }

		protected ObservableCollection<SettingItem> _settingsItems;
		public ObservableCollection<SettingItem> SettingsItems
		{
			get
			{
				_settingsItems = LoadSettings();
				return _settingsItems;
			}
			set
			{ 
				if (_settingsItems != value)
				{
					_settingsItems = value;
					OnPropertyChanged("SettingItems");
				}
			}
		}

		public SettingsViewModel  (INavigation navigation) : base(navigation)
		{
			Title = AppResources.SettingsTitle;
		}

		protected ObservableCollection<SettingItem> LoadSettings()
		{
			var list = new ObservableCollection<SettingItem> ();

			list.Add (new SettingItem (){ Title = AppResources.SettingsUnitSystemTitle, Hint = "", Clicked = async (sender, e) => {

					string[] options = new string[2];
					options[0] = AppResources.UnitSystemMetric;
					options[1] = AppResources.UnitSystemImperial;

					var action = await Page.DisplayActionSheet (AppResources.SettingsUnitSystemTitle, "Cancel", null, options);

					var settingItem = sender as SettingItem;
					settingItem.Value = "xxx";
					OnPropertyChanged("SettingItems"); <--- flatten the list To properties int the viewmodel
				}});

			list.Add (new SettingItem (){ Title = AppResources.SettingsTrainingRulesTitle, Value = "asdfa", Hint = AppResources.SettingsTrainingRulesHint});
			list.Add (new SettingItem (){ Title = AppResources.SettingsMaxRepsTitle, Hint = AppResources.SettingsMaxRepsHint});
			list.Add (new SettingItem (){ Title = AppResources.SettingsMinRepsTitle, Hint = AppResources.SettingsMinRepsHint});
			list.Add (new SettingItem (){ Title = AppResources.SettingsExerciseGoalTitle, Hint = AppResources.SettingsExerciseGoalHint});
			list.Add (new SettingItem (){ Title = AppResources.SettingsRestTimerTitle, Hint = ""});

			return list;
		}
	}
}

