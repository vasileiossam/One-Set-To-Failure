using System;

using Xamarin.Forms;

namespace Set
{
	public class WorkoutPage : ContentPage
	{
		public WorkoutPage ()
		{
			Content = new StackLayout { 
				Children = {
					new Label { Text = "Hello ContentPage" }
				}
			};
		}
	}
}


