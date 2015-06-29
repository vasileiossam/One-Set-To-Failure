using System;

namespace Set.Abstract
{
	public interface IScreenSizeHandler
	{
		void InitScreenSizeHandler(); 
		void ChangeOrientation();
		//void OnSizeAllocated(double width, double height);
	}
}

