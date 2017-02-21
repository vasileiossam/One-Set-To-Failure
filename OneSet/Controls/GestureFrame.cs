using System;
using Xamarin.Forms;

namespace OneSet.Controls
{
	public class GestureFrame : Frame
	{
	    public event EventHandler SwipeDown;
		public event EventHandler SwipeTop;
		public event EventHandler SwipeLeft;
		public event EventHandler SwipeRight;

		public void OnSwipeDown()
		{
            SwipeDown?.Invoke(this, null);
        }

		public void OnSwipeTop()
		{
            SwipeTop?.Invoke(this, null);
        }

		public void OnSwipeLeft()
		{
            SwipeLeft?.Invoke(this, null);
        }

		public void OnSwipeRight()
		{
            SwipeRight?.Invoke(this, null);
        }
	}
}