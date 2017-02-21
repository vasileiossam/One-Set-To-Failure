using AutoMapper;
using OneSet.Resx;

namespace OneSet.ViewModels
{
	public class AnalysisViewModel : BaseViewModel
	{
		// TODO replace this with MessagingCenter
		// https://forums.xamarin.com/discussion/22499/looking-to-pop-up-an-alert-like-displayalert-but-from-the-view-model-xamarin-forms-labs
		[IgnoreMap]
		public Views.AnalysisPage Page { get; set; }

		public AnalysisViewModel ()
		{
			Title = AppResources.AnalysisTitle;
		}
	}
}

