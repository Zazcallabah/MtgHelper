using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace MtgHelper.Misc
{
	public class ScanAreaFactory
	{
		public static ScanArea Get( Bitmap b )
		{
			var x = 1;
			var y = b.Height / 2;
			for( ; x < b.Width; x++ )
			{
				if( !b.GetPixel( x, y ).IsBlack() )
					break;
			}

			for( ; y >= 0; y-- )
			{
				if( b.GetPixel( x, y ).IsBlack() )
				{
					y++;
					break;
				}
			}

			if( y < 0 )
				return null;

			for( ; x < b.Width; x++ )
			{
				if( b.GetPixel( x, y ).IsBlack() )
					break;
			}

			// top left diagonal down right
			for( ; x < b.Width && y < b.Height; x++, y++ )
			{
				if( b.GetPixel( x, y ).IsGray() )
					break;
			}
			// up
			for( ; x >= 0; x-- )
			{
				if( !b.GetPixel( x, y ).IsGray() )
				{
					x++;
					break;
				}
			}

			// left
			for( ; y >= 0; y-- )
			{
				if( !b.GetPixel( x, y ).IsGray() )
				{
					y++;
					break;
				}
			}


			var tl = new Point( x, y );

			y++;

			for( ; x < b.Width; x++ )
			{
				if( !b.GetPixel( x, y ).IsGray() )
				{
					x -= 2;
					break;
				}
			}

			for( ; y < b.Height; y++ )
			{
				if( !b.GetPixel( x, y ).IsGray() )
				{
					y--;
					break;
				}
			}

			var br = new Point( x, y );

			return new ScanArea( tl, br );


		}
	}
}