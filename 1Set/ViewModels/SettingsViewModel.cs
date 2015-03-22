using System;
using Set.Resx;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Set.Models;
using System.Threading.Tasks;
using System.Linq;
using Toasts.Forms.Plugin.Abstractions;

namespace Set.ViewModels
{
	public class SettingsViewModel : BaseViewModel
	{
		// TODO replace this with MessagingCenter
		// https://forums.xamarin.com/discussion/22499/looking-to-pop-up-an-alert-like-displayalert-but-from-the-view-model-xamarin-forms-labs
		public SettingsPage Page { get; set; }

		protected ObservableCollection<PreferenceGroup> _settings;
		public ObservableCollection<PreferenceGroup> Settings
		{
			get
			{
				_settings = LoadSettings();
				return _settings;
			}
			set
			{ 
				if (_settings != value)
				{
					_settings = value;
					OnPropertyChanged("Settings");
				}
			}
		}

		public SettingsViewModel  () : base()
		{
			Title = AppResources.SettingsTitle;
		}

		protected ObservableCollection<PreferenceGroup> LoadSettings()
		{
			var list = new ObservableCollection<PreferenceGroup>();

			var generalGroup = new PreferenceGroup (){ Title = AppResources.SettingsGeneralTitle };
			list.Add (generalGroup);

			var options = new string[2];
			options[0] = AppResources.UnitSystemMetric;
			options[1] = AppResources.UnitSystemImperial;
			var value = options [1];
			if (App.Settings.IsMetric)
				value = options [0];
			generalGroup.Add (new ListPreference ()
			{ 
				Title = AppResources.SettingsUnitSystemTitle, 
				Hint = "",
				Value = value,
				Options = options,
				Clicked = OnClicked,
				OnSave = (sender, args) =>
				{
					var preference = sender as ListPreference;
					App.Settings.IsMetric = ((string)preference.Value == AppResources.UnitSystemMetric);
					App.SaveSettings();
				}
			});

			var rulesGroup = new PreferenceGroup (){ Title = AppResources.SettingsTrainingRulesTitle, Hint = AppResources.SettingsTrainingRulesHint };
			list.Add (rulesGroup);

			rulesGroup.Add (new ListPreference ()
			{ 
				Title = AppResources.SettingsMaxRepsTitle, 
				Hint = AppResources.SettingsMaxRepsHint,
				Value = App.Settings.MaxReps,
				Options = ListPreference.GetOptionsList(0, 50, false),
				Clicked = OnClicked,
				OnSave = (sender, args) =>
				{
					var preference = sender as ListPreference;
					App.Settings.MaxReps = int.Parse((string) preference.Value);
					App.SaveSettings();
				}
			});

			rulesGroup.Add (new ListPreference ()
			{ 
				Title = AppResources.SettingsMinRepsTitle, 
				Hint = AppResources.SettingsMinRepsHint,
				Value = App.Settings.MinReps,
				Options = ListPreference.GetOptionsList(0, 50, false),
				Clicked = OnClicked,
				OnSave = (sender, args) =>
				{
					var preference = sender as ListPreference;
					App.Settings.MinReps = int.Parse((string) preference.Value);
					App.SaveSettings();
				}
			});

			var index = 0;
			options = new String[App.Database.RepsIncrements.Count];
			foreach(var item in App.Database.RepsIncrements)
			{
				options[index++] = item.Description;
			}
			rulesGroup.Add (new ListPreference ()
			{ 
				Title = AppResources.SettingsExerciseGoalTitle, 
				Hint = AppResources.SettingsExerciseGoalHint,
				Value = App.Settings.RepsIncrement.Description,
				Options = options,
				Clicked = OnClicked,
				OnSave = (sender, args) =>
				{
					var preference = sender as ListPreference;
					App.Settings.RepsIncrementId = App.Database.RepsIncrements.FirstOrDefault (x => x.Description == (string) preference.Value).RepsIncrementId;
					App.SaveSettings();
				}
			});

			index = 0;
			options = new String[App.Database.RestTimers.Count];
			foreach(var item in App.Database.RestTimers)
			{
				options[index++] = item.Description;
			}
			rulesGroup.Add (new ListPreference ()
			{ 
				Title = AppResources.SettingsRestTimerTitle, 
				Hint = "",
				Value = App.Settings.RestTimer.Description,
				Options = options,
				Clicked = OnClicked,
				OnSave = (sender, args) =>
				{
					var preference = sender as ListPreference;
					App.Settings.RestTimerId = App.Database.RestTimers.FirstOrDefault (x => x.Description == (string) preference.Value).RestTimerId;
					App.SaveSettings();
				}
			});


			var dataGroup = new PreferenceGroup (){ Title = AppResources.SettingsDataTitle };
			list.Add (dataGroup);

			dataGroup.Add (new AlertPreference ()
			{ 
				Title = AppResources.SettingsClearWorkoutDataTitle, 
				Hint = AppResources.SettingsClearWorkoutDataHint,
				PopupTitle = AppResources.SettingsClearWorkoutDataTitle,
				PopupMessage = AppResources.ClearWorkoutDataQuestion,
				TostMessage = AppResources.ClearWorkoutDataCompleted,
				Clicked = OnClicked,
				OnSave = (sender, args) =>
				{
					var preference = sender as ListPreference;
					App.Settings.MinReps = int.Parse((string) preference.Value);
					App.SaveSettings();
				}
			});

			var otherGroup = new PreferenceGroup (){ Title = AppResources.SettingsOtherTitle };
			list.Add (otherGroup);

			otherGroup.Add (new PagePreference ()
			{ 
				Title = AppResources.SettingsAboutTitle,
				Hint = AppResources.SettingsAboutHint,
				Navigation = Page.Navigation,
				NavigateToPage = typeof(AboutPage) 
			});

			return list;
		}

		public async void OnClicked(object sender, EventArgs args)
		{
			if (sender is ListPreference)
			{
				var preference = sender as ListPreference;
				var action = await Page.DisplayActionSheet (preference.Title, AppResources.CancelButton, null, preference.Options);

				if ((action != null) && (action != AppResources.CancelButton))
				{
					preference.Value = action;
					preference.OnSave (sender, args);
				}
			}

			if (sender is AlertPreference)
			{
				var preference = sender as AlertPreference;
				var answer = await Page.DisplayAlert (preference.PopupTitle, preference.PopupMessage, AppResources.Yes, AppResources.No);

				if (answer)
				{
					App.Database.ClearWorkoutData ();
					App.ShowToast (ToastNotificationType.Info, preference.PopupTitle, preference.TostMessage);
				}
			}
		}
	}
}

