using OneSet.Abstract;
using Xamarin.Forms;

namespace OneSet.Extensions
{
    public static class Extensions
    {
        public static void ToToast(this string message, ToastNotificationType type = ToastNotificationType.Info, string title = null)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var toaster = DependencyService.Get<IToastNotifier>();
                toaster?.Notify(message, type, title ?? type.ToString().ToUpper());
            });
        }
    }
}
