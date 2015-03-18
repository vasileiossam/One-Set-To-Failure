using System;
using Set.Resx;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Set.Models;
using System.Threading.Tasks;
using System.Linq;
using Toasts.Forms.Plugin.Abstractions;

namespace Set.ViewModels
{
	public class AboutViewModel : BaseViewModel
	{
        public string AppTitle { get; set; }
        public string Version { get; set; }
        public string Author { get; set; }
        public string SupportEmail { get; set; }
        public string Credit1 { get; set; }

        public AboutViewModel(INavigation navigation)
            : base(navigation)
		{
			Title = AppResources.AboutTitle;
            AppTitle = ?
            Version = ?
            Author = ?
            SupportEmail = ?
            Credit1 = ?
		}
		
	}
}

