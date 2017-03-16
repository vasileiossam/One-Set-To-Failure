using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using OneSet.Abstract;
using OneSet.Extensions;
using OneSet.Models;
using OneSet.Resx;
using Xamarin.Forms;

namespace OneSet.ViewModels
{
	public class SettingsViewModel : BaseViewModel, INavigationAware
    {
        #region private variables
        private readonly IDialogService _dialogService;
        private readonly IMessagingService _messagingService;
        private readonly ISettingsStorage _settingsStorage;
        private readonly ΙStatistics _statistics;
        private readonly IExporter _exporter;
        private readonly IBackupRestoreService _backupRestoreService;
        #endregion

        #region properties
        private ObservableCollection<PreferenceGroup> _settings;
        public ObservableCollection<PreferenceGroup> Settings
        {
            get { return _settings; }
            set { SetProperty(ref _settings, value); }
        }

        public ICommand SelectItemCommand { get; }
        #endregion

        public SettingsViewModel(IDialogService dialogService, IMessagingService messagingService, IBackupRestoreService backupRestoreService,
            ISettingsStorage settingsStorage, ΙStatistics statistics, IExporter exporter)
        {
            _dialogService = dialogService;
            _messagingService = messagingService;
            _settingsStorage = settingsStorage;
            _statistics = statistics;
            _exporter = exporter;
            _backupRestoreService = backupRestoreService;

            Title = AppResources.SettingsTitle;

            SelectItemCommand = new Command<Preference>(OnItemSelected);
        }

        #region private methods
        private async Task<string> GetLastBackupDate()
        {
            var lastBackupDate = string.Empty;
            var backupInfo = await _backupRestoreService.GetBackupInfo();

            if (backupInfo.LastBackupDate != null)
            {
                var date = (DateTime)backupInfo.LastBackupDate;

                lastBackupDate = string.Format(AppResources.SettingsLastBackupDate,
                    date.ToString("D") + ", " + date.ToString("hh:mm:ss")
                );
            }

            return lastBackupDate;
        }

        private async Task<string> GetRestoreQuestionMessage()
        {
            var lastBackupDate = string.Empty;
            var backupInfo = await _backupRestoreService.GetBackupInfo();

            if (backupInfo.LastBackupDate != null)
            {
                var date = (DateTime)backupInfo.LastBackupDate;

                lastBackupDate = string.Format(AppResources.RestoreQuestionMessage,
                    date.ToString("D") + ", " + date.ToString("hh:mm:ss")
                );
            }

            return lastBackupDate;
        }
        #endregion

        public async void OnClicked(object sender, EventArgs args)
		{
		    var listPreference = sender as ListPreference;
		    if (listPreference != null)
			{
				var preference = listPreference;
				var action = await _dialogService.DisplayActionSheet(preference.Title, AppResources.CancelButton, null, preference.Options);

				if (action != null && action != AppResources.CancelButton)
				{
					preference.Value = action;
					preference.OnSave (listPreference, args);
				}
			}

            var alertPreference = sender as AlertPreference;
            if (alertPreference == null) return;
            {
                var preference = alertPreference;

                if (!string.IsNullOrEmpty(preference.PopupMessage))
                {
                    var answer = await _dialogService.DisplayAlert(preference.PopupTitle, preference.PopupMessage, AppResources.Yes, AppResources.No);

                    if (answer)
                    {
                        preference.OnExecute(alertPreference, args);
                    }
                }
                else
                {
                    preference.OnExecute(alertPreference, args);
                }
            }
        }

        #region INavigationAware
        public async Task OnNavigatedFrom(NavigationParameters parameters)
        {
            await Task.FromResult(0);
        }

