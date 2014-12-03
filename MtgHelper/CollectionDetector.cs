using System.Drawing;

namespace MtgHelper
{
	/// <summary>
	/// Given a main window, this class will cut out the main card collection area
	/// </summary>
	public class CollectionDetector
	{
		readonly Bitmap _b;

		public CollectionDetector( Bitmap bitmap )
		{
			_b = bitmap;
		}

		public ImageArea CollectionArea()
		{
			var x = _b.Width - 2;
			var y = 100;
			while( _b.GetPixel( x, y ).IsBlack() )
				x--;

			while( !_b.GetPixel( x, y ).IsBlack() )
				y--;
			y++;

			var x1 = x;
			var y1 = y;

			while( !_b.GetPixel( x, y ).IsBlack() )
				x--;
			x++;

			while( !_b.GetPixel( x, y ).IsBlack() )
				y++;
			y--;

			var x2 = x;
			var y2 = y;


			return new ImageArea( new Point( x2, y1 ), new Point( x1, y2 ) );

		}
	}
}