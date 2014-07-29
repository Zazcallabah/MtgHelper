using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace MtgHelper
{
	public static class ColorExt
	{
		public static long Crc32( this Bitmap bitmap )
		{
			var crc = new Crc32();
			BitmapData bmpdata = bitmap.LockBits( new Rectangle( 0, 0, bitmap.Width, bitmap.Height ), ImageLockMode.ReadOnly, bitmap.PixelFormat );
			int numbytes = bmpdata.Stride * bitmap.Height;
			byte[] bytedata = new byte[numbytes];
			IntPtr ptr = bmpdata.Scan0;

			Marshal.Copy( ptr, bytedata, 0, numbytes );

			bitmap.UnlockBits( bmpdata );
			crc.Update( bytedata );
			return crc.Value;
		}
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
		public static void DrawRectangle( this Bitmap b, Point tl, Point br )
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
			var dRg = Math.Abs( c.R - c.G );
			var dGb = Math.Abs( c.G - c.B );
			var dBr = Math.Abs( c.B - c.R );
			return dRg > threshold || dGb > threshold || dBr > threshold;
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