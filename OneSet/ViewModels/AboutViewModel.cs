using OneSet.Resx;

namespace OneSet.ViewModels
{
	public class AboutViewModel
	{
        public string Title { get; set; }
        public string AppTitle { get; set; }
        public string Version { get; set; }
        public string Author { get; set; }
        public string SupportEmail { get; set; }
        public string Credit1 { get; set; }
		public string Credit2 { get; set; }

        public AboutViewModel()
        {
			Title = AppResources.AboutTitle;
			AppTitle = "One Set To Failure";
			Version = App.Version;
			Author = "Vas";
			SupportEmail = "vas@samaltanos.com";
			Credit1 = "\"Comfortable office worker\" Icon made by Freepik (www.freepik.com) from www.flaticon.com is licensed under CC BY 3.0";
			Credit2 = "\"Gymnast in gym practicing strengthen exercise\" Icon made by Freepik (www.freepik.com) from www.flaticon.com is licensed under CC BY 3.0";
		}
	}
}