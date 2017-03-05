using OneSet.Services;

namespace OneSet.Abstract
{
	public interface IScreenSizeHandler
	{
	    Orientations GetStartingOrientation();
	    ScreenSizes GetScreenSize();
	    Orientations OnSizeAllocated(double width, double height);
	}
}

