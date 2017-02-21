using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using OneSet.Droid.Renders;
using Android.Graphics;
using OneSet.Controls;

[assembly: ExportRenderer (typeof (RoundedBoxView), typeof (RoundedBoxViewRenderer))]

namespace OneSet.Droid.Renders
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
			var rbv = (RoundedBoxView)Element;

			var rc = new Rect();
			GetDrawingRect(rc);

			var interior = rc;
			interior.Inset((int)rbv.StrokeThickness, (int)rbv.StrokeThickness);

			var p = new Paint
			{
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

