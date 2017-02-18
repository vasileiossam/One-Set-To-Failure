namespace OneSet
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
		Small,
	}

	public class ScreenSizeHandler
	{
		private double _width;
		private double _height;

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
			if ((App.ScreenWidth <= 480) || (App.ScreenHeight <= 480))
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

