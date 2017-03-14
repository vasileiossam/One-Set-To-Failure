using System.Threading.Tasks;
using OneSet.Abstract;
using OneSet.ViewModels;
using Xamarin.Forms;
using System;
using Autofac;
using OneSet.Views;
using OneSet.Models;

namespace OneSet.Services
{
    public class MasterDetailNavigation : IMasterDetailNavigation
    {
        private MasterDetailPage _root;
        private readonly IComponentContext _componentContext;
        private NavigationPage _navPage;
        private Type _mainViewModel;

        public MasterDetailNavigation(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }

        public void InitNavigation(MasterDetailPage root)
        { 
            _root = root;
        }

        public async Task NavigateToDetail<T>(NavigationParameters parameters = null) where T : BaseViewModel
        {
            var page = GetPage<T>();
            await BindViewModel<T>(page, parameters);
            
            // init
            if (_navPage == null)
            {
                _navPage = new NavigationPage(page);
                _mainViewModel = typeof(T);
                _root.Detail = _navPage;
                return;
            }

            // main page
            if (typeof(T) == _mainViewModel)
            {
                _root.Detail = _navPage;
            }
            // other page
            else
            {
                await _navPage.PushAsync(page);
            }
        }

        public async Task NavigateToHierarchical<T>(NavigationParameters parameters = null) where T : BaseViewModel
        {
            var page = GetPage<T>();
            await BindViewModel<T>(page, parameters);
            await _root.Detail.Navigation.PushAsync(page);
        }


        public async Task NavigateToHierarchical(Page page)
        {
            await _root.Detail.Navigation.PushAsync(page);
        }

        public void NavigateToDetail(Page page)
        {
            _root.Detail = new NavigationPage(page);
        }

        private async Task BindViewModel<T>(Page page, NavigationParameters parameters = null) where T : BaseViewModel
        {
            if (parameters == null)
            {
                parameters = new NavigationParameters();
            }

            var viewModel = _componentContext.Resolve<T>();

            if (page is BasePage<T>)
            {
                var p = page as BasePage<T>;
                p.ViewModel = viewModel;
            }

            if (viewModel is INavigationAware)
            {
                await OnNavigateFrom();
                await (viewModel as INavigationAware).OnNavigatedTo(parameters);
            }
        }

        public async Task PopAsync()
        {
            await OnNavigateFrom();
            await _root.Detail.Navigation.PopAsync();
        }

        public async Task OnNavigateFrom()
        {
            // TODO implement OnNavigateFrom
        }

        private Page GetPage<T>()
        {
            return GetPage(typeof(T));
        }

        private Page GetPage(Type viewModelType)
        {
            // by convention every page should be named as FooBarViewModel => FooBarPage
            var pageName = viewModelType.Name.Replace("ViewModel", "Page");

            var pageType = Type.GetType($"OneSet.Views.{pageName}");
            return _componentContext.Resolve(pageType) as Page;
        }
    }
}
