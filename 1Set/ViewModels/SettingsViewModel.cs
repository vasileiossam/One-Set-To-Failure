using System;
using Set.Resx;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Set.Models;
using System.Threading.Tasks;
using System.Linq;
using Toasts.Forms.Plugin.Abstractions;
using Set.Abstract;
using System.Globalization;

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
				_settings = LoadSettings().Result;
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

		protected async Task<ObservableCollection<PreferenceGroup>> LoadSettings()
		{
			var list = new ObservableCollection<PreferenceGroup>();

			#region general group
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
			#endregion

			#region rules group
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

			#endregion

			#region data group
			var dataGroup = new PreferenceGroup (){ Title = AppResources.SettingsDataTitle };
			list.Add (dataGroup);

			dataGroup.Add (new AlertPreference ()
			{ 
				Title = AppResources.SettingsClearWorkoutDataTitle, 
				Hint = AppResources.SettingsClearWorkoutDataHint,
				PopupTitle = AppResources.SettingsClearWorkoutDataTitle,
				PopupMessage = AppResources.ClearWorkoutDataQuestion,
				Clicked = OnClicked,
				OnExecute = async(sender, args) =>
				{
					var preference = sender as AlertPreference;
					await App.Database.ClearWorkoutData ();
					App.ShowToast (ToastNotificationType.Info, preference.PopupTitle, AppResources.ClearWorkoutDataCompleted);
				}
			});

			var backupPreference = new AlertPreference ()
			{ 
				Title = AppResources.SettingsBackupLocallyTitle, 
				Hint = AppResources.SettingsBackupLocallyHint,
				Clicked = OnClicked,
				Value = await GetLastBackupDate(),
				IsValueVisible = !string.IsNullOrEmpty(await GetLastBackupDate())
			};
			backupPreference.OnExecute += async (sender, args) =>
			{
				try
				{	
					var backupService = DependencyService.Get<IBackupRestore>();
					await backupService.Backup();

					var backupInfo = await DependencyService.Get<IBackupRestore>().GetBackupInfo();				
					App.ShowToast (ToastNotificationType.Info, AppResources.SettingsBackupToastTitleOnSuccess, string.Format(AppResources.SettingsBackupToastMessageOnSuccess, backupInfo.BackupFolder));		

					var preference = sender as AlertPreference;
					preference.Value = await GetLastBackupDate();
				}
				catch(Exception ex)
				{
					App.ShowToast (ToastNotificationType.Error, AppResources.ToastErrorTitle, ex.Message);		
				}
			};
			dataGroup.Add (backupPreference);

			var restorePreference = new AlertPreference ()
			{ 
				Title = AppResources.SettingsRestoreLocallyTitle, 
				Hint = AppResources.SettingsRestoreLocallyHint,
				Clicked = OnClicked
			};
			restorePreference.OnExecute += async (sender, args) =>
			{
				try
				{	
					var backupService = DependencyService.Get<IBackupRestore>();
					var backupInfo = await DependencyService.Get<IBackupRestore>().GetBackupInfo();				

					if (backupInfo.LastBackupDate == null)
					{
						await Page.DisplayAlert (AppResources.RestoreNoBackupTitle, AppResources.RestoreNoBackupMessage, AppResources.OK);
					}
					else
					{
						var answer = await Page.DisplayAlert (AppResources.RestoreQuestionTitle, await GetRestoreQuestionMessage(), AppResources.Yes, AppResources.No);
						if (answer)
						{
							await backupService.Restore();	

							// reload settings
							App.Settings = DependencyService.Get<ISettingsStorage>().Load();
							Page.Refresh();

							App.ShowToast (ToastNotificationType.Info, AppResources.SettingsRestoreToastTitleOnSuccess, AppResources.SettingsRestoreToastMessageOnSuccess);		
						}
					}
				}
				catch(Exception ex)
				{
					App.ShowToast (ToastNotificationType.Error, AppResources.ToastErrorTitle, ex.Message);		
				}

			};
			dataGroup.Add (restorePreference);

			#endregion

			#region other group
			var otherGroup = new PreferenceGroup (){ Title = AppResources.SettingsOtherTitle };
			list.Add (otherGroup);

			otherGroup.Add (new ListPreference ()
			{ 
				Title = AppResources.ShowPreviousRepsWeight,
				Value = ListPreference.GetBoolAsString(App.Settings.PreviousRepsWeightVisible),
				Options = ListPreference.YesNoOptions,
				Clicked = OnClicked,
				OnSave = (sender, args) =>
				{
					var preference = sender as ListPreference;
					App.Settings.PreviousRepsWeightVisible = preference.GetValueAsBool();
					App.SaveSettings();
				}
			});

			otherGroup.Add (new ListPreference ()
			{ 
				Title = AppResources.ShowTargetRepsWeight,
				Value = ListPreference.GetBoolAsString(App.Settings.TargetRepsWeightVisible),
				Options = ListPreference.YesNoOptions,
				Clicked = OnClicked,
				OnSave = (sender, args) =>
				{
					var preference = sender as ListPreference;
					App.Settings.TargetRepsWeightVisible = preference.GetValueAsBool();
					App.SaveSettings();
				}
			});

			otherGroup.Add (new PagePreference ()
			{ 
				Title = AppResources.SettingsAboutTitle,
				Hint = AppResources.SettingsAboutHint,
				Navigation = Page.Navigation,
				NavigateToPage = typeof(AboutPage) 
			});
			#endregion

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

				if (!string.IsNullOrEmpty (preference.PopupMessage))
				{
					var answer = await Page.DisplayAlert (preference.PopupTitle, preference.PopupMessage, AppResources.Yes, AppResources.No);

					if (answer)
					{
						preference.OnExecute (sender, args);
					}
				} 
				else
				{
					preference.OnExecute (sender, args);
				}
			}
		}

		public async Task<string> GetLastBackupDate()
		{
			var lastBackupDate = string.Empty;
			var backupInfo = await DependencyService.Get<IBackupRestore>().GetBackupInfo();

			if (backupInfo.LastBackupDate != null) 
			{
				var date = (DateTime) backupInfo.LastBackupDate;
			
				lastBackupDate = string.Format(AppResources.SettingsLastBackupDate, 
					date.ToString("D") + ", " + date.ToString("hh:mm:ss")
				);
			}

			return lastBackupDate;
		}

		public async Task<string> GetRestoreQuestionMessage()
		{
			var lastBackupDate = string.Empty;
			var backupInfo = await DependencyService.Get<IBackupRestore>().GetBackupInfo();

			if (backupInfo.LastBackupDate != null) 
			{
				var date = (DateTime) backupInfo.LastBackupDate;

				lastBackupDate = string.Format(AppResources.RestoreQuestionMessage, 
					date.ToString("D") + ", " + date.ToString("hh:mm:ss")
				);
			}

			return lastBackupDate;
		}

	}
}

