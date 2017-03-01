using System.Linq;
using OneSet.Localization;
using OneSet.Resx;
using OneSet.ViewModels;
using Xamarin.Forms;

namespace OneSet.Views
{
	public partial class ExerciseDetailsPage : ExerciseDetailsPageXaml
    {
		public ExerciseDetailsPage ()
		{
			InitializeComponent ();
		}

        protected override async void OnAppearing()
        {
            base.OnAppearing();

			BindingContext = ViewModel;

            // set Text
            PlateWeightLabel.Text = string.Format (AppResources.PlateWeightLabel, L10n.GetWeightUnit());
			var dayNames = AppResources.Culture.DateTimeFormat.DayNames;
            MonLabel.Text = dayNames[1];
            TueLabel.Text = dayNames[2];
            WedLabel.Text = dayNames[3];
            ThuLabel.Text = dayNames[4];
            FriLabel.Text = dayNames[5];
            SatLabel.Text = dayNames[6];
            SunLabel.Text = dayNames[0];

            // setup delete toolbar item
			var deleteItem = ToolbarItems.FirstOrDefault (x => x.Icon == "ic_action_discard");
			if (ViewModel.Exercise.ExerciseId > 0 && deleteItem == null)
			{
				deleteItem = new ToolbarItem { Order = ToolbarItemOrder.Primary, Icon = "ic_action_discard" };
				deleteItem.SetBinding<ExerciseDetailsViewModel> (MenuItem.CommandProperty, x => x.DeleteCommand);
				ToolbarItems.Insert (0, deleteItem);
			}
        }
	}

    public class ExerciseDetailsPageXaml : BasePage<ExerciseDetailsViewModel>
    {

    }
}

