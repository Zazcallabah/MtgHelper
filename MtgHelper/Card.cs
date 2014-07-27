using System.Drawing;

namespace MtgHelper
{
	public class Card
	{
		public System.Drawing.Point TopLeft { get; set; }
		public System.Drawing.Point BottomRight { get; set; }

		public void Draw( Bitmap b )
		{
			b.DrawRectangle( TopLeft, BottomRight );
		}
	}
}