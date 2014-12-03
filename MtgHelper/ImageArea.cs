using System.Drawing;

namespace MtgHelper
{
	public class ImageArea
	{
		public ImageArea( Point topleft, Point bottomright )
		{
			Bottomright = bottomright;
			Topleft = topleft;
		}

		public Point Topleft { get; private set; }
		public Point Bottomright { get; private set; }
	}
}