using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;

namespace MtgHelper
{
	/// <summary>
	/// Provides functions to capture the entire screen, or a particular window, and save it to a file.
	/// </summary>
	public class ScreenCapture
	{
		public IntPtr GetHandle()
		{
			return User32.GetForegroundWindow();
		}

		public Bitmap CaptureScreen( IntPtr windowHandle )
		{
			var rect = new User32.Rect();
			User32.GetWindowRect( windowHandle, ref rect );
			var bounds = new Rectangle( rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top );
			if( bounds.Width == 0 || bounds.Height == 0 )
				return null;
			var result = new Bitmap( bounds.Width, bounds.Height );
			using( var g = Graphics.FromImage( result ) )
			{
				g.CopyFromScreen( new Point( bounds.Left, bounds.Top ), Point.Empty, bounds.Size );
			}
			return result;
		}

		/// <summary>
		/// Helper class containing User32 API functions
		/// </summary>
		private class User32
		{
			[StructLayout( LayoutKind.Sequential )]
			public struct Rect
			{
				public int Left;
				public int Top;
				public int Right;
				public int Bottom;
			}

			[DllImport( "user32.dll" )]
			public static extern IntPtr GetWindowRect( IntPtr hWnd, ref Rect rect );

			[DllImport( "user32.dll" )]
			public static extern IntPtr GetForegroundWindow();
		}
	}
}
