using System;
using Xamarin.Forms;

namespace Set
{
	public enum Orientations  
	{
		Default,
		Landscape,
		Portrait
	}

	public enum ScreenSizes
	{
		Default,
		Small
	}

	public class ScreenSizeHandler
	{
		private double _width = 0.0;
		private double _height = 0.0;

		public ScreenSizeHandler ()
		{

		}

		public Orientations GetStartingOrientation()
		{
			if (App.ScreenWidth > App.ScreenHeight)
			{
				return Orientations.Landscape;
			} 
			else
			{
				return Orientations.Portrait;
			}
		}

		public ScreenSizes GetScreenSize()
		{
			if ((App.ScreenWidth <= 320) || (App.ScreenHeight <= 320))
			{
				return ScreenSizes.Small;
			}

			return ScreenSizes.Default;
		}

		public Orientations OnSizeAllocated(double width, double height)
		{
			if (width != _width || height != _height)
			{
				_width = width;
				_height = height;

				if (width > height)
				{
					return Orientations.Landscape;
				} 
				else
				{
					return Orientations.Portrait;
				}
			} 

			return Orientations.Default;
		}
	}
}

