using System;
using System.Threading.Tasks;
using OneSet.Models;
using OneSet.ViewModels;
using Xamarin.Forms;

namespace OneSet.Abstract
{
    [Obsolete("Replaced by IMasterDetailNavigation")]
    public interface IHierarchicalNavigation
    {
        NavigationPage InitNavigation<T>(NavigationParameters parameters = null) where T : BaseViewModel;
        Task NavigateTo<T>(NavigationParameters parameters = null) where T : BaseViewModel;
        Task PushAsync(Page page);
        Task PopAsync();
    }
}