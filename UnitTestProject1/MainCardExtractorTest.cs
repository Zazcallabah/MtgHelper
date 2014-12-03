using System;
using System.Drawing;
using MtgHelper;
using NUnit.Framework;

namespace MtgHelperTests
{
	[TestFixture]
	public class MainCardExtractorTest
	{
		[Test]
		public void CanIdentifySmallCards_AlmostAlignedToTop_WhereRightMarginIsLargerThanCard()
		{
			var s = "tmp_oteu000m.png";
			var b = (Bitmap) System.Drawing.Image.FromFile( s );
			long mark = DateTime.Now.Ticks;
			var d = new CollectionDetector( b );
			var col = d.CollectionArea();
			var ext = new CardCollectionExtractor( b, col );
			var cards = ext.Cards();
			Assert.AreEqual( 24, cards.Count );
		}

		[Test]
		public void CanIdentifySmallCards_WithSmallRightMargin()
		{
			var s = "tmp_j4mgynou.png";
			var b = (Bitmap) System.Drawing.Image.FromFile( s );
			long mark = DateTime.Now.Ticks;
			var d = new CollectionDetector( b );
			var col = d.CollectionArea();
			var ext = new CardCollectionExtractor( b, col );
			var cards = ext.Cards();
			Assert.AreEqual( 27, cards.Count );
		}
		[Test]
		public void CanIdentifySmallCards_NeitherAlignedInTopOrBottom()
		{
			var s = "tmp_gkc44din.png";
			var b = (Bitmap) System.Drawing.Image.FromFile( s );
			long mark = DateTime.Now.Ticks;
			var d = new CollectionDetector( b );
			var col = d.CollectionArea();
			var ext = new CardCollectionExtractor( b, col );
			var cards = ext.Cards();
			Assert.AreEqual( 16, cards.Count );
		}


		[Test]
		public void CanIdentifyBigCards_AlignedToTop()
		{
			var s = "tmp_lorf5ggn.png";
			var b = (Bitmap) System.Drawing.Image.FromFile( s );
			long mark = DateTime.Now.Ticks;
			var d = new CollectionDetector( b );
			var col = d.CollectionArea();
			var ext = new CardCollectionExtractor( b, col );
			var cards = ext.Cards();
			Assert.AreEqual( 4, cards.Count );
		}
		[Test]
		public void CanIdentifyBigCards_NotAlignedToTop()
		{
			var s = "tmp_ukrrv23p.png";
			var b = (Bitmap) System.Drawing.Image.FromFile( s );
			var d = new CollectionDetector( b );
			var col = d.CollectionArea();
			var ext = new CardCollectionExtractor( b, col );
			var cards = ext.Cards();
			Assert.AreEqual( 3, cards.Count );
		}
	}
}