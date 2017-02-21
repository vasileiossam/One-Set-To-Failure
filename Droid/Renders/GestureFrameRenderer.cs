using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using OneSet.Droid.Renders;
using Android.Views;
using OneSet.Controls;

[assembly: ExportRenderer(typeof(GestureFrame), typeof(GestureFrameRenderer))]
namespace OneSet.Droid.Renders
{
	public class GestureFrameRenderer : FrameRenderer
	{
		private readonly CustomGestureListener _listener;
		private readonly GestureDetector _detector;

		public GestureFrameRenderer ()
		{
			_listener = new CustomGestureListener();
			_detector = new GestureDetector(_listener);
		}

		protected override void OnElementChanged (ElementChangedEventArgs<Frame> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement == null)
			{
				GenericMotion -= HandleGenericMotion;
				Touch -= HandleTouch;
				_listener.OnSwipeLeft -= HandleOnSwipeLeft;
				_listener.OnSwipeRight -= HandleOnSwipeRight;
				_listener.OnSwipeTop -= HandleOnSwipeTop;
				_listener.OnSwipeDown -= HandleOnSwipeDown;
			}

		    if (e.OldElement != null) return;
		    GenericMotion += HandleGenericMotion;
		    Touch += HandleTouch;
		    _listener.OnSwipeLeft += HandleOnSwipeLeft;
		    _listener.OnSwipeRight += HandleOnSwipeRight;
		    _listener.OnSwipeTop += HandleOnSwipeTop;
		    _listener.OnSwipeDown += HandleOnSwipeDown;
		}

		void HandleTouch(object sender, TouchEventArgs e)
		{
			_detector.OnTouchEvent(e.Event);
		}

		void HandleGenericMotion(object sender, GenericMotionEventArgs e)
		{
			_detector.OnTouchEvent(e.Event);
		}

		void HandleOnSwipeLeft(object sender, EventArgs e)
		{
			var gi = (GestureFrame)Element;
			gi.OnSwipeLeft();
		}

		void HandleOnSwipeRight(object sender, EventArgs e)
		{
			var gi = (GestureFrame)Element;
			gi.OnSwipeRight();
		}

		void HandleOnSwipeTop(object sender, EventArgs e)
		{
			var gi = (GestureFrame)Element;
			gi.OnSwipeTop();
		}

		void HandleOnSwipeDown(object sender, EventArgs e)
		{
			var gi = (GestureFrame)Element;
			gi.OnSwipeDown();
		}
	}
}