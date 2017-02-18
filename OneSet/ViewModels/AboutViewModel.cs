using OneSet.Resx;

namespace OneSet.ViewModels
{
	public class AboutViewModel : BaseViewModel
	{
        public string AppTitle { get; set; }
        public string Version { get; set; }
        public string Author { get; set; }
        public string SupportEmail { get; set; }
        public string Credit1 { get; set; }
		public string Credit2 { get; set; }

        public AboutViewModel()
            : base()
		{
			Title = AppResources.AboutTitle;
			AppTitle = "One Set To Fatigue";
			Version = App.Version;
			Author = "Uncle Jacar";
			SupportEmail = "sos@arxeio.gr";
			Credit1 = "\"Comfortable office worker\" Icon made by Freepik (www.freepik.com) from www.flaticon.com is licensed under CC BY 3.0";
			Credit2 = "\"Gymnast in gym practicing strengthen exercise\" Icon made by Freepik (www.freepik.com) from www.flaticon.com is licensed under CC BY 3.0";

		}
		
	}
}

