using System.Threading.Tasks;
using OneSet.Abstract;
using OneSet.ViewModels;
using Xamarin.Forms;
using System;
using System.Linq;
using System.Reflection;
using Autofac;
using OneSet.Views;
using OneSet.Models;

namespace OneSet.Services
{
    [Obsolete]
    public class HierarchicalNavigation : IHierarchicalNavigation
    {
        private NavigationPage _rootPage;
        private readonly IComponentContext _componentContext;

        public HierarchicalNavigation(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }

        /// <summary>
        /// Init navigation to set the root page
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public NavigationPage InitNavigation<T>(NavigationParameters parameters = null) where T : BaseViewModel
        {
            var page = GetPage<T>();
            _rootPage = new NavigationPage(page);

            Task.Run(async () =>
            {
                await BindViewModel<T>(page, parameters);
                await _rootPage.PushAsync(page);
            }).Wait();

            return _rootPage;
        }

        public async Task NavigateTo<T>(NavigationParameters parameters = null) where T : BaseViewModel
        {
            var page = GetPage<T>();
            await BindViewModel<T>(page, parameters);
            await _rootPage.PushAsync(page);
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

        public async Task NavigateTo(BaseViewModel viewModel)
        {
            var page = GetPage(viewModel.GetType());
            var property = page.GetType().GetRuntimeProperties().FirstOrDefault(x => x.Name == "ViewModel");
            property?.SetValue(page, viewModel);

            await _rootPage.PushAsync(page);
        }

        public async Task PushAsync(Page page)
        {
            await _rootPage.PushAsync(page);
        }

        public async Task PopAsync()
        {
            await OnNavigateFrom();
            await _rootPage.PopAsync();
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
