using System.Drawing;

namespace MtgHelper
{
	public class Card
	{
		readonly Bitmap _b;

		public Card( Bitmap b )
		{
			_b = b;
		}

		public Point TopLeft { get; set; }
		public Point BottomRight { get; set; }

		public void Draw()
		{
			_b.DrawRectangle( TopLeft, BottomRight );
		}

		public Bitmap Data()
		{
			var dx = BottomRight.X - TopLeft.X;
			var dy = BottomRight.Y - TopLeft.Y;
			var r = new Rectangle( TopLeft, new Size( dx, dy ) );

			Bitmap bmp = new Bitmap( r.Width, r.Height );
			using( Graphics g = Graphics.FromImage( bmp ) )
			{
				g.DrawImage( _b, 0, 0, r, GraphicsUnit.Pixel );
			}
			bmp.Mask();
			return bmp;
		}
	}
}