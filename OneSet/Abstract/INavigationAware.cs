using OneSet.Models;
using System.Threading.Tasks;

namespace OneSet.Abstract
{
    public interface INavigationAware
    {
        Task OnNavigatedFrom(NavigationParameters parameters);
        Task OnNavigatedTo(NavigationParameters parameters);
    }
}
