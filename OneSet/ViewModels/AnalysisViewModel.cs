using OneSet.Resx;

namespace OneSet.ViewModels
{
	public class AnalysisViewModel
	{
        public string Title { get; set; }

        public AnalysisViewModel ()
		{
			Title = AppResources.AnalysisTitle;
		}
	}
}

