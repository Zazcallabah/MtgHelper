using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using NUnit.Framework;
using MtgHelper;

namespace UnitTestProject1
{
	[TestFixture]
	public class UnitTest1
	{

		[Test]
		public void FindColumn()
		{
			var img = System.Drawing.Image.FromFile( @"C:\src\git\MtgHelper\MtgHelper\nozoom.bmp" );
			var b = new Bitmap( img );
			var tl = new Point( 286, 121 );
			var br = new Point( 286 + 1049, 121 + 433 );
			b.DrawRectangle( tl, br );
			for( int i = 0; i < b.Width; i++ )
				for( int j = 0; j < b.Height; j++ )
					Assert.IsTrue( b.GetPixel( i, j ).IsGray() );

			var s = new Shot( b, tl, br );
			var c = s.GetColumns();
			//Point p = null;

		}


	}

}
