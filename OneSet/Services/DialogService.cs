using System.Threading.Tasks;
using OneSet.Abstract;

namespace OneSet.Services
{
    public class DialogService : IDialogService
    {
        public async Task<string> DisplayActionSheet(string title, string cancel, string destruction, params string[] buttons)
        {
            return await Xamarin.Forms.Application.Current.MainPage.DisplayActionSheet(title, cancel, destruction, buttons);
        }

        public async Task DisplayAlert(string title, string message, string cancel)
        {
            await Xamarin.Forms.Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }

        public async Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
        {
            return await Xamarin.Forms.Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);
        }
    }
}
