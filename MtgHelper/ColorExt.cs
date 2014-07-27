using System.Drawing;

namespace MtgHelper
{
	public static class ColorExt
	{
		public static void DrawRectangle( this Bitmap b, System.Drawing.Point tl, System.Drawing.Point br )
		{
			for( int x = tl.X; x < br.X; x++ )
			{
				b.SetPixel( x, tl.Y, Color.Red );
				b.SetPixel( x, br.Y, Color.Red );
			}
			for( int y = tl.Y; y < br.Y; y++ )
			{
				b.SetPixel( tl.X, y, Color.Red );
				b.SetPixel( br.X, y, Color.Red );
			}
		}

		public static bool IsGray( this Color c )
		{
			return IsGray( c.R ) && IsGray( c.G ) && IsGray( c.B );
		}
		public static bool IsBlack( this Color c )
		{
			return IsBlack( c.R ) && IsBlack( c.G ) && IsBlack( c.B );
		}
		static bool IsGray( int value )
		{
			return value >= 220 && value <= 240;
		}

		static bool IsBlack( int value )
		{
			return value < 20;
		}
	}
}