using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using MtgHelper;
using MtgHelper.Misc;
using NUnit.Framework;

namespace MtgHelperTests
{
	[TestFixture]
	public class UnitTest1
	{
		[Test]
		public void Runocr()
		{
			var s = "1936114076.png";
			var ocr = new TesseractOcr( @".\tesseract.exe" );
			Assert.AreEqual( "Battlefield Forge", ocr.OCRFromFile( s ).Trim() );
		}

		[Test]
		public void FindColumn()
		{
			var d = new Dictionary<long, Bitmap>();
			var img = (Bitmap) System.Drawing.Image.FromFile( @"C:\src\git\MtgHelper\MtgHelper\nozoom.bmp" );
			var sa = ScanAreaFactory.Get( img );
			var sh = new Shot( img, sa );
			var c = sh.GetCards();
			foreach( var a in c )
			{
				var i = a.Data();
				var crc = i.Crc32();
				if( !d.ContainsKey( crc ) )
					d.Add( crc, i );
			}

			foreach( var kvp in d )
			{
				kvp.Value.Save( kvp.Key.ToString() + ".png", ImageFormat.Png );
			}
			/*			var b = new Bitmap( img );
						var tl = new Point( 286, 121 );
						var br = new Point( 286 + 1049, 121 + 433 );
						b.DrawRectangle( tl, br );
						for( int i = 0; i < b.Width; i++ )
							for( int j = 0; j < b.Height; j++ )
								Assert.IsTrue( b.GetPixel( i, j ).IsGray() );

						var s = new Shot( b, tl, br );
						var c = s.GetColumns();
						//Point p = null;
						*/
		}


	}

}
