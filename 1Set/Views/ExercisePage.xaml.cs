using System;
using System.Collections.Generic;
using Set.ViewModels;
using Xamarin.Forms;
using Set.Resx;

namespace Set
{
	public partial class ExercisePage : TabbedPage
	{
		private ExerciseViewModel _viewModel;
		public ExerciseViewModel ViewModel
		{
			get
			{
				if (_viewModel == null)
				{
					_viewModel =  new ExerciseViewModel(){Navigation = this.Navigation};
				}
				return _viewModel;
			}
			set
			{
				_viewModel = value;
			}
		}

		public ExercisePage ()
		{
			InitializeComponent ();
		}
	}
}

