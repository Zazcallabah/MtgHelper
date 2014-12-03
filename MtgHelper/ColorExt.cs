using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace MtgHelper
{
	public static class ColorExt
	{
		public static Bitmap Cut( this Bitmap b, ImageArea area )
		{
			return b.Cut( area.Topleft, area.Bottomright );
		}
		public static Bitmap Cut( this Bitmap b, Point topleft, Point bottomright )
		{
			return b.Cut( topleft.X, topleft.Y, bottomright.X - topleft.X, bottomright.Y - topleft.Y );
		}

		public static Bitmap Cut( this Bitmap b, int x, int y, int width, int height )
		{
			var r = new Rectangle( x, y, width, height );

			var copy = new Bitmap( r.Width, r.Height );
			using( Graphics g = Graphics.FromImage( copy ) )
			{
				g.DrawImage( b, 0, 0, r, GraphicsUnit.Pixel );
			}
			return copy;
		}
		public static Bitmap Scale( this Bitmap b, float f )
		{
			var w = (int) ( b.Width * f );
			var h = (int) ( b.Height * f );
			Bitmap result = new Bitmap( w, h );
			using( Graphics g = Graphics.FromImage( result ) )
			{
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
				g.DrawImage( b, 0, 0, w, h );
			}
			return result;

		}


		public static long Crc32( this Bitmap bitmap, Rectangle bounds )
		{
			var crc = new Crc32();
			BitmapData bmpdata = bitmap.LockBits( bounds, ImageLockMode.ReadOnly, bitmap.PixelFormat );
			int numbytes = bmpdata.Stride * bmpdata.Height;
			byte[] bytedata = new byte[numbytes];
			IntPtr ptr = bmpdata.Scan0;

			Marshal.Copy( ptr, bytedata, 0, numbytes );

			bitmap.UnlockBits( bmpdata );
			crc.Update( bytedata );
			return crc.Value;
		}

		public static long Crc32( this Bitmap bitmap, Point topLeft, Point bottomRight )
		{
			return Crc32( bitmap, new Rectangle( topLeft, new Size( bottomRight ) ) );
		}
		public static long Crc32( this Bitmap bitmap )
		{
			return Crc32( bitmap, new Rectangle( 0, 0, bitmap.Width, bitmap.Height ) );
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
			return value < 30;
		}
	}
}