using System.Threading.Tasks;
using Xamarin.Forms;

namespace OneSet.Abstract
{
    public interface INavigationService
    {
        Task PushAsync(Page page);
        Task PopAsync();
    }
}