using OneSet.ViewModels;
using Xamarin.Forms;

namespace OneSet.Views
{
    public class BasePage<T> : ContentPage where T : BaseViewModel
    {
        public T ViewModel { get; set; }
    }
}
