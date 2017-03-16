using System.Threading.Tasks;
using Android.Widget;
using OneSet.Abstract;
using OneSet.Droid.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(ToastNotifier))]

namespace OneSet.Droid.Services
{
    public class ToastNotifier : IToastNotifier
    {
        public Task<bool> Notify(string message, ToastNotificationType type = ToastNotificationType.Info, string title = null)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();
            Toast.MakeText(Forms.Context, message, ToastLength.Short).Show();
            return taskCompletionSource.Task;
        }
    }
}