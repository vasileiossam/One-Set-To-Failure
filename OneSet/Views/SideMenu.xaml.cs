using System.Collections.Generic;
using OneSet.Models;
using Xamarin.Forms;
using OneSet.Resx;
using OneSet.ViewModels;

namespace OneSet.Views
{
    public partial class SideMenu : ContentPage
    {
        public ListView ListView { get { return listView; } }

        public SideMenu()
        {
            InitializeComponent();

            var masterPageItems = new List<SideMenuItem>
            {
                new SideMenuItem
                {
                    Title = AppResources.Workouts,
                    IconSource = "home",
                    TargetType = typeof(WorkoutsViewModel)
                },
                new SideMenuItem
                {
                    Title = AppResources.ExercisesTitle,
                    IconSource = "exercises.png",
                    TargetType = typeof(ExerciseListViewModel)
                },
                new SideMenuItem
                {
                    Title = AppResources.AnalysisTitle,
                    IconSource = "linechart.png",
                    TargetType = typeof(AnalysisTabbedPage)
                },
                new SideMenuItem
                {
                    Title = AppResources.SettingsTitle,
                    IconSource = "cog.png",
                    TargetType = typeof(SettingsViewModel)
                },
                new SideMenuItem
                {
                    Title = AppResources.AboutTitle,
                    IconSource = "info.png",
                    TargetType = typeof(AboutPage)
                }
            };
            listView.ItemsSource = masterPageItems;
        }
    }
}
