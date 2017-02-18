using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using OneSet.Resx;
using OneSet.Models;
using Xamarin.Forms;

namespace OneSet.ViewModels
{
	public class AnalysisViewModel : BaseViewModel
	{
		// TODO replace this with MessagingCenter
		// https://forums.xamarin.com/discussion/22499/looking-to-pop-up-an-alert-like-displayalert-but-from-the-view-model-xamarin-forms-labs
		[IgnoreMap]
		public AnalysisPage Page { get; set; }

		public AnalysisViewModel () : base()
		{
			Title = AppResources.AnalysisTitle;
		}
	}
}

