using System;
using System.Collections.Generic;
using Set.ViewModels;
using Xamarin.Forms;

namespace Set
{
	public partial class ExerciseListPage : ContentPage
	{
		public ExerciseListPage ()
		{
			InitializeComponent ();
		}

		public void OnAddExerciseButtonClicked(object sender, EventArgs args)
		{
			var exercisePage = new ExercisePage
			{
				ViewModel = new ExerciseViewModel()
			};

			Navigation.PushAsync(exercisePage);
		}
	}
}