        public async Task OnNavigatedTo(NavigationParameters parameters)
        {
            var list = new ObservableCollection<PreferenceGroup>();

            #region general group
            var generalGroup = new PreferenceGroup { Title = AppResources.SettingsGeneralTitle };
            list.Add(generalGroup);

            var options = new string[2];
            options[0] = AppResources.UnitSystemMetric;
            options[1] = AppResources.UnitSystemImperial;
            var value = options[1];
            if (App.Settings.IsMetric)
                value = options[0];
            generalGroup.Add(new ListPreference
            {
                Title = AppResources.SettingsUnitSystemTitle,
                Hint = "",
                Value = value,
                Options = options,
                Clicked = OnClicked,
                OnSave = (sender, args) =>
                {
                    var preference = sender as ListPreference;
                    if (preference != null) App.Settings.IsMetric = ((string)preference.Value == AppResources.UnitSystemMetric);
                    SaveSettings();
                }
            });
            #endregion

            #region rules group
            var rulesGroup = new PreferenceGroup { Title = AppResources.SettingsTrainingRulesTitle, Hint = AppResources.SettingsTrainingRulesHint };
            list.Add(rulesGroup);

            rulesGroup.Add(new ListPreference
            {
                Title = AppResources.SettingsMaxRepsTitle,
                Hint = AppResources.SettingsMaxRepsHint,
                Value = App.Settings.MaxReps,
                Options = ListPreference.GetOptionsList(0, 50, false),
                Clicked = OnClicked,
                OnSave = (sender, args) =>
                {
                    var preference = sender as ListPreference;
                    if (preference != null) App.Settings.MaxReps = int.Parse((string)preference.Value);
                    SaveSettings();
                }
            });

            rulesGroup.Add(new ListPreference
            {
                Title = AppResources.SettingsMinRepsTitle,
                Hint = AppResources.SettingsMinRepsHint,
                Value = App.Settings.MinReps,
                Options = ListPreference.GetOptionsList(0, 50, false),
                Clicked = OnClicked,
                OnSave = (sender, args) =>
                {
                    var preference = sender as ListPreference;
                    if (preference != null) App.Settings.MinReps = int.Parse((string)preference.Value);
                    SaveSettings();
                }
            });

            var index = 0;
            options = new string[App.Database.RepsIncrements.Count];
            foreach (var item in App.Database.RepsIncrements)
            {
                options[index++] = item.Description;
            }
            rulesGroup.Add(new ListPreference
            {
                Title = AppResources.SettingsExerciseGoalTitle,
                Hint = AppResources.SettingsExerciseGoalHint,
                Value = App.Settings.RepsIncrement.Description,
                Options = options,
                Clicked = OnClicked,
                OnSave = (sender, args) =>
                {
                    var preference = sender as ListPreference;
                    App.Settings.RepsIncrementId = App.Database.RepsIncrements.FirstOrDefault(x => x.Description == (string)preference.Value).RepsIncrementId;
                    SaveSettings();
                }
            });

            #endregion

            #region motivation group
            var motivationGroup = new PreferenceGroup { Title = AppResources.SettingsMotivationTitle };
            list.Add(motivationGroup);

            index = 0;
            options = new string[App.Database.ImagePacks.Count];
            foreach (var item in App.Database.ImagePacks)
            {
                options[index++] = item.Title;
            }
            motivationGroup.Add(new ListPreference
            {
                Title = AppResources.SettingsMotivationalImagePacksTitle,
                Hint = AppResources.SettingsMotivationalImagePacksHint,
                Options = options,
                Clicked = OnClicked,
                Value = App.Settings.ImagePack.Title,
                OnSave = (sender, args) =>
                {
                    var preference = sender as ListPreference;
                    if (preference == null) throw new ArgumentNullException(nameof(preference));
                    if (App.Settings != null)
                        App.Settings.ImagePackId = App.Database.ImagePacks.FirstOrDefault(x => x.Title == (string)preference.Value).ImagePackId;
                    SaveSettings();
                }
            });

            motivationGroup.Add(new ListPreference
            {
                Title = AppResources.SettingsShowImagesInRestTimerTitle,
                Value = ListPreference.GetBoolAsString(App.Settings.CanShowImagePackInRestTimer),
                Options = ListPreference.YesNoOptions,
                Clicked = OnClicked,
                OnSave = (sender, args) =>
                {
                    var preference = sender as ListPreference;
                    if (preference != null) App.Settings.CanShowImagePackInRestTimer = preference.GetValueAsBool();
                    SaveSettings();
                }
            });

            //            motivationGroup.Add(new ListPreference()
            //            {
            //                Title = AppResources.SettingsShowImagesPackInWorkoutTitle,
            //                Value = ListPreference.GetBoolAsString(App.Settings.CanShowImagePackInWorkout),
            //                Options = ListPreference.YesNoOptions,
            //                Clicked = OnClicked,
            //                OnSave = (sender, args) =>
            //                {
            //                    var preference = sender as ListPreference;
            //                    App.Settings.CanShowImagePackInWorkout = preference.GetValueAsBool();
            //                    App.SaveSettings();
            //                }
            //            });

            #endregion

            #region data group
            var dataGroup = new PreferenceGroup { Title = AppResources.SettingsDataTitle };
            list.Add(dataGroup);

            dataGroup.Add(new AlertPreference
            {
                Title = AppResources.SettingsClearWorkoutDataTitle,
                Hint = AppResources.SettingsClearWorkoutDataHint,
                PopupTitle = AppResources.SettingsClearWorkoutDataTitle,
                PopupMessage = AppResources.ClearWorkoutDataQuestion,
                Clicked = OnClicked,
                OnExecute = async (sender, args) =>
                {
                    var preference = sender as AlertPreference;
                    await App.Database.ClearWorkoutData();
                    _messagingService.Send(this, Messages.WorkoutDataCleared);
                    if (preference != null) AppResources.ClearWorkoutDataCompleted.ToToast(ToastNotificationType.Info, preference.PopupTitle);
                }
            });

            options = new string[1];
            options[0] = "Life Fitness equipment";
            dataGroup.Add(new ListPreference
            {
                Title = AppResources.SettingsLoadSampleDataTitle,
                Hint = AppResources.SettingsLoadSampleDataHint,
                Options = options,
                Clicked = OnClicked,
                IsValueVisible = false,
                OnSave = async (sender, args) =>
                {
                    var preference = sender as Preference;
                    await App.Database.LoadLifeFitnessData();
                    if (preference != null) AppResources.LoadSampleDataCompleted.ToToast(ToastNotificationType.Info, preference.Title);
                }
            });

            var backupPreference = new AlertPreference
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
                    await _backupRestoreService.Backup();

                    var backupInfo = await _backupRestoreService.GetBackupInfo();

                    string.Format(AppResources.SettingsBackupToastMessageOnSuccess, backupInfo.BackupFolder)
                        .ToToast(ToastNotificationType.Info, AppResources.SettingsBackupToastTitleOnSuccess);

                    var preference = sender as AlertPreference;
                    if (preference != null) preference.Value = await GetLastBackupDate();
                }
                catch (Exception ex)
                {
                    App.ShowErrorPage(this, ex);
                }
            };
            dataGroup.Add(backupPreference);

