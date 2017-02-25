using System.Threading.Tasks;
using OneSet.ViewModels;
using Xamarin.Forms;

namespace OneSet.Abstract
{
    public interface INavigationService
    {
        NavigationPage InitMain<T>() where T : BaseViewModel;
        Task NavigateTo<T>() where T : BaseViewModel;
        Task NavigateTo(BaseViewModel viewModel);
        Task PushAsync(Page page);
        Task PopAsync();
    }
}