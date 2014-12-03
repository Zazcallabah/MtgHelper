using System.Drawing;

namespace MtgHelper.Misc
{
	public class ScanArea
	{
		public ScanArea( Point tl, Point br )
		{
			TopLeft = tl;
			BottomRight = br;
		}
		public Point TopLeft { get; private set; }
		public Point BottomRight { get; private set; }
	}
}