            var restorePreference = new AlertPreference
            {
                Title = AppResources.SettingsRestoreLocallyTitle,
                Hint = AppResources.SettingsRestoreLocallyHint,
                Clicked = OnClicked
            };
            restorePreference.OnExecute += async (sender, args) =>
            {
                try
                {
                    var backupInfo = await _backupRestoreService.GetBackupInfo();

                    if (backupInfo.LastBackupDate == null)
                    {
                        await _dialogService.DisplayAlert(AppResources.RestoreNoBackupTitle, AppResources.RestoreNoBackupMessage, AppResources.OK);
                    }
                    else
                    {
                        var answer = await _dialogService.DisplayAlert(AppResources.RestoreQuestionTitle, await GetRestoreQuestionMessage(), AppResources.Yes, AppResources.No);
                        if (!answer) return;
                        await _backupRestoreService.Restore();

                        // reload settings 
                        App.Settings = _settingsStorage.Load();
                        _messagingService.Send(this, Messages.SettingsReloaded);

                        AppResources.SettingsRestoreToastMessageOnSuccess.ToToast(ToastNotificationType.Info, AppResources.SettingsRestoreToastTitleOnSuccess);
                    }
                }
                catch (Exception ex)
                {
                    App.ShowErrorPage(this, ex);
                }

            };
            dataGroup.Add(restorePreference);

            options = new string[1];
            options[0] = AppResources.SettingsExportWorkoutDataOption1;
            dataGroup.Add(new ListPreference
            {
                Title = AppResources.SettingsExportWorkoutDataTitle,
                Hint = AppResources.SettingsExportWorkoutDataHint,
                Options = options,
                Clicked = OnClicked,
                IsValueVisible = false,
                OnSave = async (sender, args) =>
                {
                    var preference = sender as Preference;
                    var pathName = await _exporter.ExportToCsv();
                    if (pathName != string.Empty)
                    {
                        if (preference != null)
                            string.Format(AppResources.SettingsExportWorkoutDataCompleted, pathName).ToToast(ToastNotificationType.Info, preference.Title);
                    }
                }
            });

            var recalcStatisticsPreference = new AlertPreference
            {
                Title = AppResources.SettingsRecalcStatisticsTitle,
                Hint = AppResources.SettingsRecalcStatisticsTitleHint,
                Clicked = OnClicked
            };
            recalcStatisticsPreference.OnExecute += async (sender, args) =>
            {
                try
                {
                    await _statistics.Recalc();
                    AppResources.SettingsRecalcStatisticsFinished.ToToast(ToastNotificationType.Info, AppResources.SettingsRecalcStatisticsTitle);
                }
                catch (Exception ex)
                {
                    App.ShowErrorPage(this, ex);
                }

            };
            dataGroup.Add(recalcStatisticsPreference);


            #endregion

            #region other group
            var otherGroup = new PreferenceGroup { Title = AppResources.SettingsOtherTitle };
            list.Add(otherGroup);

            otherGroup.Add(new ListPreference
            {
                Title = AppResources.ShowPreviousRepsWeight,
                Value = ListPreference.GetBoolAsString(App.Settings.PreviousRepsWeightVisible),
                Options = ListPreference.YesNoOptions,
                Clicked = OnClicked,
                OnSave = (sender, args) =>
                {
                    var preference = sender as ListPreference;
                    if (preference != null) App.Settings.PreviousRepsWeightVisible = preference.GetValueAsBool();
                    SaveSettings();
                }
            });

            otherGroup.Add(new ListPreference
            {
                Title = AppResources.ShowTargetRepsWeight,
                Value = ListPreference.GetBoolAsString(App.Settings.TargetRepsWeightVisible),
                Options = ListPreference.YesNoOptions,
                Clicked = OnClicked,
                OnSave = (sender, args) =>
                {
                    var preference = sender as ListPreference;
                    if (preference != null) App.Settings.TargetRepsWeightVisible = preference.GetValueAsBool();
                    SaveSettings();
                }
            });
            #endregion

            Settings = list;
        }
        #endregion

        private void OnItemSelected(Preference item)
        {
            item?.Clicked(item, null);
        }

        private void SaveSettings()
        {
            _settingsStorage.Save(App.Settings);
        }
    }
}

