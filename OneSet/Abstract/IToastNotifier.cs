using System.Threading.Tasks;

namespace OneSet.Abstract
{
    public interface IToastNotifier
    {
        Task<bool> Notify(string message, ToastNotificationType type = ToastNotificationType.Info, string title = null);
    }

    public enum ToastNotificationType
    {
        Info,
        Success,
        Error,
        Warning,
    }
}