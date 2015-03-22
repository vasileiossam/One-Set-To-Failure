using System;
using System.Collections.Generic;
using Set.ViewModels;
using Set.Models;
using Xamarin.Forms;
using Set.Localization;
using Set.Resx;
using System.Linq;

namespace Set
{
	public partial class ExerciseDetailsPage : ContentPage
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

		public ExerciseDetailsPage ()
		{
			InitializeComponent ();
		}

        protected async override void OnAppearing()
        {
            base.OnAppearing();
			BindingContext = ViewModel;

			PlateWeightLabel.Text = string.Format (AppResources.PlateWeightLabel, L10n.GetWeightUnit());
			var dayNames = AppResources.Culture.DateTimeFormat.DayNames;
            MonLabel.Text = dayNames[1];
            TueLabel.Text = dayNames[2];
            WedLabel.Text = dayNames[3];
            ThuLabel.Text = dayNames[4];
            FriLabel.Text = dayNames[5];
            SatLabel.Text = dayNames[6];
            SunLabel.Text = dayNames[0];

			var deleteItem = ToolbarItems.FirstOrDefault (x => x.Icon == "ic_action_discard");
			if ((ViewModel.ExerciseId > 0) && (deleteItem == null))
			{
				deleteItem = new ToolbarItem (){ Order = ToolbarItemOrder.Primary, Icon = "ic_action_discard" };
				deleteItem.SetBinding<ExerciseViewModel> (ToolbarItem.CommandProperty, x => x.DeleteCommand);
				ToolbarItems.Insert (0, deleteItem);
			}
        }

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
		}

		protected void OnPlateWeightStepperValueChanged(object sender, ValueChangedEventArgs args)
		{
			PlateWeightStepperLabel.Text = args.NewValue.ToString ();
		}


	}
}

