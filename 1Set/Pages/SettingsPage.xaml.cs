using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Set
{
	public partial class SettingsPage : ContentPage
	{
		public SettingsPage ()
		{
			InitializeComponent ();
		}

        // Units and measurements
        //   Kilograms (Kgs)                                            <- combo to select one or the other
        //   Pounds (Lbs)
        
        // Training Rules 
        // (you can override in Exercises) 
        //
        // Training rules are ON / OFF
        //
        // Total reps to advance to next weight:                       12 <- combo with values  0, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18...50
        // Reps when starting training on a weight for the first time:  5 <- combo with values 0..50
        
        // Workout goal in Reps
        // (number of Reps to increase performance in Reps since the last workout or how many more Reps aim to do each time)
        //    +1
        //    +2
        //    +1 every 2 times
        //    +1 every 3 times
        //    +1 every 4 times

        // Rest timer 
        //  hide rest timer
        //  30 secs
        //  45 secs
        //  1 min
        //  1 min 30 secs
        //  1 min 45 secs
        //  2 min
        //  2 min 30 secs
        
        // Exercises page
        // have options to override all training rules (rest timer included)
        // where to have it? see UI patterns 
        //   app toolbar button: Training Rules?
        //   button Training Rules?
        //   tabs: Basic / Advanced?
	}
}

