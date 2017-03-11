using OneSet.Models;
using System;
using System.Threading.Tasks;
using Autofac;
using Xamarin.Forms;
using OneSet.Abstract;
using OneSet.ViewModels;

namespace OneSet.Views
{
    public partial class MainPage : MasterDetailPage
    {
        private readonly IComponentContext _componentContext;
        private readonly IMasterDetailNavigation _navigationService;
        
        public MainPage(IComponentContext componentContext, IMasterDetailNavigation navigationService)
        {
            InitializeComponent();
            _componentContext = componentContext;
            _navigationService = navigationService;

            sideMenu.ListView.ItemSelected += OnItemSelected;
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as SideMenuItem;
            if (item == null) return;

            if (item.TargetType == typeof(WorkoutsViewModel))
            {
                await _navigationService.NavigateToDetail<WorkoutsViewModel>();
            }
            else
            if (item.TargetType == typeof(ExerciseListViewModel))
            {
                await _navigationService.NavigateToDetail<ExerciseListViewModel>();
            }
            else
            if (item.TargetType == typeof(SettingsViewModel))
            {
                await _navigationService.NavigateToDetail<SettingsViewModel>();
            }
            else
            if (item.TargetType == typeof(AnalysisTabbedPage))
            {
                _navigationService.NavigateToDetail(_componentContext.Resolve<AnalysisTabbedPage>());
            }
            else
            if (item.TargetType == typeof(AboutPage))
            {
                _navigationService.NavigateToDetail(_componentContext.Resolve<AboutPage>());
            }

            sideMenu.ListView.SelectedItem = null;
            IsPresented = false;
        }
    }
}