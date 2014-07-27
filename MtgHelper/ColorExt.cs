using System;
using System.Drawing;

namespace MtgHelper
{
	public static class ColorExt
	{
		public static void Mask( this Bitmap b )
		{
			for( int x = 0; x < b.Width; x++ )
				for( int y = 0; y < b.Height; y++ )
				{
					var px = b.GetPixel( x, y );
					if( px.IsColor() || !px.IsDark() )
						b.SetPixel( x, y, Color.White );
				}
		}
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

		public static bool IsDark( this Color c )
		{
			const int threshold = 160;
			return c.R < threshold && c.G < threshold && c.B < threshold;
		}

		public static bool IsColor( this Color c )
		{
			const int threshold = 60;
			var dRG = Math.Abs( c.R - c.G );
			var dGB = Math.Abs( c.G - c.B );
			var dBR = Math.Abs( c.B - c.R );
			return dRG > threshold || dGB > threshold || dBR > threshold;
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