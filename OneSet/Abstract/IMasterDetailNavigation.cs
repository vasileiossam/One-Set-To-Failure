using System.Threading.Tasks;
using OneSet.Models;
using OneSet.ViewModels;
using Xamarin.Forms;
using System;

namespace OneSet.Abstract
{
    public interface IMasterDetailNavigation
    {
        void InitNavigation(MasterDetailPage root);
        Task NavigateToDetail<T>(NavigationParameters parameters = null) where T : BaseViewModel;
        void NavigateToDetail(Page page);
        Task NavigateToHierarchical<T>(NavigationParameters parameters = null) where T : BaseViewModel;
        Task NavigateToHierarchical(Page page);
        Task PopAsync();
    }
}