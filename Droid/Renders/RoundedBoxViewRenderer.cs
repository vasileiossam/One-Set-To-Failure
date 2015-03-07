using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Set.Droid.Renders;
using Android.Views;
using Android.Widget;
using Set.Controls;
using Android.Graphics;

[assembly: ExportRenderer (typeof (RoundedBoxView), typeof (RoundedBoxViewRenderer))]

namespace Set.Droid.Renders
{
	/// <summary>
	/// http://forums.xamarin.com/discussion/17792/video-on-making-custom-renderers
	/// </summary>
	public class RoundedBoxViewRenderer : BoxRenderer
	{
		public RoundedBoxViewRenderer()
		{
			this.SetWillNotDraw(false);
		}

		public override void Draw(Canvas canvas)
		{
			RoundedBoxView rbv = (RoundedBoxView)this.Element;

			Rect rc = new Rect();
			GetDrawingRect(rc);

			Rect interior = rc;
			interior.Inset((int)rbv.StrokeThickness, (int)rbv.StrokeThickness);

			Paint p = new Paint() {
				Color = rbv.Color.ToAndroid(),
				AntiAlias = true,
			};

			canvas.DrawRoundRect(new RectF(interior), (float)rbv.CornerRadius, (float)rbv.CornerRadius, p);

			p.Color = rbv.Stroke.ToAndroid();
			p.StrokeWidth = (float)rbv.StrokeThickness;
			p.SetStyle(Paint.Style.Stroke);

			canvas.DrawRoundRect(new RectF(rc), (float)rbv.CornerRadius, (float)rbv.CornerRadius, p);
		}


	}
}

